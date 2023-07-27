using System;

namespace FluentRest;

/// <summary>
/// Contains the standard set of headers applicable to an HTTP request.
/// </summary>
public static class HttpRequestHeaders
{
    ///<summary>Content-Types that are acceptable</summary>
    public const string Accept = "Accept";
    ///<summary>Character sets that are acceptable</summary>
    public const string AcceptCharset = "Accept-Charset";
    ///<summary>Acceptable encodings. See HTTP compression.</summary>
    public const string AcceptEncoding = "Accept-Encoding";
    ///<summary>Acceptable languages for response</summary>
    public const string AcceptLanguage = "Accept-Language";
    ///<summary>Acceptable version in time</summary>
    public const string AcceptDatetime = "Accept-Datetime";
    ///<summary>Authentication credentials for HTTP authentication</summary>
    public const string Authorization = "Authorization";
    ///<summary>Used to specify directives that MUST be obeyed by all caching mechanisms along the request/response chain</summary>
    public const string CacheControl = "Cache-Control";
    ///<summary>What type of connection the user-agent would prefer</summary>
    public const string Connection = "Connection";
    ///<summary>an HTTP cookie previously sent by the server with Set-Cookie (below)</summary>
    public const string Cookie = "Cookie";
    ///<summary>The length of the request body in octets (8-bit bytes)</summary>
    public const string ContentLength = "Content-Length";
    ///<summary>A Base64-encoded binary MD5 sum of the content of the request body</summary>
    public const string ContentM5 = "Content-MD5";
    ///<summary>The MIME type of the body of the request (used with POST and PUT requests)</summary>
    public const string ContentType = "Content-Type";
    ///<summary>The content encoding</summary>
    public const string ContentEncoding = "Content-Encoding";
    ///<summary>The date and time that the message was sent</summary>
    public const string Date = "Date";
    ///<summary>Indicates that particular server behaviors are required by the client</summary>
    public const string Expect = "Expect";
    ///<summary>The email address of the user making the request</summary>
    public const string From = "From";
    ///<summary>The domain name of the server (for virtual hosting), mandatory since HTTP/1.1. Although domain name are specified as case-insensitive[5][6], it is not specified whether the contents of the Host field should be interpreted in a case-insensitive manner[7] and in practice some implementations of virtual hosting interpret the contents of the Host field in a case-sensitive manner.[citation needed]</summary>
    public const string Host = "Host";
    ///<summary>Only perform the action if the client supplied entity matches the same entity on the server. This is mainly for methods like PUT to only update a resource if it has not been modified since the user last updated it.</summary>
    public const string IfMatch = "If-Match";
    ///<summary>Allows a 304 Not Modified to be returned if content is unchanged</summary>
    public const string IfModifiedSince = "If-Modified-Since";
    ///<summary>Allows a 304 Not Modified to be returned if content is unchanged, see HTTP ETag</summary>
    public const string IfNoneMatch = "If-None-Match";
    ///<summary>If the entity is unchanged, send me the part(s) that I am missing; otherwise, send me the entire new entity</summary>
    public const string IfRange = "If-Range";
    ///<summary>Only send the response if the entity has not been modified since a specific time.</summary>
    public const string IfUnmodifiedSince = "If-Unmodified-Since";
    ///<summary>Limit the number of times the message can be forwarded through proxies or gateways.</summary>
    public const string MaxForwards = "Max-Forwards";
    ///<summary>Implementation-specific headers that may have various effects anywhere along the request-response chain.</summary>
    public const string Pragma = "Pragma";
    ///<summary>Authorization credentials for connecting to a proxy.</summary>
    public const string ProxyAuthorization = "Proxy-Authorization";
    ///<summary>Request only part of an entity. Bytes are numbered from 0.</summary>
    public const string Range = "Range";
    ///<summary>This is the address of the previous web page from which a link to the currently requested page was followed. (The word �referrer� is misspelled in the RFC as well as in most implementations.)</summary>
    public const string Referer = "Referer";
    ///<summary>The transfer encodings the user agent is willing to accept: the same values as for the response header Transfer-Encoding can be used, plus the trailers value (related to the chunked transfer method) to notify the server it expects to receive additional headers (the trailers) after the last, zero-sized, chunk.</summary>
    public const string TE = "TE";
    ///<summary>Ask the server to upgrade to another protocol.</summary>
    public const string Upgrade = "Upgrade";
    ///<summary>The user agent string of the user agent</summary>
    public const string UserAgent = "User-Agent";
    ///<summary>Informs the server of proxies through which the request was sent.</summary>
    public const string Via = "Via";
    ///<summary>A general warning about possible problems with the entity body.</summary>
    public const string Warning = "Warning";
    ///<summary>Used to specify an HTTP method override value.</summary>
    public const string MethodOverride = "X-HTTP-Method-Override";
}
