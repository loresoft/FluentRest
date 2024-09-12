using System.Collections.Specialized;
using System.Text;
using System.Web;

namespace FluentRest;

/// <summary>
/// Build and modify uniform resource locator (URL)
/// </summary>
public class UrlBuilder
{
    //scheme:[//[user[:password]@]host[:port]][/path][?query][#fragment]

    private static readonly Dictionary<string, int?> _schemePorts = new Dictionary<string, int?>(StringComparer.OrdinalIgnoreCase)
    {
        { "acap", 674 },
        { "afp", 548 },
        { "dict", 2628 },
        { "dns", 53 },
        { "file", null },
        { "ftp", 21 },
        { "git", 9418 },
        { "gopher", 70 },
        { "http", 80 },
        { "https", 443 },
        { "imap", 143 },
        { "ipp", 631 },
        { "ipps", 631 },
        { "irc", 194 },
        { "ircs", 6697 },
        { "ldap", 389 },
        { "ldaps", 636 },
        { "mms", 1755 },
        { "msrp", 2855 },
        { "msrps", null },
        { "mtqp", 1038 },
        { "nfs", 111 },
        { "nntp", 119 },
        { "nntps", 563 },
        { "pop", 110 },
        { "prospero", 1525 },
        { "redis", 6379 },
        { "rsync", 873 },
        { "rtsp", 554 },
        { "rtsps", 322 },
        { "rtspu", 5005 },
        { "sftp", 22 },
        { "smb", 445 },
        { "snmp", 161 },
        { "ssh", 22 },
        { "steam", null },
        { "svn", 3690 },
        { "telnet", 23 },
        { "ventrilo", 3784 },
        { "vnc", 5900 },
        { "wais", 210 },
        { "ws", 80 },
        { "wss", 443 },
        { "xmpp", null }
    };

    private readonly NameValueCollection _query;
    private readonly List<string> _path;
    private readonly string _schemeDelimiter;

    private string _fragment;
    private string _host;
    private string _password;
    private int? _port;
    private string _scheme;
    private string _username;

