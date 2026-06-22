# EllipticBit Extensions

`EllipticBit.Extensions` is a small library of common helper extension methods shared
across the EllipticBit Services libraries. It includes utilities for system types, IO
operations, and dictionaries that are useful in everyday application code.

This package is part of the [EllipticBit Services](https://github.com/ellipticbit/services)
collection of cross-platform service libraries for .NET.

## Features

- `SystemExtensions` - helpers for common system types.
- `IOExtensions` - helpers for stream and IO operations.
- `DictionaryExtensions` - helpers for working with dictionaries.

## Installation

Install the package from [NuGet](https://www.nuget.org/packages/EllipticBit.Extensions):

```shell
dotnet add package EllipticBit.Extensions
```

Or add a `PackageReference` to your project file:

```xml
<PackageReference Include="EllipticBit.Extensions" Version="x.y.z" />
```

## Usage

Import the namespace and use the extension methods directly on the relevant types:

```csharp
using EllipticBit.Extensions;

var value = dictionary.GetValueOrDefault("key", "fallback");
```

These helpers are static extension methods, so no dependency injection registration is
required.

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
