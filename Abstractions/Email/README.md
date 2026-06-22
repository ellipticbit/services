# EllipticBit Services - Email Abstractions

`EllipticBit.Services.Email.Abstractions` contains the service abstractions and
interfaces for transactional email in the EllipticBit Services library. It defines the
provider-agnostic `IEmailService` contract along with supporting models for addresses,
attachments, and results.

This package is part of the [EllipticBit Services](https://github.com/ellipticbit/services)
collection of cross-platform service libraries for .NET.

## Features

- Provider-agnostic `IEmailService` contract with text, HTML, and template support.
- `IEmailServiceBuilder<T>` for fluent provider configuration.
- Shared models: `EmailAddress`, `IEmailAttachment`, and `IEmailResult`.

## Installation

Install the package from [NuGet](https://www.nuget.org/packages/EllipticBit.Services.Email.Abstractions):

```shell
dotnet add package EllipticBit.Services.Email.Abstractions
```

Or add a `PackageReference` to your project file:

```xml
<PackageReference Include="EllipticBit.Services.Email.Abstractions" Version="x.y.z" />
```

## Usage

Depend on the `IEmailService` abstraction and let dependency injection supply a
concrete provider at runtime:

```csharp
using EllipticBit.Services.Email;

public class WelcomeMailer
{
	private readonly IEmailService _email;

	public WelcomeMailer(IEmailService email)
	{
		_email = email;
	}

	public Task SendAsync(string address)
		=> _email.Send(new EmailAddress(address), "Welcome!", "Thanks for joining.");
}
```

Register a concrete provider (for example SendGrid) in your composition root:

```csharp
// dotnet add package EllipticBit.Services.Email.SendGrid
services.AddSendGridEmailServices();
```

## Provider Packages

- `EllipticBit.Services.Email.SendGrid` - SendGrid email provider.
- `EllipticBit.Services.Email.SmtpClient` - SMTP email provider.

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
