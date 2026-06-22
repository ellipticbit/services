# EllipticBit Services - SQL Server Scheduler Synchronization

`EllipticBit.Services.Scheduler.SqlServer` is a SQL Server based network
synchronization provider for the EllipticBit task scheduler. It coordinates scheduled
actions across multiple application instances so that network-scoped actions run
exactly where and when they should.

This package is part of the [EllipticBit Services](https://github.com/ellipticbit/services)
collection of cross-platform service libraries for .NET.

## Features

- SQL Server backed `ISchedulerSynchronizationContext` for network scheduling.
- Coordinates scheduled actions across multiple instances.
- Integrates with the EllipticBit scheduler services.

## Installation

Install the package from [NuGet](https://www.nuget.org/packages/EllipticBit.Services.Scheduler.SqlServer):

```shell
dotnet add package EllipticBit.Services.Scheduler.SqlServer
```

Or add a `PackageReference` to your project file:

```xml
<PackageReference Include="EllipticBit.Services.Scheduler.SqlServer" Version="x.y.z" />
```

## Usage

Register the SQL Server synchronization provider alongside the scheduler services:

```csharp
using EllipticBit.Services.Scheduler;
using Microsoft.Extensions.DependencyInjection;

services.AddSchedulerSqlServerSynchronization(new SqlServerNetworkSynchronizationOptions
{
	ConnectionString = "Server=.;Database=Scheduler;Trusted_Connection=True;"
});

services.AddNetworkSchedulerServices<SqlServerSynchronizationContext>()
	.AddAction<ReportGenerationAction>();
```

The scheduler will now use SQL Server to coordinate network-scoped actions across all
participating instances.

## Related Packages

- `EllipticBit.Services.Scheduler.Abstractions` - the scheduler abstractions.
- `EllipticBit.Services.Scheduler.NetStandard` - the core scheduler implementation.

## Contributing

Contributions are welcome! To contribute:

1. Fork the repository and create a feature branch.
2. Make your changes, following the existing code style and conventions.
3. Add or update tests where appropriate.
4. Open a merge/pull request with a clear description of the change.

### AI / LLM-assisted contributions

If any part of your contribution was generated with the assistance of a Large
Language Model (LLM) or other AI tool, you **must** include the prompt(s) used to
generate it in the [`PROMPTS.txt`](../../PROMPTS.txt) file as part of the same
pull request.

## License

This project is licensed under the [Boost Software License 1.0 (BSL-1.0)](https://opensource.org/license/bsl-1-0).
