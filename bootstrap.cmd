@echo off
Nuget.exe restore "Source\KickStart.sln"

NuGet.exe install MSBuildTasks -OutputDirectory .\Tools\ -ExcludeVersion -NonInteractive
NuGet.exe install xunit.runner.console -OutputDirectory .\Tools\ -ExcludeVersion -NonInteractive
