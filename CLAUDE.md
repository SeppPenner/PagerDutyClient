# Project rules for Claude

## What this is

PagerDutyClient is a small NuGet library that sends alarms to PagerDuty and reads incidents back.
The package id is `HaemmerElectronics.SeppPenner.PagerDutyClient`, the assembly and the root
namespace are `PagerDutyClient`. `GeneratePackageOnBuild` is on, so every Release build writes a
`.nupkg` and a `.snupkg` into `src/PagerDutyClient/bin/Release`. `BuildAndPushPackage.bat` pushes
both to nuget.org and to GitHub Packages.

The library holds two independent clients for two different PagerDuty APIs. They share nothing: no
base class, no interface, no HTTP client.

- `PagerDutyClientV2` talks to the Events API v2 at `https://events.pagerduty.com`, one POST to
  `v2/enqueue` per call. It takes no API token, the routing key is a parameter of every call and
  travels in the payload. `TriggerAlert`, `AcknowledgeAlert` and `ResolveAlert` return `Task`
  without a result.
- `PagerDutyClient` talks to the REST API v2 at `https://api.pagerduty.com` and needs an API token
  in the constructor. `GetIncidents`, `GetIncidentsForService`, `GetIncidentsForServices`,
  `AcknowledgeAlert` and `ResolveAlert` return a DTO or `null`.

One solution `src/PagerDutyClient.sln` with exactly two projects:

- `src/PagerDutyClient/PagerDutyClient.csproj`, `OutputType` `Library`, the actual code.
- `src/PagerDutyClient.Tests/PagerDutyClient.Tests.csproj`, MSTest, added in version 1.0.13.0.

There is no demo project.

Layout inside `src/PagerDutyClient`:

- `PagerDutyClient.cs`: the REST API client. The private helpers `GetFromApi` and `PutToApi` do the
  HTTP work, the private static `GetIncidentRequestType` maps the enum to the wire value. Keep new
  logic in that shape.
- `PagerDutyClientV2.cs`: the Events API client, with `SendToEventsApi`, `GetSeverity` and
  `GetEventAction` as its private helpers.
- `Constants/HeaderKeys.cs`: the two request header names, `From` and `Token`.
- `Enumeration/`: `Severity` is public, `EventAction` and `IncidentRequestType` are internal.
- `Events/`: `DtoEvent` and `DtoEventPayload`, the request body of the Events API, both internal.
- `Incidents/`: 18 DTOs of the REST API. The result types are public, the request types
  `DtoIncidentRequest` and `DtoIncidentDataRequest` are internal.
- `GlobalUsings.cs`: all usings of the project.

Layout inside `src/PagerDutyClient.Tests`:

- `PagerDutyClientTests.cs`: the REST API client. The `Token token=` prefix of the authorization
  header, the query parameters, the repeated `service_ids[]`, the deserialization of an answer, the
  acknowledge and resolve payloads, an error status code and the argument validation.
- `PagerDutyClientV2Tests.cs`: the events API client. The endpoint, the payload of a trigger, the
  missing deduplication key of a trigger, the key of an acknowledge and a resolve, every severity
  and an error status code.
- `TestHttpMessageHandler.cs`: an `HttpMessageHandler` that records the request and answers from a
  canned status code and body. It is the reason the tests need no network.
- `TestDataProvider.cs`: the tokens, identifiers and the canned incidents answer both test classes
  use, plus a Serilog logger without a sink.
- `GlobalUsings.cs`: all usings of the test project, including the alias `RestClient`.

Repository root: `README.md` (the only user documentation, it is packed into the NuGet package),
`Changelog.md`, `Updating.md` (the release steps in short form, kept in sync with `## Releasing` below),
`License.txt` (MIT), `Icon.png`, `BuildAndPushPackage.bat`, `Delete-BIN-OBJ-Folders.bat`,
`.gitattributes` and `.gitignore`. There is no `HowToUse.md` and no screenshots.

## Build

```powershell
dotnet build src/PagerDutyClient.sln -c Release
```

```powershell
dotnet test src/PagerDutyClient.sln -c Release
```

- Target frameworks `net10.0;net11.0`, so every change is compiled twice and building needs the
  .NET 11 SDK. Nothing in the code is Windows specific. Net 9.0 was dropped in version 1.0.13.0
  because it is out of support, Net 8.0 after 1.0.13.0.
