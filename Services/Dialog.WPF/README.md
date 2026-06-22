# EllipticBit Services - WPF Dialogs

`EllipticBit.Services.Dialog.WPF` provides a dependency-injection friendly dialog
service for WPF desktop applications. It includes a reusable message dialog and a
ready-to-use login dialog, allowing view models to show dialogs without taking a
direct dependency on WPF windowing types.

This package is part of the [EllipticBit Services](https://github.com/ellipticbit/services)
collection of cross-platform service libraries for .NET.

## Features

- `DialogService` for showing message and login dialogs from view models.
- Configurable message dialogs through `MessageDialogSettings`.
- Built-in login dialog returning a strongly-typed `LoginDialogResult`.

## Installation

Install the package from [NuGet](https://www.nuget.org/packages/EllipticBit.Services.Dialog.WPF):

```shell
dotnet add package EllipticBit.Services.Dialog.WPF
```

Or add a `PackageReference` to your project file:

```xml
<PackageReference Include="EllipticBit.Services.Dialog.WPF" Version="x.y.z" />
```

## Usage

Register the dialog service with the dependency injection container:

```csharp
using EllipticBit.Services.Dialog.WPF;
using Microsoft.Extensions.DependencyInjection;

services.RegisterDialogService();
```

Then inject `DialogService` into your view models:

```csharp
using EllipticBit.Services.Dialog.WPF;

public class ShellViewModel
{
	private readonly DialogService _dialogs;

	public ShellViewModel(DialogService dialogs)
	{
		_dialogs = dialogs;
	}

	public void ConfirmDelete()
	{
		_dialogs.Show(new MessageDialogSettings
		{
			Title = "Delete item",
			Message = "Are you sure you want to delete this item?"
		});
	}
}
```

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
