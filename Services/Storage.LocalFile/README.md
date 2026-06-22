# EllipticBit Services - Local File Storage

`EllipticBit.Services.Storage.LocalFile` is a local file system provider for the
EllipticBit storage abstractions. It implements the unified storage API backed by the
local disk, making it ideal for development, testing, and single-server deployments.

This package is part of the [EllipticBit Services](https://github.com/ellipticbit/services)
collection of cross-platform service libraries for .NET.

## Features

- Unified `ILocalStorageService` implementation backed by the local file system.
- Drop-in replacement for remote storage providers during development and testing.
- Configuration-driven setup.

## Installation

Install the package from [NuGet](https://www.nuget.org/packages/EllipticBit.Services.Storage.LocalFile):

```shell
dotnet add package EllipticBit.Services.Storage.LocalFile
```

Or add a `PackageReference` to your project file:

```xml
<PackageReference Include="EllipticBit.Services.Storage.LocalFile" Version="x.y.z" />
```

## Usage

Register the local file storage service with the dependency injection container:

```csharp
using EllipticBit.Services.Storage;
using Microsoft.Extensions.DependencyInjection;

services.AddLocalFileStorage(configuration);
```

Then inject `ILocalStorageService` to read and write files:

```csharp
using EllipticBit.Services.Storage;

public class ExportService
{
	private readonly ILocalStorageService _storage;

	public ExportService(ILocalStorageService storage)
	{
		_storage = storage;
	}

	public Task SaveAsync(string path, Stream content)
		=> _storage.Write(path, content);
}
```

## Related Packages

- `EllipticBit.Services.Storage.Abstractions` - the storage abstractions implemented by this package.
- `EllipticBit.Services.Storage.AzureBlob` - an Azure Blob Storage provider for the same abstractions.

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
