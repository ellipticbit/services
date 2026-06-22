# EllipticBit Services - WPF Scheduler

`EllipticBit.Services.Scheduler.WPF` provides WPF integration for the EllipticBit task
scheduler. It dispatches scheduled actions onto the user interface thread, allowing
desktop applications to safely update the UI from scheduled background work.

This package is part of the [EllipticBit Services](https://github.com/ellipticbit/services)
collection of cross-platform service libraries for .NET.

## Features

- Dispatches scheduled actions onto the WPF UI thread.
- Integrates with the EllipticBit scheduler services.
- Ideal for desktop applications that update UI from background actions.

## Installation

Install the package from [NuGet](https://www.nuget.org/packages/EllipticBit.Services.Scheduler.WPF):

```shell
dotnet add package EllipticBit.Services.Scheduler.WPF
```

Or add a `PackageReference` to your project file:

```xml
<PackageReference Include="EllipticBit.Services.Scheduler.WPF" Version="x.y.z" />
```

## Usage

Register the scheduler using the WPF synchronization context so that actions are
dispatched onto the UI thread:

```csharp
using EllipticBit.Services.Scheduler;
using Microsoft.Extensions.DependencyInjection;

services.AddInstanceSchedulerServices<WpfSynchronizationContext>()
	.AddAction<RefreshDashboardAction>();
```

Scheduled actions registered this way can safely interact with WPF UI elements when
executed.

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
