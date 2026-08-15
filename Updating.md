## How to update the package

1. Add your changes.
2. Add an entry at the top of the Changelog.md.
3. Update the release notes in the *.csproj file.
4. Update version using a Git tag.
5. Build the project with Visual Studio.
6. Upload nuget package.

The tag comes before the build on purpose. The version of the package comes from GitVersion, so
building before the tag exists puts a prerelease version into the package.