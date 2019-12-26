# FloodGate SDK for .NET

## Overview

This is the .NET SDK for [Floodgate](https://floodgate.io), a feature rollout service which provides a centralised management console for managing remote feature flags.

## Compatibility

The .NET SDK is currently compatible with .NET Standard 2.0+ (.NET Core 2.0+) and .NET Framework 4.5+

## Installing

Install the Floodgate SDK via NuGet

```console
Install-Package FloodGateSDK
```

## Usage

Below is a simple example of how you can use the .NET Framework SDK to check on the status of a flag.

Add FloodGate.SDK to your application

```csharp
using FloodGate.SDK;
```

Create a FloodGate Client instance

```csharp
var client = new FloodGateClient("ENTER-YOUR-API-KEY");
```

Retrieve your flag value

```csharp
var myFeatureFlag = client.GetValue("my-feature-flag", false);

if (myFeatureFlag)
{
  // Do something new and amazing here
}
else
{
  // Do whatever it is I'm usually doing here
}
```

Finishing up

```csharp
client.Dispose();
```

## Submitting issues

Sometimes everyone has issues! The Floodgate teams tracks all issues submitted to this [issue tracker](https://github.com/floodgate-io/dotnet-framework-sdk/issues). You are encouraged to use the issue tracker to report any bugs or submit your general feedback and feature enhancements. We will do our best to respond as quickly as possible.

## Contributing

We are always looking for talented engineers to join the Floodgate team. If you would like to contribue to our projects feel free to fork this project and when ready issue a PR back for review.

## About Floodgate

Floodgate is a remote feature management system designed to help engineering teams and product teams work independently. Using feature flags managed by Floodgate you will dramatically reduce the risks software companies face when releasing and deploying new features.

With Floodgate you can use fine grained user targeting to test out new features in your production environment with minimal impact and risk to your existing systems and customers. Floodgate provides a simple to use percentage rollout facility to allow you to perform canary releases with just a few clicks.

To learn more about Floodgate, visit us at https://floodgate.io or contact hello@floodgate.io. To get started with feature flags for free at https://app.floodgate.io/signup.

Floodgate has currently developed following SDKs.

* .Net [GitHub](https://github.com/floodgate-io/dotnet-sdk)
* JavaScript [GitHub](https://github.com/floodgate-io/javascript-sdk)
* Node [GitHub](https://github.com/floodgate-io/node-sdk)
* PHP [GitHub](https://github.com/floodgate-io/php-sdk)

## Contributing a New SDK

If you would like to contribute to Floodgate's library of SDKs and create an SDK for a new language, feel free to drop us an email at contribute@floodgate.io
