# clean
msbuild.exe -t:clean Jacdac.sln

# restore
nuget restore Jacdac.sln -PackagesDirectory ./packages

# Jacdac.TinyCLR
cd Jacdac.TinyCLR
msbuild.exe -t:build -p:Configuration=Release Jacdac.TinyCLR.csproj -v:d
nuget.exe pack Jacdac.TinyCLR.csproj -Prop Configuration=Release -OutputDirectory ../newpackages -Verbosity detailed -PackagesDirectory ./packages
cd ..

# Jacdac.TinyCLR.Clients
cd Jacdac.TinyCLR.Clients
msbuild.exe -t:build -p:Configuration=Release Jacdac.TinyCLR.Clients.csproj -v:d
nuget.exe pack Jacdac.TinyCLR.Clients.csproj -Prop Configuration=Release -OutputDirectory ../newpackages -Verbosity detailed -PackagesDirectory ./packages
cd ..

# Jacdac.TinyCLR.Storage
cd Jacdac.TinyCLR.Storage
msbuild.exe -t:build -p:Configuration=Release Jacdac.TinyCLR.Storage.csproj -v:d
nuget.exe pack Jacdac.TinyCLR.Storage.csproj -Prop Configuration=Release -OutputDirectory ../newpackages -Verbosity detailed -PackagesDirectory ./packages
cd ..

# .NET assemblies
nuget.exe restore Jacdac.sln
msbuild.exe -t:build -p:Configuration=Release Jacdac.sln
dotnet pack Jacdac.DevTools/Jacdac.DevTools.csproj -c Release -o newpackages
dotnet pack Jacdac.NET/Jacdac.NET.csproj -c Release -o newpackages
dotnet pack Jacdac.NET.Clients/Jacdac.NET.Clients.csproj -c Release -o newpackages
dotnet pack Jacdac.NET.Transports.Hf2/Jacdac.Transports.Hf2.csproj -c Release -o newpackages
dotnet pack Jacdac.NET.Transports.Spi/Jacdac.Transports.Spi.csproj -c Release -o newpackages
dotnet pack Jacdac.NET.Transports.WebSockets/Jacdac.Transports.WebSockets.csproj -c Release -o newpackages
dotnet pack Jacdac.NET.Servers.SoundPlayer/Jacdac.Servers.SoundPlayer.csproj -c Release -o newpackages

# let's see  who got built
ls /R ./newpackages/