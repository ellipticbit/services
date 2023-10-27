using System;

namespace EllipticBit.Services.Email
{
	public interface IEmailServiceFactory
	{
		/// <summary>
		/// Create a basic Email Service.
		/// </summary>
		/// <param name="name">The name of the service to create.</param>
		/// <returns cref="IEmailService">An IEmailService interface to the implementation.</returns>
		/// <exception cref="ArgumentOutOfRangeException">No service was registered the implements the IEmailService interface.</exception>
		IEmailService Create(string name = null);

		/// <summary>
		/// Create an Email Service that supports sending templated emails.
		/// </summary>
		/// <param name="name">The name of the service to create.</param>
		/// <returns cref="IEmailTemplateService">An IEmailTemplateService interface to the implementation.</returns>
		/// <exception cref="ArgumentNullException">No email service was specified.</exception>
		/// <exception cref="ArgumentOutOfRangeException">No service was registered the implements the IEmailTemplateService interface.</exception>
		IEmailTemplateService CreateTemplate(string name);

		/// <summary>
		/// Create an Email Service that supports scheduling of emails.
		/// </summary>
		/// <param name="name">The name of the service to create.</param>
		/// <returns cref="IEmailScheduleService">An IEmailScheduleService interface to the implementation.</returns>
		/// <exception cref="ArgumentNullException">No email service was specified.</exception>
		/// <exception cref="ArgumentOutOfRangeException">No service was registered the implements the IEmailScheduleService interface.</exception>
		IEmailScheduleService CreateSchedule(string name);
	}
}
