# EllipticBit Services - Database Abstractions

`EllipticBit.Services.Database.Abstractions` contains the service abstractions and
interfaces for database change detection and conflict resolution in the EllipticBit
Services library. Reference this package to depend on the database contracts without
binding to a specific data access implementation.

This package is part of the [EllipticBit Services](https://github.com/ellipticbit/services)
collection of cross-platform service libraries for .NET.

## Features

- `IDatabaseConflictDetection` contract for detecting concurrent changes.
- `IDatabaseMergeable<T>` contract and extensions for merging entity changes.
- Provider-agnostic contracts shared across data access implementations.

## Installation

Install the package from [NuGet](https://www.nuget.org/packages/EllipticBit.Services.Database.Abstractions):

```shell
dotnet add package EllipticBit.Services.Database.Abstractions
```

Or add a `PackageReference` to your project file:

```xml
<PackageReference Include="EllipticBit.Services.Database.Abstractions" Version="x.y.z" />
```

## Usage

Implement the mergeable contract on entities that support conflict resolution:

```csharp
using EllipticBit.Services.Database;

public class Customer : IDatabaseMergeable<Customer>
{
	public string Name { get; set; }

	public void Merge(Customer other)
	{
		Name = other.Name;
	}
}
```

Register a concrete implementation in your composition root:

```csharp
// dotnet add package EllipticBit.Services.Database.EntityFrameworkCore
services.AddEntityFrameworkExtensions();
```

## Implementation Packages

- `EllipticBit.Services.Database.EntityFrameworkCore` - Entity Framework Core implementation.

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
