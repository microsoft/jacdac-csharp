# Jacdac .NET

**Jacdac** is a plug-and-play hardware/software stack 
for **microcontrollers** and their peripherals (sensors/actuators), 
with applications to rapid prototyping, making, and physical computing. 

This repository contains a **.NET 5** or **TinyCLR** client library for the [Jacdac](https://aka.ms/jacdac) protocol.

* **[Jacdac Documentation](https://aka.ms/jacdac/)**
* Discussions at https://github.com/microsoft/jacdac/discussions
* Issues are tracked on https://github.com/microsoft/jacdac/issues

The rest of this page is for developers of the jacdac-dotnet library.

This layer is still under construction.

## Assemblies

This repository contains a C# implementation of the Jacdac protocol for various .NET runtime, including desktop or TinyClR.
To avoid mscorlib issues, each platform has a different set of assemblies where C# files are simply shared as links.

  - `Jacdac`, core Jacdac library, .NET5
  - `Jacdac.NET`, HF2 protocol layer and .NET famework specific platform, .NET5
  - `Jacdac.NET.Playground`, .NET5 test application using jacdac development server
  - `Jacdac.Transports.WebSockets`, WebSocket transport, .NET5
  - `Jacdac.TinyCLR`, mirror of `Jacdac` library, TinyCLR
  - `Jacdac.TinyCLR.Playground`, TinyCLR test application
  - `Jacdac.Tests`, unit tests, .NET6

  - `Jacdac.Transports.Hf2`, HF2 protocol layer, .NET6, under construction
  - `Jacdac.Transports.Usb`, Usb transport, .NET6, under construction
  - `Jacdac.Transports.Spi`, SPI transport layer for SPI Jacdapter using .NET IoT, .NET5, under construction

## Developer setup

* clone this repository and pull all submodules
```
git clone https://github.com/microsoft/jacdac-dotnet
git submodule update --init --recursive
git pull
```

* Restore Nuget packages. (Either in your preferred IDE/Editor or using `dotnet restore`).
* Execute the desired tool or build the core library using your IDE or `dotnet build`/`dotnet run`

## Testing with .NET and Jacdac development server

* install NodeJS 14+
* install Jacdac cli
```
npm install -g jacdac-cli
```

* launch Jacdac dev tools
```
jacdac devtools
```

* start running or debugging Jacdac.NET.Playground. The webdashboard will serve as a connector to the hardware.

## Contributing

This project welcomes contributions and suggestions.  Most contributions require you to agree to a
Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us
the rights to use your contribution. For details, visit https://cla.opensource.microsoft.com.

When you submit a pull request, a CLA bot will automatically determine whether you need to provide
a CLA and decorate the PR appropriately (e.g., status check, comment). Simply follow the instructions
provided by the bot. You will only need to do this once across all repos using our CLA.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or
contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

## Trademarks

This project may contain trademarks or logos for projects, products, or services. Authorized use of Microsoft 
trademarks or logos is subject to and must follow 
[Microsoft's Trademark & Brand Guidelines](https://www.microsoft.com/en-us/legal/intellectualproperty/trademarks/usage/general).
Use of Microsoft trademarks or logos in modified versions of this project must not cause confusion or imply Microsoft sponsorship.
Any use of third-party trademarks or logos are subject to those third-party's policies.
