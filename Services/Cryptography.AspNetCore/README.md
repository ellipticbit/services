# EllipticBit Services - ASP.NET Core Cryptography

`EllipticBit.Services.Cryptography.AspNetCore` provides an ASP.NET Core Identity
`IPasswordHasher<TUser>` implementation built on top of the EllipticBit cryptography
services. It lets you use the EllipticBit password hashing algorithms as a drop-in
replacement for the default ASP.NET Core Identity password hasher.

This package is part of the [EllipticBit Services](https://github.com/ellipticbit/services)
collection of cross-platform service libraries for .NET.

## Features

- Drop-in `IPasswordHasher<TUser>` implementation for ASP.NET Core Identity.
- Backed by the EllipticBit cryptography services (Argon2 / Scrypt / PBKDF2).
- Simple single-call dependency injection registration.

## Installation

Install the package from [NuGet](https://www.nuget.org/packages/EllipticBit.Services.Cryptography.AspNetCore):

```shell
dotnet add package EllipticBit.Services.Cryptography.AspNetCore
```

Or add a `PackageReference` to your project file:

```xml
<PackageReference Include="EllipticBit.Services.Cryptography.AspNetCore" Version="x.y.z" />
```

## Usage

Register the password hasher for your identity user type:

```csharp
using EllipticBit.Services.Cryptography;
using Microsoft.Extensions.DependencyInjection;

services.AddCryptographyServices();
services.AddPasswordHashing<ApplicationUser>();
```

ASP.NET Core Identity will now use the EllipticBit password hasher whenever it
hashes or verifies a password:

```csharp
public class AccountService
{
	private readonly IPasswordHasher<ApplicationUser> _hasher;

	public AccountService(IPasswordHasher<ApplicationUser> hasher)
	{
		_hasher = hasher;
	}

	public string Hash(ApplicationUser user, string password)
		=> _hasher.HashPassword(user, password);
}
```

## Related Packages

- `EllipticBit.Services.Cryptography.Abstractions` - the cryptography abstractions.
- `EllipticBit.Services.Cryptography.DotNet` - the modern .NET cryptography implementation.

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
