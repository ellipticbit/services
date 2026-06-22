# EllipticBit Services - Azure Blob Storage

`EllipticBit.Services.Storage.AzureBlob` is an [Azure Blob Storage](https://azure.microsoft.com/products/storage/blobs)
provider for the EllipticBit storage abstractions. It implements the unified remote
storage API backed by Azure Storage, so application code can read and write files
without taking a direct dependency on the Azure SDK.

This package is part of the [EllipticBit Services](https://github.com/ellipticbit/services)
collection of cross-platform service libraries for .NET.

## Features

- Unified `IRemoteStorageService` implementation backed by Azure Blob Storage.
- Integrates with the EllipticBit secret services for credential resolution.
- Configuration-driven setup.

## Installation

Install the package from [NuGet](https://www.nuget.org/packages/EllipticBit.Services.Storage.AzureBlob):

```shell
dotnet add package EllipticBit.Services.Storage.AzureBlob
```

Or add a `PackageReference` to your project file:

```xml
<PackageReference Include="EllipticBit.Services.Storage.AzureBlob" Version="x.y.z" />
```

## Usage

Register the Azure Blob storage service with the dependency injection container. The
registration is asynchronous because it resolves credentials through the secret
service:

```csharp
using EllipticBit.Services.Storage;
using Microsoft.Extensions.DependencyInjection;

await services.AddAzureBlobStorage(configuration, secretService);
```

Then inject `IRemoteStorageService` to read and write files:

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

## Related Packages

- `EllipticBit.Services.Storage.Abstractions` - the storage abstractions implemented by this package.
- `EllipticBit.Services.Storage.LocalFile` - a local file system provider for the same abstractions.
- `EllipticBit.Services.Secret.AzureKeyVault` - Azure Key Vault secret provider used for credentials.

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
