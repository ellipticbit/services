//-----------------------------------------------------------------------------
// Copyright (c) 2023-2024 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;

namespace EllipticBit.Services.Email
{
	public interface IEmailTemplateService<in TTemplate, TResult> : IEmailService<TResult>
		where TTemplate : EmailTemplateBase
	{
		Task<TResult> Send(IEnumerable<TTemplate> templateData, EmailAddress from = null);
		Task<TResult> Send(TTemplate templateData, EmailAddress from = null);
	}
}
