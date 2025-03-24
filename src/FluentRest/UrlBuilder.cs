using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace FluentRest;

/// <summary>
/// Build and modify uniform resource locator (URL)
/// </summary>
public sealed partial class UrlBuilder
{
    //scheme:[//[user[:password]@]host[:port]][/path][?query][#fragment]

    private static readonly Dictionary<string, int?> _schemePorts = new(StringComparer.OrdinalIgnoreCase)
    {
        { "acap", 674 },{ "afp", 548 },{ "dict", 2628 },{ "dns", 53 },{ "file", null },{ "ftp", 21 },{ "git", 9418 },{ "gopher", 70 },
        { "http", 80 },{ "https", 443 },{ "imap", 143 },{ "ipp", 631 },{ "ipps", 631 },{ "irc", 194 },{ "ircs", 6697 },{ "ldap", 389 },
        { "ldaps", 636 },{ "mms", 1755 },{ "msrp", 2855 },{ "msrps", null },{ "mtqp", 1038 },{ "nfs", 111 },{ "nntp", 119 },{ "nntps", 563 },
        { "pop", 110 },{ "prospero", 1525 },{ "redis", 6379 },{ "rsync", 873 },{ "rtsp", 554 },{ "rtsps", 322 },{ "rtspu", 5005 },{ "sftp", 22 },
        { "smb", 445 },{ "snmp", 161 },{ "ssh", 22 },{ "steam", null },{ "svn", 3690 },{ "telnet", 23 },{ "ventrilo", 3784 },{ "vnc", 5900 },
        { "wais", 210 },{ "ws", 80 },{ "wss", 443 },{ "xmpp", null }
    };

    private const string _urlParseExpression = @"^(?:(?<scheme>[a-z][a-z0-9+\-.]*):\/\/)?(?:(?<username>[^:]*)(?::(?<password>[^@]*))?@)?(?<host>[^\/?#:]*)(?::(?<port>\d+))?(?<path>[^?#]*)(?:\?(?<query>[^#]*))?(?:#(?<fragment>.*))?$";

#if NET7_0_OR_GREATER
    [GeneratedRegex(_urlParseExpression)]
    private static partial Regex UrlParseRegex();
#else
    private static readonly Lazy<Regex> _urlParseRegex = new(() => new(_urlParseExpression));
    private static Regex UrlParseRegex() => _urlParseRegex.Value;
#endif

