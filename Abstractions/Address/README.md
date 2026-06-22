# EllipticBit Services - Address Abstractions

`EllipticBit.Services.Address.Abstractions` contains the service abstractions and
interfaces for address validation and standardization in the EllipticBit Services
library. Reference this package when you want to depend on the address service
contracts (such as `IAddressService`) without binding to a specific provider.

This package is part of the [EllipticBit Services](https://github.com/ellipticbit/services)
collection of cross-platform service libraries for .NET.

## Features

- Provider-agnostic `IAddressService` contract for address validation.
- Shared `Address` model and helper extensions.
- Lets application and library code depend on abstractions, not implementations.

## Installation

Install the package from [NuGet](https://www.nuget.org/packages/EllipticBit.Services.Address.Abstractions):

```shell
dotnet add package EllipticBit.Services.Address.Abstractions
```

Or add a `PackageReference` to your project file:

```xml
<PackageReference Include="EllipticBit.Services.Address.Abstractions" Version="x.y.z" />
```

## Usage

Depend on the `IAddressService` abstraction in your services and let dependency
injection supply a concrete provider at runtime:

```csharp
using EllipticBit.Services.Address;

public class CheckoutService
{
	private readonly IAddressService _addressService;

	public CheckoutService(IAddressService addressService)
	{
		_addressService = addressService;
	}

	public Task<Address> ValidateAsync(Address address)
		=> _addressService.Validate(address);
}
```

Register a concrete provider (for example the USPS provider) in your composition root:

```csharp
// dotnet add package EllipticBit.Services.Address.USPS
services.AddAddressUsps(options);
```

## Provider Packages

- `EllipticBit.Services.Address.USPS` - USPS address validation provider.

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
