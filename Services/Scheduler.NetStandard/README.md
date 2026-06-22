# EllipticBit Services - Task Scheduler

`EllipticBit.Services.Scheduler.NetStandard` provides instance and network-aware task
scheduling services for .NET. It supports recurring and one-shot background action
execution via dependency injection, with pluggable synchronization contexts for
coordinating work across a single instance or a network of instances.

This package is part of the [EllipticBit Services](https://github.com/ellipticbit/services)
collection of cross-platform service libraries for .NET.

## Features

- Instance-level and network-level scheduling services.
- Register reusable scheduled actions implementing `ISchedulerAction`.
- Enable, disable, and manually execute actions at runtime.
- Pluggable `ISchedulerSynchronizationContext` implementations.

## Installation

Install the package from [NuGet](https://www.nuget.org/packages/EllipticBit.Services.Scheduler.NetStandard):

```shell
dotnet add package EllipticBit.Services.Scheduler.NetStandard
```

Or add a `PackageReference` to your project file:

```xml
<PackageReference Include="EllipticBit.Services.Scheduler.NetStandard" Version="x.y.z" />
```

## Usage

Register the scheduler and its actions with the dependency injection container:

```csharp
using EllipticBit.Services.Scheduler;
using Microsoft.Extensions.DependencyInjection;

services.AddInstanceSchedulerServices()
	.AddAction<CleanupAction>();
```

Implement a scheduled action:

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

Start the scheduler and enable actions at runtime:

```csharp
public class Worker
{
	private readonly ISchedulerService _scheduler;

	public Worker(ISchedulerService scheduler)
	{
		_scheduler = scheduler;
	}

	public void Start()
	{
		_scheduler.Enable<CleanupAction>();
		_scheduler.Start();
	}
}
```

## Related Packages

- `EllipticBit.Services.Scheduler.Abstractions` - the scheduler abstractions implemented by this package.
- `EllipticBit.Services.Scheduler.SqlServer` - SQL Server based network synchronization.
- `EllipticBit.Services.Scheduler.WPF` - WPF UI-thread integration for the scheduler.

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