    /// <summary>
    /// Initializes a new instance of the <see cref="UrlBuilder"/> class.
    /// </summary>
    public UrlBuilder()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UrlBuilder"/> with the specified <paramref name="url"/>.
    /// </summary>
    /// <param name="url">A URL string.</param>
    public UrlBuilder(string? url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return;

        ParseUrl(url);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UrlBuilder"/> with the specified <paramref name="uri"/>.
    /// </summary>
    /// <param name="uri">An instance of the <see cref="Uri"/> class</param>
    /// <exception cref="ArgumentNullException">uri is null</exception>
    public UrlBuilder(Uri uri)
    {
        if (uri is null)
            throw new ArgumentNullException(nameof(uri));

        SetFieldsFromUri(uri);
    }


    /// <summary>
    /// Gets the scheme name of the Url.
    /// </summary>
    /// <value>
    /// The scheme name of the Url.
    /// </value>
    public string? Scheme { get; private set; }

    /// <summary>
    /// Gets the user name associated with the user that accesses the Url.
    /// </summary>
    /// <value>
    /// The user name associated with the user that accesses the Url.
    /// </value>
    public string? UserName { get; private set; }

    /// <summary>
    /// Gets the password associated with the user that accesses the Url.
    /// </summary>
    /// <value>
    /// The password associated with the user that accesses the Url.
    /// </value>
    public string? Password { get; private set; }

    /// <summary>
    /// Gets the Domain Name System (DNS) host name or IP address of a server.
    /// </summary>
    /// <value>
    /// The Domain Name System (DNS) host name or IP address of a server.
    /// </value>
    public string? Host { get; private set; }

    /// <summary>
    /// Gets the port number of the Url.
    /// </summary>
    /// <value>
    /// The port number of the Url.
    /// </value>
    public int? Port { get; private set; }


    /// <summary>
    /// Gets the path segment collection to the resource referenced by the Url.
    /// </summary>
    /// <value>
    /// The path segment collection to the resource referenced by the Url.
    /// </value>
    public List<string> Path { get; } = [];

    /// <summary>
    /// Gets the query string dictionary information included in the Url.
    /// </summary>
    /// <value>
    /// The query string dictionary information included in the Url.
    /// </value>
    public NameValueCollection Query { get; } = [];

    /// <summary>
    /// Gets the fragment portion of the Url.
    /// </summary>
    /// <value>
    /// The fragment portion of the Url.
    /// </value>
    public string? Fragment { get; private set; }


    /// <summary>
    /// Replace the schema name for the current Url.
    /// </summary>
    /// <param name="value">The schema name.</param>
    /// <returns></returns>
    public UrlBuilder SetScheme(string? value)
    {
        Scheme = value;

        if (Scheme is null)
            return this;

        int index = Scheme.IndexOf(':');
        if (index != -1)
            Scheme = Scheme.Substring(0, index);

        if (Scheme.Length != 0)
        {
            if (!Uri.CheckSchemeName(value))
                throw new ArgumentException("Invalid URI: The URI scheme is not valid.", nameof(value));

            Scheme = Scheme.ToLowerInvariant();
        }

        return this;
    }

    /// <summary>
    /// Replace the user name for the current Url.
    /// </summary>
    /// <param name="value">The user name associated with the user that access the Url.</param>
    /// <param name="unescape">Converts a url string to its unescaped representation.</param>
    /// <returns></returns>
    public UrlBuilder SetUserName(string? value, bool unescape = false)
    {
        UserName = value != null && unescape ? Uri.UnescapeDataString(value) : value;
        return this;
    }

    /// <summary>
    /// Replace the password for the current Url.
    /// </summary>
    /// <param name="value">The password associated with the user that access the Url.</param>
    /// <param name="unescape">Converts a url string to its unescaped representation.</param>
    /// <returns></returns>
    public UrlBuilder SetPassword(string? value, bool unescape = false)
    {
        Password = value != null && unescape ? Uri.UnescapeDataString(value) : value;
        return this;
    }

    /// <summary>
    /// Replace the Domain Name System (DNS) host name or IP address for the current Url.
    /// </summary>
    /// <param name="value">The Domain Name System (DNS) host name or IP address.</param>
    /// <returns></returns>
    public UrlBuilder SetHost(string? value)
    {
        Host = value;

        if (Host is null)
            return this;

        //probable ipv6 address - Note: this is only supported for cases where the authority is inet-based.
        if (Host.Contains(':'))
        {
            //set brackets
            if (Host[0] != '[')
                Host = "[" + Host + "]";
        }

        return this;
    }

    /// <summary>
    /// Replace the port number for the current Url.
    /// </summary>
    /// <param name="value">The port number.</param>
    /// <returns></returns>
    public UrlBuilder SetPort(int value)
    {
        if (value < -1 || value > 0xFFFF)
            throw new ArgumentOutOfRangeException(nameof(value));

        Port = value;
        return this;
    }

    /// <summary>
    /// Replace the port number for the current Url.
    /// </summary>
    /// <param name="value">The port number.</param>
    /// <returns></returns>
    public UrlBuilder SetPort(string? value)
    {
        if (!string.IsNullOrEmpty(value) && int.TryParse(value, out int port))
            return SetPort(port);

        Port = null;
        return this;
    }

    /// <summary>
    /// Replace the fragment portion for the current Url.
    /// </summary>
    /// <param name="value">The fragment portion.</param>
    /// <returns></returns>
    public UrlBuilder SetFragment(string? value)
    {
        Fragment = value;
        if (Fragment is null)
            return this;

        if (Fragment.Length > 0 && Fragment[0] != '#')
            Fragment = '#' + Fragment;

        return this;
    }


    /// <summary>
    /// Appends a path segment to the current Url.
    /// </summary>
    /// <param name="path">The path segment to append.</param>
    /// <returns></returns>
    public UrlBuilder AppendPath(Uri? path)
    {
        if (path is null)
            return this;

        ParsePath(path.AbsolutePath);

        return this;
    }

    /// <summary>
    /// Appends a path segment to the current Url.
    /// </summary>
    /// <param name="path">The path segment to append.</param>
    /// <returns></returns>
    public UrlBuilder AppendPath(string? path)
    {
        if (path is null)
            return this;

        ParsePath(path);

        return this;
    }

    /// <summary>
    /// Appends a path segment to the current Url.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="path">The path segment to append.</param>
    /// <returns></returns>
    public UrlBuilder AppendPath<TValue>(TValue? path)
    {
        if (path is null)
            return this;

        var v = path.ToString();
        ParsePath(v);

        return this;
    }

    /// <summary>
    /// Appends the path segments to the current Url.
    /// </summary>
    /// <param name="paths">The path segments to append.</param>
    /// <returns></returns>
    public UrlBuilder AppendPaths(params IEnumerable<string>? paths)
    {
        if (paths is null)
            return this;

        foreach (var path in paths)
            ParsePath(path);

        return this;
    }

    /// <summary>
    /// Appends a string formatted path segment to the current Url.
    /// </summary>
    /// <param name="format">A composite format string.</param>
    /// <param name="arguments">An array that contains zero or more objects to format.</param>
    /// <returns></returns>
    public UrlBuilder AppendPathFormat(string format, params object[] arguments)
    {
        var p = string.Format(format, arguments);

        return AppendPath(p);
    }


    /// <summary>
    /// Conditionally appends a path segment to the current url if the specified <paramref name="condition" /> is <c>true</c>.
    /// </summary>
    /// <param name="condition">The condition on weather the query string is appended.</param>
    /// <param name="path">The path segment to append.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">name is <c>null</c></exception>
    public UrlBuilder AppendPathIf(Func<bool> condition, string? path)
    {
        if (condition is null || !condition())
            return this;

        return AppendPath(path);
    }

    /// <summary>
    /// Conditionally appends a path segment to the current url if the specified <paramref name="condition" /> is <c>true</c>.
    /// </summary>
    /// <param name="condition">The condition on weather the query string is appended.</param>
    /// <param name="path">The path segment to append.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">name is <c>null</c></exception>
    public UrlBuilder AppendPathIf(Func<string?, bool> condition, string? path)
    {
        if (condition is null || !condition(path))
            return this;

        return AppendPath(path);
    }

    /// <summary>
    /// Conditionally appends a path segment to the current url if the specified <paramref name="condition" /> is <c>true</c>.
    /// </summary>
    /// <param name="condition">The condition on weather the query string is appended.</param>
    /// <param name="path">The path segment to append.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">name is <c>null</c></exception>
    public UrlBuilder AppendPathIf(bool condition, string? path)
    {
        if (!condition)
            return this;

        return AppendPath(path);
    }

    /// <summary>
    /// Conditionally appends a path segment to the current url if the specified <paramref name="condition" /> is <c>true</c>.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="condition">The condition on weather the query string is appended.</param>
    /// <param name="path">The path segment to append.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">name is <c>null</c></exception>
    public UrlBuilder AppendPathIf<TValue>(Func<bool> condition, TValue? path)
    {
        if (condition is null || !condition())
            return this;

        return AppendPath(path);
    }

    /// <summary>
    /// Conditionally appends a path segment to the current url if the specified <paramref name="condition" /> is <c>true</c>.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="condition">The condition on weather the query string is appended.</param>
    /// <param name="path">The path segment to append.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">name is <c>null</c></exception>
    public UrlBuilder AppendPathIf<TValue>(Func<TValue?, bool> condition, TValue? path)
    {
        if (condition is null || !condition(path))
            return this;

        return AppendPath(path);
    }

    /// <summary>
    /// Conditionally appends a path segment to the current url if the specified <paramref name="condition" /> is <c>true</c>.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="condition">The condition on weather the query string is appended.</param>
    /// <param name="path">The path segment to append.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">name is <c>null</c></exception>
    public UrlBuilder AppendPathIf<TValue>(bool condition, TValue? path)
    {
        if (!condition)
            return this;

        return AppendPath(path);
    }


    /// <summary>
    /// Replace the entire path for the current Url.  The <see cref="Path"/> collection is replaced with this path.
    /// </summary>
    /// <param name="path">The path segment to set.</param>
    /// <returns></returns>
    public UrlBuilder SetPath(string? path)
    {
        Path.Clear();
        if (path is null)
            return this;

        ParsePath(path);
        return this;
    }


    /// <summary>
    /// Appends the query string name and value to the current url.
    /// </summary>
    /// <param name="name">The query string name.</param>
    /// <param name="value">The query string value.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">name is <c>null</c></exception>
    public UrlBuilder AppendQuery(string name, string? value)
    {
        if (name is null)
            throw new ArgumentNullException(nameof(name));

        Query.Add(name, value);

        return this;
    }

    /// <summary>
    /// Appends the query string name and value to the current url.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="name">The query string name.</param>
    /// <param name="value">The query string value.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">name is <c>null</c></exception>
    public UrlBuilder AppendQuery<TValue>(string name, TValue? value)
    {
        if (name is null)
            throw new ArgumentNullException(nameof(name));

        var v = value?.ToString();
        return AppendQuery(name, v);
    }


    /// <summary>
    /// Appends the query string name and values to the current url.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="name">The query string name.</param>
    /// <param name="values">The query string values.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">name is <c>null</c></exception>
    public UrlBuilder AppendQueries<TValue>(string name, IEnumerable<TValue>? values)
    {
        if (name is null)
            throw new ArgumentNullException(nameof(name));

        if (values is null)
            return this;

        foreach (var value in values)
        {
            var v = value?.ToString();
            AppendQuery(name, v);
        }

        return this;
    }

    /// <summary>
    /// Appends the query string name and values to the current url.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="values">The query string values.</param>
    /// <returns></returns>
    public UrlBuilder AppendQueries<TValue>(IEnumerable<KeyValuePair<string, TValue>>? values)
    {
        if (values is null)
            return this;

        foreach (var value in values)
        {
            var n = value.Key;
            var v = value.Value?.ToString();

            AppendQuery(n, v);
        }

        return this;
    }

    /// <summary>
    /// Appends the query string name and values to the current url.
    /// </summary>
    /// <param name="values">The query string values.</param>
    /// <returns></returns>
    public UrlBuilder AppendQueries(NameValueCollection? values)
    {

        if (values is null)
            return this;

        Query.Add(values);

        return this;
    }


    /// <summary>
    /// Conditionally appends the query string name and value to the current url if the specified <paramref name="condition" /> is <c>true</c>.
    /// </summary>
    /// <param name="condition">The condition on weather the query string is appended.</param>
    /// <param name="name">The query string name.</param>
    /// <param name="value">The query string value.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">name is <c>null</c></exception>
    public UrlBuilder AppendQueryIf(Func<bool> condition, string name, string? value)
    {
        if (condition is null || !condition())
            return this;

        return AppendQuery(name, value);
    }

    /// <summary>
    /// Conditionally appends the query string name and value to the current url if the specified <paramref name="condition" /> is <c>true</c>.
    /// </summary>
    /// <param name="condition">The condition on weather the query string is appended.</param>
    /// <param name="name">The query string name.</param>
    /// <param name="value">The query string value.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">name is <c>null</c></exception>
    public UrlBuilder AppendQueryIf(Func<string?, bool> condition, string name, string? value)
    {
        if (condition is null || !condition(value))
            return this;

        return AppendQuery(name, value);
    }

    /// <summary>
    /// Conditionally appends the query string name and value to the current url if the specified <paramref name="condition" /> is <c>true</c>.
    /// </summary>
    /// <param name="condition">The condition on weather the query string is appended.</param>
    /// <param name="name">The query string name.</param>
    /// <param name="value">The query string value.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">name is <c>null</c></exception>
    public UrlBuilder AppendQueryIf(bool condition, string name, string? value)
    {
        if (!condition)
            return this;

        return AppendQuery(name, value);
    }

    /// <summary>
    /// Conditionally appends the query string name and value to the current url if the specified <paramref name="condition" /> is <c>true</c>.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="condition">The condition on weather the query string is appended.</param>
    /// <param name="name">The query string name.</param>
    /// <param name="value">The query string value.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">name is <c>null</c></exception>
    public UrlBuilder AppendQueryIf<TValue>(Func<bool> condition, string name, TValue? value)
    {
        if (condition is null || !condition())
            return this;

        return AppendQuery(name, value);
    }

    /// <summary>
    /// Conditionally appends the query string name and value to the current url if the specified <paramref name="condition" /> is <c>true</c>.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="condition">The condition on weather the query string is appended.</param>
    /// <param name="name">The query string name.</param>
    /// <param name="value">The query string value.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">name is <c>null</c></exception>
    public UrlBuilder AppendQueryIf<TValue>(Func<TValue?, bool> condition, string name, TValue? value)
    {
        if (condition is null || !condition(value))
            return this;

        return AppendQuery(name, value);
    }

    /// <summary>
    /// Conditionally appends the query string name and value to the current url if the specified <paramref name="condition" /> is <c>true</c>.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="condition">The condition on weather the query string is appended.</param>
    /// <param name="name">The query string name.</param>
    /// <param name="value">The query string value.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">name is <c>null</c></exception>
    public UrlBuilder AppendQueryIf<TValue>(bool condition, string name, TValue? value)
    {
        if (!condition)
            return this;

        return AppendQuery(name, value);
    }


    /// <summary>
    /// Appends the query string to the current url.
    /// </summary>
    /// <param name="queryString">The query string to append.</param>
    /// <returns></returns>
    public UrlBuilder AppendQuery(string? queryString)
    {
        if (queryString is null)
            return this;

        ParseQueryString(queryString);
        return this;
    }


    /// <summary>
    /// Replace the entire query string for the current Url.  The <see cref="Query"/> dictionary is replaced with this query string.
    /// </summary>
    /// <param name="queryString">The query string to set.</param>
    /// <returns></returns>
    public UrlBuilder SetQuery(string? queryString)
    {
        Query.Clear();
        if (queryString is null)
            return this;

        ParseQueryString(queryString);

        return this;
    }


    /// <summary>
    /// Returns a <see cref="System.Uri" /> that represents this instance.
    /// </summary>
    /// <returns>
    /// A <see cref="System.Uri" /> that represents this instance.
    /// </returns>
    public Uri ToUri()
    {
        var url = ToString();
        return new Uri(url, UriKind.RelativeOrAbsolute);
    }

    /// <summary>
    /// Returns a <see cref="System.String" /> that represents this instance.
    /// </summary>
    /// <returns>
    /// A <see cref="System.String" /> that represents this instance.
    /// </returns>
    public override string ToString()
    {
        var builder = StringBuilderCache.Acquire(150);

        if (!string.IsNullOrWhiteSpace(Scheme))
            builder.Append(Scheme).Append(Uri.SchemeDelimiter);

        if (!string.IsNullOrWhiteSpace(UserName))
        {
            builder.Append(Uri.EscapeDataString(UserName));
            if (!string.IsNullOrWhiteSpace(Password))
                builder.Append(':').Append(Uri.EscapeDataString(Password));

            builder.Append('@');
        }

        if (!string.IsNullOrWhiteSpace(Host))
        {
            builder.Append(Host);
            if (Port.HasValue && !IsStandardPort())
                builder.Append(':').Append(Port);
        }

        WritePath(builder);
        WriteQueryString(builder);

        if (!string.IsNullOrWhiteSpace(Fragment))
            builder.Append(Fragment);

        return StringBuilderCache.ToString(builder);
    }

    /// <summary>
    /// Creates a new <see cref="UrlBuilder"/> using the optional base path.
    /// </summary>
    /// <param name="baseUrl">The base path for the UrlBuilder</param>
    /// <returns>A new instance of UrlBuilder</returns>
    public static UrlBuilder Create(string? baseUrl = null) => new(baseUrl);


    private void WritePath(StringBuilder builder)
    {
        builder.Append('/');
        if (Path is null || Path.Count == 0)
            return;

        int start = builder.Length;
        foreach (var p in Path)
        {
            if (builder.Length > start)
                builder.Append('/');

            var v = Uri.EscapeDataString(p ?? string.Empty);

            builder.Append(v);
        }
    }

    private void WriteQueryString(StringBuilder builder)
    {
        if (Query is null || Query.Count == 0)
            return;

        builder.Append('?');

        int start = builder.Length;

        foreach (var key in Query.AllKeys)
        {
            var k = Uri.EscapeDataString(key ?? string.Empty);
            var values = Query.GetValues(key) ?? [string.Empty];

            foreach (var value in values)
            {
                if (builder.Length > start)
                    builder.Append('&');

                var v = Uri.EscapeDataString(value ?? string.Empty);

                builder
                    .Append(k)
                    .Append('=')
                    .Append(v);
            }
        }
    }


    private void SetFieldsFromUri(Uri uri)
    {
        if (!uri.IsAbsoluteUri)
        {
            // fall back to regex parser
            ParseUrl(uri.ToString());
            return;
        }

        Scheme = uri.Scheme;
        Host = uri.Host;
        Port = uri.Port;
        Fragment = uri.Fragment;

        ParseQueryString(uri.Query);
        ParsePath(uri.AbsolutePath);

        var userInfo = uri.UserInfo;

        if (string.IsNullOrEmpty(userInfo))
            return;

        int index = userInfo.IndexOf(':');

        if (index != -1)
        {
            Password = userInfo.Substring(index + 1);
            UserName = userInfo.Substring(0, index);
        }
        else
        {
            UserName = userInfo;
        }
    }

    private void ParseUrl(string? url)
    {
        if (string.IsNullOrEmpty(url))
            return;

        var match = UrlParseRegex().Match(url);
        if (!match.Success)
            return;

        SetScheme(match.Groups["scheme"].Value);
        SetUserName(match.Groups["username"].Value, true);
        SetPassword(match.Groups["password"].Value, true);
        SetHost(match.Groups["host"].Value);
        SetPort(match.Groups["port"].Value);
        SetPath(match.Groups["path"].Value);
        SetQuery(match.Groups["query"].Value);
        SetFragment(match.Groups["fragment"].Value);
    }

    private void ParseQueryString(string queryString)
    {
        if (string.IsNullOrEmpty(queryString))
            return;

        var result = HttpUtility.ParseQueryString(queryString);
        Query.Add(result);
    }

    private void ParsePath(string? path)
    {
        if (string.IsNullOrEmpty(path))
            return;

        var parts = path?.Split('/');
        if (parts == null)
            return;

        foreach (var part in parts)
        {
            if (string.IsNullOrEmpty(part))
                continue;

            var p = Uri.UnescapeDataString(part);

            Path.Add(p);
        }
    }


    private bool IsStandardPort()
    {
        if (string.IsNullOrEmpty(Scheme))
            return false;

        if (_schemePorts.TryGetValue(Scheme!, out int? port))
            return port == Port;

        return false;
    }
}
