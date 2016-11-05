# ParallelDaemon
Parallelized daemon for CWOJ.

## Prerequisites
* A RabbitMQ server. (Not required at the moment)
* Protocol Buffer compiler. Installing: `apt install protobuf-compiler`.
* Dotnet Core. [Installation instructions](https://www.microsoft.com/net/core)

## Compiling
Please `cd` to `src/ParallelDaemon`.
The project can be compiled via `dotnet build`. Please run `dotnet restore` to download the packages required.