    /// <summary>
    /// Initializes a new instance of the <see cref="UrlBuilder"/> class.
    /// </summary>
    public UrlBuilder()
    {
        _query = new NameValueCollection();
        _path = new List<string>();
        _fragment = string.Empty;
        _host = "localhost";
        _password = string.Empty;
        _scheme = "http";
        _schemeDelimiter = "://";
        _username = string.Empty;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UrlBuilder"/> with the specified <paramref name="uri"/>.
    /// </summary>
    /// <param name="uri">A URI string.</param>
    /// <exception cref="ArgumentNullException">uri is null</exception>
    public UrlBuilder(string uri) : this()
    {
        if (uri == null)
            throw new ArgumentNullException(nameof(uri));

        // setting allowRelative=true for a string like www.acme.org
        Uri tryUri = new Uri(uri, UriKind.RelativeOrAbsolute);

        if (tryUri.IsAbsoluteUri)
        {
            SetFieldsFromUri(tryUri);
        }
        else
        {
            uri = "http://" + uri;
            SetFieldsFromUri(new Uri(uri));
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UrlBuilder"/> with the specified <paramref name="uri"/>.
    /// </summary>
    /// <param name="uri">An instance of the <see cref="Uri"/> class</param>
    /// <exception cref="ArgumentNullException">uri is null</exception>
    public UrlBuilder(Uri uri) : this()
    {
        if (uri == null)
            throw new ArgumentNullException(nameof(uri));

        SetFieldsFromUri(uri);
    }


    /// <summary>
    /// Gets the scheme name of the Url.
    /// </summary>
    /// <value>
    /// The scheme name of the Url.
    /// </value>
    public string Scheme => _scheme;

    /// <summary>
    /// Gets the user name associated with the user that accesses the Url.
    /// </summary>
    /// <value>
    /// The user name associated with the user that accesses the Url.
    /// </value>
    public string UserName => _username;

    /// <summary>
    /// Gets the password associated with the user that accesses the Url.
    /// </summary>
    /// <value>
    /// The password associated with the user that accesses the Url.
    /// </value>
    public string Password => _password;

    /// <summary>
    /// Gets the Domain Name System (DNS) host name or IP address of a server.
    /// </summary>
    /// <value>
    /// The Domain Name System (DNS) host name or IP address of a server.
    /// </value>
    public string Host => _host;

    /// <summary>
    /// Gets the port number of the Url.
    /// </summary>
    /// <value>
    /// The port number of the Url.
    /// </value>
    public int? Port => GetStandardPort();

    /// <summary>
    /// Gets the path segment collection to the resource referenced by the Url.
    /// </summary>
    /// <value>
    /// The path segment collection to the resource referenced by the Url.
    /// </value>
    public IList<string> Path => _path;

    /// <summary>
    /// Gets the query string dictionary information included in the Url.
    /// </summary>
    /// <value>
    /// The query string dictionary information included in the Url.
    /// </value>
    public NameValueCollection Query => _query;

    /// <summary>
    /// Gets the fragment portion of the Url.
    /// </summary>
    /// <value>
    /// The fragment portion of the Url.
    /// </value>
    public string Fragment => _fragment;


    /// <summary>
    /// Replace the schema name for the current Url.
    /// </summary>
    /// <param name="value">The schema name.</param>
    /// <returns></returns>
    public UrlBuilder SetScheme(string value)
    {
        if (value == null)
            value = string.Empty;

        int index = value.IndexOf(':');
        if (index != -1)
            value = value.Substring(0, index);

        if (value.Length != 0)
        {
            if (!Uri.CheckSchemeName(value))
                throw new ArgumentException("Invalid URI: The URI scheme is not valid.", nameof(value));

            value = value.ToLowerInvariant();
        }

        _scheme = value;
        return this;
    }

    /// <summary>
    /// Replace the user name for the current Url.
    /// </summary>
    /// <param name="value">The user name associated with the user that access the Url.</param>
    /// <returns></returns>
    public UrlBuilder SetUserName(string value)
    {
        if (value == null)
            value = string.Empty;

        _username = value;
        return this;
    }

    /// <summary>
    /// Replace the password for the current Url.
    /// </summary>
    /// <param name="value">The password associated with the user that access the Url.</param>
    /// <returns></returns>
    public UrlBuilder SetPassword(string value)
    {
        if (value == null)
            value = string.Empty;

        _password = value;
        return this;
    }

    /// <summary>
    /// Replace the Domain Name System (DNS) host name or IP address for the current Url.
    /// </summary>
    /// <param name="value">The Domain Name System (DNS) host name or IP address.</param>
    /// <returns></returns>
    public UrlBuilder SetHost(string value)
    {
        if (value == null)
            value = string.Empty;

        _host = value;
        //probable ipv6 address - Note: this is only supported for cases where the authority is inet-based.
        if (_host.IndexOf(':') >= 0)
        {
            //set brackets
            if (_host[0] != '[')
                _host = "[" + _host + "]";
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

        _port = value;
        return this;
    }

    /// <summary>
    /// Replace the port number for the current Url.
    /// </summary>
    /// <param name="value">The port number.</param>
    /// <returns></returns>
    public UrlBuilder SetPort(string value)
    {
        if (!string.IsNullOrEmpty(value) && int.TryParse(value, out int port))
            return SetPort(port);

        _port = null;
        return this;
    }

    /// <summary>
    /// Replace the fragment portion for the current Url.
    /// </summary>
    /// <param name="value">The fragment portion.</param>
    /// <returns></returns>
    public UrlBuilder SetFragment(string value)
    {
        if (value == null)
            value = string.Empty;

        if (value.Length > 0 && value[0] != '#')
            value = '#' + value;

        _fragment = value;
        return this;
    }


    /// <summary>
    /// Appends a path segment to the current Url.
    /// </summary>
    /// <param name="path">The path segment to append.</param>
    /// <returns></returns>
    public UrlBuilder AppendPath(Uri path)
    {
        if (path == null)
            return this;

        ParsePath(path.AbsolutePath);

        return this;
    }

    /// <summary>
    /// Appends a path segment to the current Url.
    /// </summary>
    /// <param name="path">The path segment to append.</param>
    /// <returns></returns>
    public UrlBuilder AppendPath(string path)
    {
        if (path == null)
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
    public UrlBuilder AppendPath<TValue>(TValue path)
    {
        if (path == null)
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
    public UrlBuilder AppendPaths(IEnumerable<string> paths)
    {
        if (paths == null)
            return this;

        foreach (var path in paths)
            ParsePath(path);

        return this;
    }

    /// <summary>
    /// Appends the path segments to the current Url.
    /// </summary>
    /// <param name="paths">The path segments to append.</param>
    /// <returns></returns>
    public UrlBuilder AppendPaths(params string[] paths)
    {
        if (paths == null)
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
    /// Replace the entire path for the current Url.  The <see cref="Path"/> collection is replaced with this path.
    /// </summary>
    /// <param name="path">The path segment to set.</param>
    /// <returns></returns>
    public UrlBuilder SetPath(string path)
    {
        Path.Clear();
        if (path == null)
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
    public UrlBuilder AppendQuery(string name, string value)
    {
        if (name == null)
            throw new ArgumentNullException(nameof(name));

        Query.Add(name, value ?? string.Empty);

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
    public UrlBuilder AppendQuery<TValue>(string name, TValue value)
    {
        if (name == null)
            throw new ArgumentNullException(nameof(name));

        var v = value != null ? value.ToString() : string.Empty;
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
    public UrlBuilder AppendQueries<TValue>(string name, IEnumerable<TValue> values)
    {
        if (name == null)
            throw new ArgumentNullException(nameof(name));

        if (values == null)
            return this;

        foreach (var value in values)
        {
            var v = value != null ? value.ToString() : string.Empty;
            AppendQuery(name, v);
        }

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
    public UrlBuilder AppendQueryIf(Func<bool> condition, string name, string value)
    {
        if (condition == null || !condition())
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
    public UrlBuilder AppendQueryIf<TValue>(Func<bool> condition, string name, TValue value)
    {
        if (condition == null || !condition())
            return this;

        return AppendQuery(name, value);
    }

    /// <summary>
    /// Appends the query string to the current url.
    /// </summary>
    /// <param name="queryString">The query string to append.</param>
    /// <returns></returns>
    public UrlBuilder AppendQuery(string queryString)
    {
        if (queryString == null)
            return this;

        ParseQueryString(queryString);
        return this;
    }


    /// <summary>
    /// Replace the entire query string for the current Url.  The <see cref="Query"/> dictionary is replaced with this query string.
    /// </summary>
    /// <param name="queryString">The query string to set.</param>
    /// <returns></returns>
    public UrlBuilder SetQuery(string queryString)
    {
        Query.Clear();
        if (queryString == null)
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
        var uri = new Uri(url, UriKind.Absolute);

        return uri;
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

        if (!string.IsNullOrWhiteSpace(_scheme))
            builder.Append(_scheme).Append(_schemeDelimiter);

        if (!string.IsNullOrWhiteSpace(_username))
        {
            builder.Append(_username);
            if (!string.IsNullOrWhiteSpace(_password))
                builder.Append(':').Append(_password);

            builder.Append('@');
        }

        if (!string.IsNullOrWhiteSpace(_host))
        {
            builder.Append(_host);
            if (_port.HasValue && !IsStandardPort())
                builder.Append(':').Append(_port);
        }

        WritePath(builder);
        WriteQueryString(builder);

        if (!string.IsNullOrWhiteSpace(_fragment))
            builder.Append(_fragment);

        return StringBuilderCache.ToString(builder);
    }


    private bool IsStandardPort()
    {
        if (_schemePorts.TryGetValue(_scheme, out int? port))
            return port == _port;

        return false;
    }


    private void WritePath(StringBuilder builder)
    {
        builder.Append('/');
        if (Path == null || Path.Count == 0)
            return;

        int start = builder.Length;
        foreach (var p in Path)
        {
            if (builder.Length > start)
                builder.Append('/');

            var v = Uri.EscapeDataString(p);

            builder.Append(v);
        }
    }

    private void WriteQueryString(StringBuilder builder)
    {
        if (Query == null || Query.Count == 0)
            return;

        builder.Append('?');

        int start = builder.Length;

        foreach (var key in Query.AllKeys)
        {
            var k = Uri.EscapeDataString(key ?? string.Empty);

            foreach (var value in Query.GetValues(key))
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
        _scheme = uri.Scheme;
        _host = uri.Host;
        _port = uri.Port;
        _fragment = uri.Fragment;

        ParseQueryString(uri.Query);
        ParsePath(uri.AbsolutePath);

        string userInfo = uri.UserInfo;

        if (userInfo.Length <= 0)
            return;

        int index = userInfo.IndexOf(':');

        if (index != -1)
        {
            _password = userInfo.Substring(index + 1);
            _username = userInfo.Substring(0, index);
        }
        else
        {
            _username = userInfo;
        }
    }

    // based on HttpUtility.ParseQueryString
    private void ParseQueryString(string s)
    {
        if (string.IsNullOrEmpty(s))
            return;

        var result = HttpUtility.ParseQueryString(s);
        Query.Add(result);
    }

    private void ParsePath(string s)
    {
        if (string.IsNullOrEmpty(s))
            return;

        var parts = s.Split('/');
        foreach (var part in parts)
        {
            if (string.IsNullOrEmpty(part))
                continue;

            var p = Uri.UnescapeDataString(part);

            Path.Add(p);
        }
    }


    private int? GetStandardPort()
    {
        if (_port.HasValue)
            return _port;

        if (string.IsNullOrEmpty(_scheme))
            return _port;

        _schemePorts.TryGetValue(_scheme, out int? port);
        return port;
    }

}