- Almost all build properties live directly in `src/PagerDutyClient/PagerDutyClient.csproj`.
  `src/Directory.Build.props` exists but sets nothing except `GenerateDocumentationFile`, which is
  why a missing XML doc comment is a build error here.
- `TreatWarningsAsErrors` is enabled, so every warning breaks the build, NuGet warnings (`NU****`)
  from restore included. A clean build reports zero warnings, keep it that way.
- `NU1803` (HTTP source usage during restore) is the one warning suppressed via `NoWarn`. Fix
  warnings instead of extending that list. `NuGetAudit` and `NuGetAuditMode=all` are on, so a
  vulnerable transitive package fails the build too.
- Versions come from GitVersion.MsBuild out of the git tags, for example `1.0.13-3` for the third
  commit after tag `1.0.12`. Never edit a version property or an assembly version by hand.
- Restore needs nuget.org. Private feeds (DevExpress, Telerik, GitHub Packages) were configured
  globally on this machine at times, on 2026-11-24 only nuget.org was registered. If a private feed
  answers 404 for a public package, restore fails with `NU1301`. Then build with an explicit source:
  `dotnet build src/PagerDutyClient.sln --source https://api.nuget.org/v3/index.json`.
- The library is built for two frameworks, the test project only for `net11.0`, the newest
  framework the library targets, keep it there. Tests are MSTest,
  with the same package set as the sibling repositories: `Microsoft.NET.Test.Sdk`,
  `MSTest.TestAdapter`, `MSTest.TestFramework`, `coverlet.collector` and `GitVersion.MsBuild`.
  `dotnet test` runs 26 tests. They need no network and no PagerDuty account, every request ends in
  `TestHttpMessageHandler` instead of a socket, and nothing is written outside of `bin` and `obj`.
  Never claim a test run happened without running it.

## Code conventions

Follow the surrounding code, it is consistent throughout every file:

- File header comment block with `<copyright file="..." company="Hämmer Electronics">` and a
  `<summary>`, then the file-scoped namespace.
- XML doc comments on every type and every member, private members included, no exceptions.
  `GenerateDocumentationFile` plus `TreatWarningsAsErrors` turn a missing one into a build error.
- `Nullable`, `ImplicitUsings` and `LangVersion latest` are enabled.
- New `using` directives go into `GlobalUsings.cs`, inside the existing `#pragma warning disable
  IDE0065` block, never at the top of a file. The editorconfig requires usings inside the namespace
  (`csharp_using_directive_placement=inside_namespace:warning`), which global usings cannot satisfy,
  that is what the pragma is for. Do not add other pragmas. The comment text in that block is German
  because Visual Studio generated it, leave it alone.
- Fields, properties, methods and events are always accessed with `this.` qualification
  (`dotnet_style_qualification_for_*` at severity `warning`).
- `src/.editorconfig` also enforces braces everywhere, no multiple blank lines, four spaces, CRLF,
  UTF-8, file scoped namespaces, `System` usings sorted first and `IDE0005` as warning. Analyzer
  warnings are fixed, not silenced.
- Every DTO carries `[JsonSchemaFlatten]`, `[Serializable]` and a `[JsonProperty("snake_case")]` on
  each member, and every reference member is initialised so that it is never null. New DTOs follow
  that pattern. The one exception is `DtoEvent.DeduplicationKey`, which stays nullable on purpose so
  that `NullValueHandling.Ignore` drops it from the payload of a trigger.

## Known quirks

Do not silently "clean up" these, they are existing behaviour:

- **Two clients, two APIs, no shared abstraction.** `PagerDutyClientV2` is not a newer version of
  `PagerDutyClient`, it is a different API. V2 needs no API token because the routing key
  authenticates the call, V1 needs one. Do not "unify" them into a base class, the two APIs
  authenticate differently and return different shapes.
- **`Token token=` is the header value, not a typo.** The REST API wants
  `Authorization: Token token=<apiToken>`, so `SetAuthorizationHeader` builds the value with the
  `token=` prefix inside it. Without the prefix every REST call answers 401, which is what versions
  up to 1.0.12.0 did.
- **Array query parameters are repeated, not comma separated.** `GetIncidentsForServices` builds
  `service_ids[]=A&service_ids[]=B`. A comma separated list is read by the API as one identifier.
