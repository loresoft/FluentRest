@echo off
Nuget.exe restore "FluentRest.sln"
NuGet.exe install MSBuildTasks -OutputDirectory .\Tools\ -ExcludeVersion -NonInteractive
