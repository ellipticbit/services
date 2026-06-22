# EllipticBit Services - Cryptography Abstractions

`EllipticBit.Services.Cryptography.Abstractions` contains the service abstractions and
interfaces for cryptographic operations in the EllipticBit Services library. It
defines the contracts for hashing, message authentication (MAC), key derivation, and
symmetric encryption, along with the supported algorithm enumerations.

This package is part of the [EllipticBit Services](https://github.com/ellipticbit/services)
collection of cross-platform service libraries for .NET.

## Features

- `ICryptographyService`, `ICryptographyHash`, `ICryptographyMac`, `ICryptographyKdf`, and `ICryptographySymmetric` contracts.
- `EncryptionAlgorithm`, `HashAlgorithm`, and `PasswordAlgorithm` enumerations.
- Shared models such as `EncryptedData` and `HashedPassword`.

## Installation

Install the package from [NuGet](https://www.nuget.org/packages/EllipticBit.Services.Cryptography.Abstractions):

```shell
dotnet add package EllipticBit.Services.Cryptography.Abstractions
```

Or add a `PackageReference` to your project file:

```xml
<PackageReference Include="EllipticBit.Services.Cryptography.Abstractions" Version="x.y.z" />
```

## Usage

Depend on the cryptography abstractions in your services and let dependency injection
supply a concrete implementation at runtime:

```csharp
using EllipticBit.Services.Cryptography;

public class PasswordService
{
	private readonly ICryptographyService _cryptography;

	public PasswordService(ICryptographyService cryptography)
	{
		_cryptography = cryptography;
	}

	public Task<HashedPassword> HashAsync(string password)
		=> _cryptography.HashPassword(PasswordAlgorithm.Argon2id, password);
}
```

Register a concrete implementation in your composition root:

```csharp
// dotnet add package EllipticBit.Services.Cryptography.DotNet
services.AddCryptographyServices();
```

## Implementation Packages

- `EllipticBit.Services.Cryptography.DotNet` - modern .NET implementation.
- `EllipticBit.Services.Cryptography.NetFramework` - .NET Framework implementation.
- `EllipticBit.Services.Cryptography.AspNetCore` - ASP.NET Core Identity password hashing.

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
