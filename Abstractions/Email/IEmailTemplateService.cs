//-----------------------------------------------------------------------------
// Copyright (c) 2023-2024 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;

namespace EllipticBit.Services.Email
{
	public interface IEmailTemplateService : IEmailService
	{
		Task<IEmailResult> Send<TTemplate>(IEnumerable<TTemplate> templateData, EmailAddress from = null)
			where TTemplate : EmailTemplateBase;
	}

	public static class EmailTemplateServiceExtensions
	{
		public static Task<IEmailResult> Send<TTemplate>(this IEmailTemplateService service, TTemplate templateData, EmailAddress from = null)
			where TTemplate : EmailTemplateBase
		{
			return service.Send(new[] { templateData }, from);
		}
	}
}
