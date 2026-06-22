# EllipticBit Services - Storage Abstractions

`EllipticBit.Services.Storage.Abstractions` contains the service abstractions and
interfaces for file storage in the EllipticBit Services library. It defines the
provider-agnostic contracts for both local and remote storage, allowing application
code to read and write files independent of the underlying storage backend.

This package is part of the [EllipticBit Services](https://github.com/ellipticbit/services)
collection of cross-platform service libraries for .NET.

## Features

- `ILocalStorageService` contract for local file storage.
- `IRemoteStorageService` contract for remote/cloud file storage.
- Provider-agnostic file storage operations.

## Installation

Install the package from [NuGet](https://www.nuget.org/packages/EllipticBit.Services.Storage.Abstractions):

```shell
dotnet add package EllipticBit.Services.Storage.Abstractions
```

Or add a `PackageReference` to your project file:

```xml
<PackageReference Include="EllipticBit.Services.Storage.Abstractions" Version="x.y.z" />
```

## Usage

Depend on the storage abstraction and let dependency injection supply a concrete
provider at runtime:

```csharp
using EllipticBit.Services.Storage;

public class DocumentService
{
	private readonly IRemoteStorageService _storage;

	public DocumentService(IRemoteStorageService storage)
	{
		_storage = storage;
	}

	public Task SaveAsync(string path, Stream content)
		=> _storage.Write(path, content);
}
```

Register a concrete provider in your composition root:

```csharp
// dotnet add package EllipticBit.Services.Storage.AzureBlob
await services.AddAzureBlobStorage(configuration, secretService);
```

## Provider Packages

- `EllipticBit.Services.Storage.AzureBlob` - Azure Blob Storage provider.
- `EllipticBit.Services.Storage.LocalFile` - local file system provider.

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
