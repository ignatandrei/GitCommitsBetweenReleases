del *.nupkg 
dotnet tool uninstall -g dotnet-gcr
dotnet pack --output ./
dotnet tool install -g dotnet-gcr --add-source ./ --version 2.0.0
dotnet gcr --help
dotnet gcr ignatandrei AOP_With_Roslyn
pause