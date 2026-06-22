# EllipticBit Services - USPS Address Validation

`EllipticBit.Services.Address.USPS` is a [USPS](https://developer.usps.com/) address
validation and standardization provider for the EllipticBit Services address
abstractions. It implements `IAddressService` using the USPS APIs and ships with a
pre-configured `HttpClient` and in-memory response caching.

This package is part of the [EllipticBit Services](https://github.com/ellipticbit/services)
collection of cross-platform service libraries for .NET.

## Features

- Address validation and standardization backed by the official USPS APIs.
- Configured, named `HttpClient` with automatic authentication handling.
- Optional testing endpoint for development and integration environments.
- Helper extensions for validating ZIP codes and checkable addresses.

## Installation

Install the package from [NuGet](https://www.nuget.org/packages/EllipticBit.Services.Address.USPS):

```shell
dotnet add package EllipticBit.Services.Address.USPS
```

Or add a `PackageReference` to your project file:

```xml
<PackageReference Include="EllipticBit.Services.Address.USPS" Version="x.y.z" />
```

## Usage

Register the USPS address service with the dependency injection container:

```csharp
using EllipticBit.Services.Address;
using Microsoft.Extensions.DependencyInjection;

services.AddAddressUsps(new UspsAddressServiceOptions {
	ClientId = "your-usps-client-id",
	ClientSecret = "your-usps-client-secret",
	UseTestingEndpoint = false
});
```

Then inject `IAddressService` wherever you need address validation:

```csharp
using EllipticBit.Services.Address;

public class ShippingService
{
	private readonly IAddressService _addressService;

	public ShippingService(IAddressService addressService)
	{
		_addressService = addressService;
	}

	public async Task<Address> StandardizeAsync(Address address)
	{
		return await _addressService.Validate(address);
	}
}
```

## Related Packages

- `EllipticBit.Services.Address.Abstractions` - the address service abstractions implemented by this package.

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
