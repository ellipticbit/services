# EllipticBit Services - Cryptography (.NET)

`EllipticBit.Services.Cryptography.DotNet` is the modern .NET implementation of the
EllipticBit cryptography abstractions. It provides hashing, message authentication
(MAC), key derivation (Argon2 and Scrypt) and symmetric encryption services behind a
clean, dependency-injection friendly API.

This package is part of the [EllipticBit Services](https://github.com/ellipticbit/services)
collection of cross-platform service libraries for .NET.

## Features

- Cryptographic hashing via `ICryptographyHash`.
- Message authentication codes via `ICryptographyMac`.
- Password key derivation (Argon2, Scrypt) via `ICryptographyKdf`.
- Symmetric encryption via `ICryptographySymmetric`.
- A unified `ICryptographyService` entry point.

## Installation

Install the package from [NuGet](https://www.nuget.org/packages/EllipticBit.Services.Cryptography.DotNet):

```shell
dotnet add package EllipticBit.Services.Cryptography.DotNet
```

Or add a `PackageReference` to your project file:

```xml
<PackageReference Include="EllipticBit.Services.Cryptography.DotNet" Version="x.y.z" />
```

## Usage

Register the cryptography services with the dependency injection container:

```csharp
using EllipticBit.Services.Cryptography;
using Microsoft.Extensions.DependencyInjection;

services.AddCryptographyServices();
```

Then inject any of the cryptography interfaces where required:

```csharp
using EllipticBit.Services.Cryptography;

public class TokenService
{
	private readonly ICryptographyHash _hash;

	public TokenService(ICryptographyHash hash)
	{
		_hash = hash;
	}

	public async Task<byte[]> HashAsync(byte[] data)
		=> await _hash.HashData(HashAlgorithm.Sha256, data);
}
```

## Related Packages

- `EllipticBit.Services.Cryptography.Abstractions` - the cryptography abstractions implemented by this package.
- `EllipticBit.Services.Cryptography.NetFramework` - the equivalent implementation for .NET Framework.
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