- **Every error is swallowed.** `GetFromApi`, `PutToApi` and `SendToEventsApi` catch every
  exception, write it to the `ILogger` and return `default` or nothing. A caller cannot tell an
  empty result from a failed request, and a non success status code is logged but never thrown. That
  is deliberate for an alarming library that must not take the caller down with it. Whoever changes
  it changes the contract of the whole package.
- **One `HttpClient` per instance, never disposed.** Both classes create their `HttpClient` from the
  base address they are given and neither implements `IDisposable`. Long lived client instances are
  the intended usage. Both also have a second constructor that takes a ready made `HttpClient`. That
  overload exists so that the tests can put a handler in front of it, and it is what a caller with
  an `IHttpClientFactory` uses. Do not remove it.
- **The mapping helpers have a silent default branch.** `GetSeverity` falls back to `info`,
  `GetEventAction` to `trigger` and `GetIncidentRequestType` to `acknowledged`. A new enum member
  without a new switch arm therefore ships as a wrong wire value instead of an exception.
- **`EventAction` and `IncidentRequestType` are internal, `Severity` is public.** A caller picks the
  action by calling `TriggerAlert`, `AcknowledgeAlert` or `ResolveAlert`, not by passing an enum.
  Making them public would add a second way to do the same thing.
- **`NJsonSchema` is referenced for one attribute.** `[JsonSchemaFlatten]` on the DTOs comes from
  that package. Nothing in the library generates a JSON schema, and the attribute has no effect on
  the Newtonsoft serialization that actually runs. The same is true for `[Serializable]`. Both are
  decorative, and both are on every DTO, so new DTOs get them too.
- **The class `PagerDutyClient` cannot be used by its simple name.** It sits in the namespace
  `PagerDutyClient`, so a consumer writing `new PagerDutyClient(...)` gets
  `CS0118: PagerDutyClient is a namespace but is used like a type`. Only
  `new PagerDutyClient.PagerDutyClient(...)` or a using alias compiles. `PagerDutyClientV2` is not
  affected. Renaming the class would be a breaking change of the package, so the `README.md` sample
  shows the alias instead.
- **Two header comment styles.** The files under `Incidents/` open with `//---` without a space,
  every other file with `// ---`. Cosmetic, not worth a commit of its own.
- **AppVeyor badge without CI in the repository.** `README.md` links an AppVeyor build that is
  configured outside of this repository. There is no `.github` folder and no pipeline file here.
- **`README.md`, `Changelog.md`, `Icon.png` and `License.txt` are packed into the package.** They
  are pulled in from the repository root by four `None` items in the csproj, `README.md` also via
  `PackageReadmeFile`. Renaming or moving one of them breaks the package build.
- **`src/PagerDutyClient.sln.DotSettings`** is tracked and holds nothing but a ReSharper user
  dictionary (`H_00E4mmer`, `Softwareentwicklung`). Leave it alone.
- **`.gitattributes` sets `* text=auto`** and every rule of the Visual Studio template below it is
  commented out. `core.autocrlf` is `true` in this clone, so the working tree is CRLF while git
  stores LF. Any binary file added later needs its own rule.

## Releasing

**Every new version is released completely.** Changelog, README, nuget.org, GitHub Packages and a
GitHub release belong to every version, none of these steps is optional and none of them waits for
a separate request.

1. Make the change.
2. Add an entry at the top of `Changelog.md` in the existing format:
   `* **Version 1.0.13.0 (2026-08-15)** : Short description.`
3. Update `PackageReleaseNotes` in `src/PagerDutyClient/PagerDutyClient.csproj` to the same text,
   in the format `Version 1.0.13.0 (2026-08-15): Short description.` It is the only place in the
   build files that names a version, and it is duplicated from the changelog by hand.
4. Update `README.md` for every version. The `## Available for` list has to match the target
   frameworks, the usage sample has to compile against the current API, and nothing there may
   describe a state the package has left behind. The README is packed into the package.
5. Commit that.
6. Tag the commit with the plain version number, no `v` prefix (`1.0.12`, `1.0.11`, ...). The
   existing tags are lightweight tags, create new ones the same way.
7. Push the commits and the tag.
8. Only then build the package, and build it while the tag is the newest commit. GitVersion takes
   the version from the tag plus the distance to it, so a build before the tag or after another
   commit produces a prerelease version like `1.0.14-3+Branch.master.Sha...`, and that version
   would end up on nuget.org. `git describe --tags` has to answer with the plain tag.
