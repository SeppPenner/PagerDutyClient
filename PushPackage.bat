dotnet nuget push "src\PagerDutyClient\bin\Release\HaemmerElectronics.SeppPenner.PagerDutyClient.*.nupkg" -s "github" --skip-duplicate
dotnet nuget push "src\PagerDutyClient\bin\Release\HaemmerElectronics.SeppPenner.PagerDutyClient.*.nupkg" -s "nuget.org" --skip-duplicate -k "%NUGET_API_KEY%"
PAUSE