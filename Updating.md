## How to update the package

Every new version is released completely: changelog, README, nuget.org, GitHub Packages and a GitHub
release. None of these steps is optional.

1. Add your changes. Keep the build free of warnings and run the tests:
   `dotnet test src/PagerDutyClient.sln -c Release`.
2. Add an entry at the top of `Changelog.md` in the existing format:
   `* **Version 1.0.13.0 (2026-08-15)** : Short description.`
3. Copy that text into `PackageReleaseNotes` in `src/PagerDutyClient/PagerDutyClient.csproj`:
   `Version 1.0.13.0 (2026-08-15): Short description.`
4. Update `README.md`. The `## Available for` list has to match the target frameworks, the usage
   sample has to compile against the current API.
5. Commit the changes.
6. Tag the commit with the plain version number, without a `v` prefix (`1.0.13`, `1.0.12`, ...). The
   existing tags are lightweight tags, create the new one the same way.
7. Push the commits and the tag.
8. Run `BuildAndPushPackage.bat` from the repository root while the tag is still the newest commit.
   It deletes every `bin` and `obj` below `src`, builds the release and pushes the `.nupkg` and the
   `.snupkg` to nuget.org and to the NuGet source named `github`. It needs `NUGET_API_KEY` and
   `GITHUB_API_KEY` in the environment. A push to nuget.org cannot be taken back.
9. Check that the `.nupkg` arrived on GitHub Packages,
   `https://nuget.pkg.github.com/SeppPenner/index.json`. The script only reaches it if a NuGet
   source named `github` is registered (`dotnet nuget list source`). Otherwise push the `.nupkg`
   from `src/PagerDutyClient/bin/Release` by hand, with a GitHub token that has the scope
   `write:packages` as API key. GitHub Packages takes no symbol packages.
10. Create the GitHub release for the tag, with the changelog sentence and the link to the version
    on nuget.org as notes:
    `gh release create 1.0.13 -R SeppPenner/PagerDutyClient --verify-tag --latest --title "Version 1.0.13.0 (2026-08-15)" --notes-file <file>`.

The tag comes before the build on purpose. The version of the package comes from GitVersion out of
the tag plus the distance to it, so building before the tag exists, or after another commit on top
of it, puts a prerelease version into the package.
