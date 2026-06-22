# EllipticBit Services - Secret Abstractions

`EllipticBit.Services.Secret.Abstractions` contains the service abstractions and
interfaces for secret and key management in the EllipticBit Services library. It
defines the provider-agnostic contracts for retrieving secrets, managing keys, and
performing key-based cryptographic operations.

This package is part of the [EllipticBit Services](https://github.com/ellipticbit/services)
collection of cross-platform service libraries for .NET.

## Features

- `ISecretService` contract for retrieving secrets.
- `IKeyService` and `IKeyCryptographyService` contracts for key management and operations.
- Key property models and algorithm enumerations.

## Installation

Install the package from [NuGet](https://www.nuget.org/packages/EllipticBit.Services.Secret.Abstractions):

```shell
dotnet add package EllipticBit.Services.Secret.Abstractions
```

Or add a `PackageReference` to your project file:

```xml
<PackageReference Include="EllipticBit.Services.Secret.Abstractions" Version="x.y.z" />
```

## Usage

Depend on the `ISecretService` abstraction and let dependency injection supply a
concrete provider at runtime:

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

Register a concrete provider (for example Azure Key Vault) in your composition root:

```csharp
// dotnet add package EllipticBit.Services.Secret.AzureKeyVault
services.AddAzureKeyVault(configuration);
```

## Provider Packages

- `EllipticBit.Services.Secret.AzureKeyVault` - Azure Key Vault secret and key provider.

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
