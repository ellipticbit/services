# EllipticBit Services - Scheduler Abstractions

`EllipticBit.Services.Scheduler.Abstractions` contains the service abstractions and
interfaces for task scheduling in the EllipticBit Services library. It defines the
contracts for the scheduler service, scheduler actions, and synchronization contexts
used by the concrete scheduler implementations.

This package is part of the [EllipticBit Services](https://github.com/ellipticbit/services)
collection of cross-platform service libraries for .NET.

## Features

- `ISchedulerService` contract for starting, stopping, enabling, and executing actions.
- `ISchedulerServiceBuilder` for registering actions during dependency injection.
- `ISchedulerAction` and `ISchedulerSynchronizationContext` contracts.
- Scheduling enumerations such as `SchedulerActionIntervalMode`.

## Installation

Install the package from [NuGet](https://www.nuget.org/packages/EllipticBit.Services.Scheduler.Abstractions):

```shell
dotnet add package EllipticBit.Services.Scheduler.Abstractions
```

Or add a `PackageReference` to your project file:

```xml
<PackageReference Include="EllipticBit.Services.Scheduler.Abstractions" Version="x.y.z" />
```

## Usage

Implement the `ISchedulerAction` contract to define a unit of scheduled work:

```csharp
using EllipticBit.Services.Scheduler;

public class CleanupAction : ISchedulerAction
{
	public Task Execute(CancellationToken cancellationToken)
	{
		// perform scheduled work
		return Task.CompletedTask;
	}
}
```

Register a concrete scheduler implementation and your actions in your composition root:

```csharp
// dotnet add package EllipticBit.Services.Scheduler.NetStandard
services.AddInstanceSchedulerServices()
	.AddAction<CleanupAction>();
```

## Implementation Packages

- `EllipticBit.Services.Scheduler.NetStandard` - core scheduler implementation.
- `EllipticBit.Services.Scheduler.SqlServer` - SQL Server network synchronization.
- `EllipticBit.Services.Scheduler.WPF` - WPF UI-thread integration.

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
