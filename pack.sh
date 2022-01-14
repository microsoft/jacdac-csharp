msbuild.exe -t:clean Jacdac.sln
msbuild.exe -t:build -p:Configuration=Release Jacdac.sln
nuget.exe restore .\Jacdac.TinyCLR\Jacdac.TinyCLR.csproj -OutputDirectory .\Jacdac.TinyCLR\packages
dotnet pack Jacdac.sln -c Release -o newpackages
nuget.exe pack ./Jacdac.TinyCLR/Jacdac.TinyCLR.csproj -Prop Configuration=Release -OutputDirectory ./newpackages