9. Publish to nuget.org with `BuildAndPushPackage.bat`, started from the repository root, its first
   line is `cd .\src`. It deletes every `bin` and `obj` below `src`, builds `-c Release` and pushes
   the `.nupkg` and the `.snupkg` to nuget.org with `%NUGET_API_KEY%` and to the NuGet source named
   `github` with `%GITHUB_API_KEY%`. **Do not run it as it is now:** `ECHO` is on at the four
   `dotnet nuget push` lines and they carry no leading `@`, so cmd prints each of them after the
   variable expansion, with both API keys in plain text, into the console and every log or
   transcript of the run. Prefix those four lines with `@` first, as in the sibling repositories.
   The script ends with `PAUSE`, so it wants a console. Call it with its full path: the Claude Code
   shell sets `NoDefaultCurrentDirectoryInExePath=1`, so cmd does not find a bare
   `BuildAndPushPackage.bat` in the current directory. Because it always rebuilds, it is only
   correct as long as no commit sits on top of the tag. If one does, push the package that was
   built from the tagged state instead, by its file name and not with the wildcard, because
   `GeneratePackageOnBuild` leaves a `.nupkg` of every intermediate build in `bin`:
   `dotnet nuget push HaemmerElectronics.SeppPenner.PagerDutyClient.1.0.13.nupkg -s "nuget.org" --skip-duplicate -k %NUGET_API_KEY%`
   in `src/PagerDutyClient/bin/Release`. That command takes the matching `.snupkg` with it. A push
   to nuget.org cannot be taken back, the version is burned even after unlisting. nuget.org needs a
   few minutes to validate a new version before it is listed.
10. Publish the same `.nupkg` to GitHub Packages,
    `https://nuget.pkg.github.com/SeppPenner/index.json`. The script only reaches it if a NuGet
    source named `github` is registered on the machine, `dotnet nuget list source` shows that. On
    2026-11-24 only nuget.org was registered here, so the two `github` lines of the script fail
    and the push has to be done by hand in `src/PagerDutyClient/bin/Release`:
    `dotnet nuget push HaemmerElectronics.SeppPenner.PagerDutyClient.1.0.13.nupkg -s https://nuget.pkg.github.com/SeppPenner/index.json --skip-duplicate --api-key <token>`.
    The API key is a GitHub token with the scope `write:packages`, for example from
    `gh auth token`. If `gh auth status` lists no `write:packages`, the user has to run
    `gh auth refresh -h github.com -s write:packages,read:packages`, it needs a browser login.
    GitHub Packages takes no symbol packages, push the `.nupkg` only.
11. Create the GitHub release:
    `gh release create 1.0.13 -R SeppPenner/PagerDutyClient --verify-tag --latest --title "Version 1.0.13.0 (2026-08-15)" --notes-file <file>`.
    The notes are the changelog sentence plus the line
    `NuGet: https://www.nuget.org/packages/HaemmerElectronics.SeppPenner.PagerDutyClient/1.0.13`.
    Write credits as `` `@name` `` in backticks, a bare `@name` notifies that person again.

The version in the `Changelog.md` has four parts (`1.0.13.0`), the tag has three (`1.0.13`).
`Updating.md` holds the same steps in short form for the reader of the repository, keep the two in
sync.

nuget.org holds every tagged version from 1.0.0 to 1.0.13. On 2026-11-24 no tag had a GitHub
release yet, and GitHub Packages held 1.0.0 to 1.0.5 and 1.0.10 to 1.0.13, the history there is
incomplete.
## Git

- **Never amend a commit.** No `git commit --amend`, not for a typo in the message, not to add a
  forgotten file, not even when the commit is still local. Write a follow-up commit instead. The
  release versions come from tags on exact commits, an amended commit leaves its tag pointing at a
  commit that no longer exists in the branch.

## Writing style

- Commit messages are written **in English only**: short, precise subject line, explanatory body
  when needed.
- Code comments and comments in project files such as `.csproj` are **always English**, regardless
  of the language used in the conversation.
- **No em dashes or en dashes** (`—`, `–`), neither in prose, commit messages, code comments nor
  documentation. Use a regular hyphen, comma, colon, parentheses or a separate sentence.
- German texts (documentation, chat replies) always use real umlauts and ß, never ASCII
  transliterations such as `ae`, `oe`, `ue` or `ss`. Identifiers, file names and configuration keys
  stay unchanged where umlauts are technically undesirable.
