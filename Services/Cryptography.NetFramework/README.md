# EllipticBit Services - Cryptography (.NET Framework)

`EllipticBit.Services.Cryptography.NetFramework` is the .NET Framework implementation
of the EllipticBit cryptography abstractions. It provides hashing, message
authentication (MAC), key derivation (Argon2 and Scrypt) and symmetric encryption
services for applications targeting .NET Framework, with the same API surface as the
modern .NET implementation.

This package is part of the [EllipticBit Services](https://github.com/ellipticbit/services)
collection of cross-platform service libraries for .NET.

## Features

- Cryptographic hashing via `ICryptographyHash`.
- Message authentication codes via `ICryptographyMac`.
- Password key derivation (Argon2, Scrypt) via `ICryptographyKdf`.
- Symmetric encryption via `ICryptographySymmetric`.
- Modern algorithm support (AES-GCM, ChaCha20-Poly1305, HKDF) on .NET Framework.

## Installation

Install the package from [NuGet](https://www.nuget.org/packages/EllipticBit.Services.Cryptography.NetFramework):

```shell
dotnet add package EllipticBit.Services.Cryptography.NetFramework
```

Or add a `PackageReference` to your project file:

```xml
<PackageReference Include="EllipticBit.Services.Cryptography.NetFramework" Version="x.y.z" />
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

public class SigningService
{
	private readonly ICryptographyMac _mac;

	public SigningService(ICryptographyMac mac)
	{
		_mac = mac;
	}

	public async Task<byte[]> SignAsync(byte[] key, byte[] data)
		=> await _mac.HashData(HashAlgorithm.Sha256, key, data);
}
```

## Related Packages

- `EllipticBit.Services.Cryptography.Abstractions` - the cryptography abstractions implemented by this package.
- `EllipticBit.Services.Cryptography.DotNet` - the equivalent implementation for modern .NET.

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
