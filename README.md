PagerDutyClient
====================================

PagerDutyClient is a library to send alarms to PagerDuty. Use the API at https://developer.pagerduty.com/docs/events-api-v2/trigger-events/.
The assembly was written and tested in Net 5.0.

[![Build status](https://ci.appveyor.com/api/projects/status/4tkko56irgvnh0w9?svg=true)](https://ci.appveyor.com/project/SeppPenner/pagerdutyclient)
[![GitHub issues](https://img.shields.io/github/issues/SeppPenner/PagerDutyClient.svg)](https://github.com/SeppPenner/PagerDutyClient/issues)
[![GitHub forks](https://img.shields.io/github/forks/SeppPenner/PagerDutyClient.svg)](https://github.com/SeppPenner/PagerDutyClient/network)
[![GitHub stars](https://img.shields.io/github/stars/SeppPenner/PagerDutyClient.svg)](https://github.com/SeppPenner/PagerDutyClient/stargazers)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://raw.githubusercontent.com/SeppPenner/PagerDutyClient/master/License.txt)
[![Nuget](https://img.shields.io/badge/PagerDutyClient-Nuget-brightgreen.svg)](https://www.nuget.org/packages/HaemmerElectronics.SeppPenner.PagerDutyClient/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/HaemmerElectronics.SeppPenner.PagerDutyClient.svg)](https://www.nuget.org/packages/HaemmerElectronics.SeppPenner.PagerDutyClient/)
[![Known Vulnerabilities](https://snyk.io/test/github/SeppPenner/PagerDutyClient/badge.svg)](https://snyk.io/test/github/SeppPenner/PagerDutyClient)
[![Gitter](https://badges.gitter.im/PagerDutyClient/community.svg)](https://gitter.im/PagerDutyClient/community?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge)

## Available for
* NetStandard 2.0
* NetStandard 2.1
* NetCore 2.1
* NetCore 3.1
* Net 5.0

## Net Core and Net latest and LTS versions
* https://dotnet.microsoft.com/download/dotnet-core
* https://dotnet.microsoft.com/download/dotnet/5.0

## Basic usage:

```csharp
var logger = Log.ForContext<SomeContext>();
var pagerDutyClient = new PagerDutyClient(logger);
await pagerDutyClient.Send(
    EventAction.Trigger,
    "78e9279b9fa84b7b8e9f1ec7b9b85fef",
    Severity.Critical,
    "MyService",
    "Some error.");
```

The project can be found on [nuget](https://www.nuget.org/packages/HaemmerElectronics.SeppPenner.PagerDutyClient/).

Change history
--------------

See the [Changelog](https://github.com/SeppPenner/PagerDutyClient/blob/master/Changelog.md).
