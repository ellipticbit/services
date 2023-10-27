namespace EllipticBit.Services.Email
{
	public interface IEmailServiceBuilder
	{
		IEmailServiceBuilder AddEmailService(string name, EmailServiceOptions options);
	}
}
