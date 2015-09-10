# FluentRest

Lightweight fluent wrapper over HttpClient to make REST calls easier

[![Build status](https://ci.appveyor.com/api/projects/status/buggns6km0ktd3j2?svg=true)](https://ci.appveyor.com/project/LoreSoft/fluentrest)

[![NuGet Version](https://img.shields.io/nuget/v/FluentRest.svg?style=flat-square)](https://www.nuget.org/packages/FluentRest/)

[![NuGet Version](https://img.shields.io/nuget/dt/FluentRest.svg?style=flat-square)](https://www.nuget.org/packages/FluentRest/)

## Download

The FluentRest library is available on nuget.org via package name `FluentRest`.

To install FluentRest, run the following command in the Package Manager Console

    PM> Install-Package FluentRest
    
More information about NuGet package available at
<https://nuget.org/packages/FluentRest>

## Development Builds

Development builds are available on the myget.org feed.  A development build is promoted to the main NuGet feed when it's determined to be stable. 

In your Package Manager settings add the following package source for development builds:
<http://www.myget.org/F/loresoft/>

## Features

* Fluent request building
* Fluent form data building
* Automatic deserialization of response content
* Plugin different serialization 
* Fake HTTP responses


## Fluent Request

Create a form post request

```csharp
var client = new FluentClient();
client.BaseUri = new Uri("http://echo.jpillora.com/", UriKind.Absolute);

var result = await client.PostAsync<EchoResult>(b => b
    .AppendPath("Project")
    .AppendPath("123")
    .FormValue("Test", "Value")
    .FormValue("key", "value")
    .QueryString("page", 10)
);
```

Custom authorization header

```csharp
var client = new FluentClient();
client.BaseUri = new Uri("https://api.github.com/", UriKind.Absolute);

var result = await client.GetAsync<Repository>(b => b
    .AppendPath("repos")
    .AppendPath("loresoft")
    .AppendPath("FluentRest")
    .Header(h => h.Authorization("token", "7ca..."))
);
```

## Fake Response

FluentRest has the ability to fake an HTTP responses by using a custom HttpClientHandler. Faking the HTTP response allows creating unit tests without having to make the actual HTTP call.  

### Fake Response Stores

Fake HTTP responses can be stored in the following message stores.  To create your own message store, implement `IFakeMessageStore`.

#### MemoryMessageStore

The memory message store allows composing a JSON response in the unit test.  Register the responses on the start of the unit test.


Register a fake response by URL.

```csharp
MemoryMessageStore.Current.Register(b => b
    .Url("https://api.github.com/repos/loresoft/FluentRest")
    .StatusCode(HttpStatusCode.OK)
    .ReasonPhrase("OK")
    .Content(c => c
        .Header("Content-Type", "application/json; charset=utf-8")
        .Data(responseObject) // object to be JSON serialized
    )
);
```

Use the fake response in a unit test

```csharp
var serializer = new JsonContentSerializer();

// use memory store by default
var fakeHttp = new FakeMessageHandler();

var client = new FluentClient(serializer, fakeHttp);
client.BaseUri = new Uri("https://api.github.com/", UriKind.Absolute);

// make HTTP call
var result = await client.GetAsync<Repository>(b => b
    .AppendPath("repos")
    .AppendPath("loresoft")
    .AppendPath("FluentRest")
    .Header(h => h.Authorization("token", "7ca..."))
);
```

#### FileMessageStore

The file message store allows saving an HTTP call response on the first use.  You can then use that saved response for all future unit test runs.


Configure the FluentRest to capture response.

```csharp
var serializer = new JsonContentSerializer();

// use file store to load from disk
var fakeStore = new FileMessageStore();
fakeStore.StorePath = @".\GitHub\Responses";

var fakeHttp = new FakeMessageHandler(fakeStore, FakeResponseMode.Capture);

var client = new FluentClient(serializer, fakeHttp);
client.BaseUri = new Uri("https://api.github.com/", UriKind.Absolute);

var result = await client.GetAsync<Repository>(b => b
    .AppendPath("repos")
    .AppendPath("loresoft")
    .AppendPath("FluentRest")
    .Header(h => h.Authorization("token", "7ca..."))
);
```

Use captured response

```csharp
var serializer = new JsonContentSerializer();

// use file store to load from disk
var fakeStore = new FileMessageStore();
fakeStore.StorePath = @".\GitHub\Responses";

var fakeHttp = new FakeMessageHandler(fakeStore, FakeResponseMode.Fake);

var client = new FluentClient(serializer, fakeHttp);
client.BaseUri = new Uri("https://api.github.com/", UriKind.Absolute);

var result = await client.GetAsync<Repository>(b => b
    .AppendPath("repos")
    .AppendPath("loresoft")
    .AppendPath("FluentRest")
    .Header(h => h.Authorization("token", "7ca..."))
);
```

