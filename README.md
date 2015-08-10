# FluentRest

Lightweight fluent wrapper over HttpClient to make REST calls easier

[![Build status](https://ci.appveyor.com/api/projects/status/buggns6km0ktd3j2?svg=true)](https://ci.appveyor.com/project/LoreSoft/fluentrest)

[![NuGet Version](https://img.shields.io/nuget/v/FluentRest.svg?style=flat-square)](https://www.nuget.org/packages/FluentRest/)

[![NuGet Version](https://img.shields.io/nuget/dt/FluentRest.svg?style=flat-square)](https://www.nuget.org/packages/FluentRest/)

## Download

The FluentRest library is available on nuget.org via package name `FluentRest`.

To install FluentRest, run the following command in the Package Manager Console

    PM> Install-Package FluentRest
    
More information about NuGet package avaliable at
<https://nuget.org/packages/FluentRest>

## Development Builds

Development builds are available on the myget.org feed.  A development build is promoted to the main NuGet feed when it's determined to be stable. 

In your Package Manager settings add the following package source for development builds:
<http://www.myget.org/F/loresoft/>

## Features

* Fluent request building
* Fluent form building
* Automatic deserialization of response
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
    .Header(h => h.Authorization("token", "7ca62d97436f382253c6b9648d40b4b59630b778"))
);
```

# Fake Response

FluentRest has the ability to fake a HTTP responses by loading the response from disk.  You can first capture the response, then use it for unit tests.

Configure the FluentRest to capture response.

```csharp
var serializer = new JsonContentSerializer();

var fakeHttp = new FakeMessageHandler();
fakeHttp.Mode = FakeResponseMode.Capture;
fakeHttp.StorePath = @".\GitHub\Responses";

var client = new FluentClient(serializer, fakeHttp);
client.BaseUri = new Uri("https://api.github.com/", UriKind.Absolute);

var result = await client.GetAsync<Repository>(b => b
    .AppendPath("repos")
    .AppendPath("loresoft")
    .AppendPath("FluentRest")
    .Header(h => h.Authorization("token", "7ca62d97436f382253c6b9648d40b4b59630b778"))
);
```

Use captured response

```csharp
var serializer = new JsonContentSerializer();

var fakeHttp = new FakeMessageHandler();
fakeHttp.Mode = FakeResponseMode.Fake;
fakeHttp.StorePath = @".\GitHub\Responses";

var client = new FluentClient(serializer, fakeHttp);
client.BaseUri = new Uri("https://api.github.com/", UriKind.Absolute);

var result = await client.GetAsync<Repository>(b => b
    .AppendPath("repos")
    .AppendPath("loresoft")
    .AppendPath("FluentRest")
    .Header(h => h.Authorization("token", "7ca62d97436f382253c6b9648d40b4b59630b778"))
);
```