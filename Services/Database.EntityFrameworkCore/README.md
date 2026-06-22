# EllipticBit Services - Entity Framework Core Database

`EllipticBit.Services.Database.EntityFrameworkCore` is the Entity Framework Core
implementation of the EllipticBit database abstractions. It provides a transaction
aware `DbContext` base class together with change detection and conflict resolution
helpers for EF Core.

This package is part of the [EllipticBit Services](https://github.com/ellipticbit/services)
collection of cross-platform service libraries for .NET.

## Features

- `EfCoreDatabaseService<TDatabase>` base `DbContext` with ambient transaction support.
- Strongly-typed `Transaction(...)` helpers using `TransactionScope`.
- Change detection and conflict resolution extensions for EF Core entities.

## Installation

Install the package from [NuGet](https://www.nuget.org/packages/EllipticBit.Services.Database.EntityFrameworkCore):

```shell
dotnet add package EllipticBit.Services.Database.EntityFrameworkCore
```

Or add a `PackageReference` to your project file:

```xml
<PackageReference Include="EllipticBit.Services.Database.EntityFrameworkCore" Version="x.y.z" />
```

## Usage

Derive your `DbContext` from `EfCoreDatabaseService<TDatabase>`:

```csharp
using EllipticBit.Services.Database;
using Microsoft.EntityFrameworkCore;

public class AppDatabase : EfCoreDatabaseService<AppDatabase>
{
	public AppDatabase(DbContextOptions options) : base(options) { }

	public DbSet<Customer> Customers { get; set; }
}
```

Register the EF Core extensions and your context with dependency injection:

```csharp
using EllipticBit.Services.Database;
using Microsoft.Extensions.DependencyInjection;

services.AddEntityFrameworkExtensions();
services.AddDbContext<AppDatabase>(options =>
	options.UseSqlServer(connectionString));
```

Run a unit of work inside a transaction:

```csharp
await database.Transaction(async db =>
{
	db.Customers.Add(new Customer { Name = "Contoso" });
	await db.SaveChangesAsync();
});
```

## Related Packages

- `EllipticBit.Services.Database.Abstractions` - the database abstractions implemented by this package.

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
