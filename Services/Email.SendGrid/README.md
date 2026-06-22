# EllipticBit Services - SendGrid Email

`EllipticBit.Services.Email.SendGrid` is a [SendGrid](https://sendgrid.com/) email
provider for the EllipticBit email abstractions. It enables transactional email
delivery, including templates and attachments, through a unified `IEmailService` API.

This package is part of the [EllipticBit Services](https://github.com/ellipticbit/services)
collection of cross-platform service libraries for .NET.

## Features

- Send plain text, HTML and templated email through SendGrid.
- Attachment and multiple-recipient support.
- Unified `IEmailService` abstraction shared with other email providers.

## Installation

Install the package from [NuGet](https://www.nuget.org/packages/EllipticBit.Services.Email.SendGrid):

```shell
dotnet add package EllipticBit.Services.Email.SendGrid
```

Or add a `PackageReference` to your project file:

```xml
<PackageReference Include="EllipticBit.Services.Email.SendGrid" Version="x.y.z" />
```

## Usage

Register the SendGrid email services with the dependency injection container:

```csharp
using EllipticBit.Services.Email;
using Microsoft.Extensions.DependencyInjection;

services.AddSendGridEmailServices()
	.Configure(options =>
	{
		options.ApiKey = "your-sendgrid-api-key";
	});
```

Then inject `IEmailService` to send messages:

```csharp
using EllipticBit.Services.Email;

public class NotificationService
{
	private readonly IEmailService _email;

	public NotificationService(IEmailService email)
	{
		_email = email;
	}

	public Task SendWelcomeAsync(string address)
		=> _email.Send(
			new EmailAddress(address),
			subject: "Welcome!",
			text: "Thanks for signing up.",
			html: "<p>Thanks for signing up.</p>");
}
```

## Related Packages

- `EllipticBit.Services.Email.Abstractions` - the email abstractions implemented by this package.
- `EllipticBit.Services.Email.SmtpClient` - an SMTP email provider for the same abstractions.

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
