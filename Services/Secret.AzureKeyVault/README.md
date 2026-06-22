# EllipticBit Services - Azure Key Vault Secrets

`EllipticBit.Services.Secret.AzureKeyVault` is an [Azure Key Vault](https://azure.microsoft.com/products/key-vault)
provider for the EllipticBit secret abstractions. It implements `ISecretService` (and
related key services) over Azure Key Vault, with optional in-memory caching of
retrieved secrets and keys.

This package is part of the [EllipticBit Services](https://github.com/ellipticbit/services)
collection of cross-platform service libraries for .NET.

## Features

- Retrieve secrets and keys from Azure Key Vault through `ISecretService`.
- Optional in-memory caching for reduced round-trips.
- Configuration-driven setup.

## Installation

Install the package from [NuGet](https://www.nuget.org/packages/EllipticBit.Services.Secret.AzureKeyVault):

```shell
dotnet add package EllipticBit.Services.Secret.AzureKeyVault
```

Or add a `PackageReference` to your project file:

```xml
<PackageReference Include="EllipticBit.Services.Secret.AzureKeyVault" Version="x.y.z" />
```

## Usage

Register the Azure Key Vault secret service with the dependency injection container,
passing your application configuration:

```csharp
using EllipticBit.Services.Secret;
using Microsoft.Extensions.DependencyInjection;

services.AddAzureKeyVault(configuration);
```

To enable in-memory caching of secrets and keys, use the cached registration instead:

```csharp
services.AddAzureKeyVaultCached(configuration);
```

Then inject `ISecretService` to read secrets:

```csharp
using EllipticBit.Services.Secret;

public class ConnectionFactory
{
	private readonly ISecretService _secrets;

	public ConnectionFactory(ISecretService secrets)
	{
		_secrets = secrets;
	}

	public Task<string> GetConnectionStringAsync()
		=> _secrets.GetSecret("database-connection-string");
}
```

## Related Packages

- `EllipticBit.Services.Secret.Abstractions` - the secret abstractions implemented by this package.

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
