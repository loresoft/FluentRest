@echo off
NuGet.exe install MSBuildTasks -OutputDirectory .\tools\ -ExcludeVersion -NonInteractive
NuGet.exe install coveralls.net -OutputDirectory .\tools\ -ExcludeVersion -NonInteractive
