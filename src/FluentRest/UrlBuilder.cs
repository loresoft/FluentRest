using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentRest
{
    public class UrlBuilder
    {
        private static readonly Dictionary<string, int?> _schemePorts;
        //scheme:[//[user[:password]@]host[:port]][/path][?query][#fragment]
        private readonly IDictionary<string, ICollection<string>> _query;
        private readonly IList<string> _path;
        private readonly string _schemeDelimiter;

        private string _fragment;
        private string _host;
        private string _password;
        private int? _port;
        private string _scheme;
        private string _username;

        static UrlBuilder()
        {
            //A null in the scheme port map indicates a protocol that uses a "//" but does not use a (known) port.
            _schemePorts = new Dictionary<string, int?>(StringComparer.OrdinalIgnoreCase)
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
        }

        public UrlBuilder()
        {
            _query = new Dictionary<string, ICollection<string>>();
            _path = new List<string>();
            _fragment = string.Empty;
            _host = "localhost";
            _password = string.Empty;
            _scheme = "http";
            _schemeDelimiter = "://";
            _username = string.Empty;
        }

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

        public UrlBuilder(Uri uri) : this()
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            SetFieldsFromUri(uri);
        }


        public string Scheme => _scheme;

        public string UserName => _username;

        public string Password => _password;

        public string Host => _host;

        public int? Port => GetStandardPort();

        public IList<string> Path => _path;

        public IDictionary<string, ICollection<string>> Query => _query;

        public string Fragment => _fragment;


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

        public UrlBuilder SetUserName(string value)
        {
            if (value == null)
                value = string.Empty;

            _username = value;
            return this;
        }

        public UrlBuilder SetPassword(string value)
        {
            if (value == null)
                value = string.Empty;

            _password = value;
            return this;
        }

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

        public UrlBuilder SetPort(int value)
        {
            if (value < -1 || value > 0xFFFF)
                throw new ArgumentOutOfRangeException(nameof(value));

            _port = value;
            return this;
        }

        public UrlBuilder SetPort(string value)
        {
            if (!string.IsNullOrEmpty(value) && int.TryParse(value, out int port))
                return SetPort(port);

            _port = null;
            return this;
        }

        public UrlBuilder SetFragment(string value)
        {
            if (value == null)
                value = string.Empty;

            if (value.Length > 0 && value[0] != '#')
                value = '#' + value;

            _fragment = value;
            return this;
        }


        public UrlBuilder AppendPath(Uri path)
        {
            if (path == null)
                return this;

            ParsePath(path.AbsolutePath);

            return this;
        }

        public UrlBuilder AppendPath(string path)
        {
            if (path == null)
                return this;

            ParsePath(path);

            return this;
        }

        public UrlBuilder AppendPath(IEnumerable<string> paths, bool encode)
        {
            if (paths == null)
                return this;

            foreach (var path in paths)
                ParsePath(path);

            return this;
        }

        public UrlBuilder AppendPath(params string[] paths)
        {
            return AppendPath(paths, false);
        }


        public UrlBuilder AppendPathFormat(string format, params object[] arguments)
        {
            var p = string.Format(format, arguments);

            return AppendPath(p);
        }


        public UrlBuilder SetPath(string path)
        {
            Path.Clear();
            if (path == null)
                return this;

            ParsePath(path);
            return this;
        }


        public UrlBuilder AppendQuery(string name, string value)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            var v = value ?? string.Empty;

            var list = Query.GetOrAdd(name, n => new List<string>());
            list.Add(v);

            return this;
        }

        public UrlBuilder AppendQuery<TValue>(string name, TValue value)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            var v = value != null ? value.ToString() : string.Empty;
            return AppendQuery(name, v);
        }

        public UrlBuilder AppendQueryIf(Func<bool> condition, string name, string value)
        {
            if (condition == null || !condition())
                return this;

            return AppendQuery(name, value);
        }

        public UrlBuilder AppendQueryIf<TValue>(Func<bool> condition, string name, TValue value)
        {
            if (condition == null || !condition())
                return this;

            return AppendQuery(name, value);
        }

        public UrlBuilder AppendQuery(string queryString)
        {
            if (queryString == null)
                return this;

            ParseQueryString(queryString);
            return this;
        }


        public UrlBuilder SetQuery(string queryString)
        {
            Query.Clear();
            if (queryString == null)
                return this;

            ParseQueryString(queryString);

            return this;
        }


        public Uri ToUri()
        {
            var url = ToString();
            var uri = new Uri(url, UriKind.Absolute);

            return uri;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(_scheme))
                builder.Append(_scheme).Append(_schemeDelimiter);

            if (!string.IsNullOrWhiteSpace(_username))
            {
                builder.Append(_username);
                if (!string.IsNullOrWhiteSpace(_password))
                    builder.Append(":").Append(_password);

                builder.Append("@");
            }

            if (!string.IsNullOrWhiteSpace(_host))
            {
                builder.Append(_host);
                if (_port.HasValue && !IsStandardPort())
                    builder.Append(":").Append(_port);
            }

            WritePath(builder);
            WriteQueryString(builder);

            if (!string.IsNullOrWhiteSpace(_fragment))
                builder.Append(_fragment);

            return builder.ToString();
        }


        private bool IsStandardPort()
        {
            if (_schemePorts.TryGetValue(_scheme, out int? port))
                return port == _port;

            return false;
        }


        private void WritePath(StringBuilder builder)
        {
            builder.Append("/");
            if (Path == null || Path.Count == 0)
                return;

            int start = builder.Length;
            foreach (var p in Path)
            {
                if (builder.Length > start)
                    builder.Append("/");

                var s = p.Replace(" ", "+");
                s = Uri.EscapeUriString(s);

                builder.Append(s);
            }
        }

        private void WriteQueryString(StringBuilder builder)
        {
            if (Query == null || Query.Count == 0)
                return;

            builder.Append("?");

            int start = builder.Length;
            foreach (var pair in Query)
            {
                var key = pair.Key;
                key = Uri.EscapeDataString(key);
                key = key.Replace("%20", "+");

                var values = pair.Value.ToList();

                foreach (var value in values)
                {
                    if (builder.Length > start)
                        builder.Append("&");

                    var v = value;
                    v = Uri.EscapeDataString(v);
                    v = v.Replace("%20", "+");

                    builder
                        .Append(key)
                        .Append("=")
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

            int l = s.Length;
            int i = 0;

            // remove leading ?
            if (s[0] == '?')
                i = 1;

            while (i < l)
            {
                // find next & while noting first = on the way (and if there are more)
                int si = i;
                int ti = -1;

                while (i < l)
                {
                    char ch = s[i];

                    if (ch == '=')
                    {
                        if (ti < 0)
                            ti = i;
                    }
                    else if (ch == '&')
                    {
                        break;
                    }

                    i++;
                }

                // extract the name / value pair
                string name = null;
                string value = null;

                if (ti >= 0)
                {
                    name = s.Substring(si, ti - si);
                    value = s.Substring(ti + 1, i - ti - 1);
                }
                else
                {
                    value = s.Substring(si, i - si);
                }

                // decode
                name = string.IsNullOrEmpty(name) ? string.Empty : Uri.UnescapeDataString(name);
                value = string.IsNullOrEmpty(value) ? string.Empty : Uri.UnescapeDataString(value);

                // add name / value pair to the collection
                if (!string.IsNullOrEmpty(name))
                    AppendQuery(name, value);

                // trailing '&'

                //if (i == l-1 && s[i] == '&')
                //    base.Add(null, String.Empty);

                i++;
            }
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
}



