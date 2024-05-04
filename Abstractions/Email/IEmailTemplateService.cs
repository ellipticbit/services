//-----------------------------------------------------------------------------
// Copyright (c) 2023-2024 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;

namespace EllipticBit.Services.Email
{
	public interface IEmailTemplateService : IEmailService
	{
		Task<TResult> Send<T, TResult>(IEnumerable<T> templateData, EmailAddress from = null) where T : EmailTemplateBase;
		Task<TResult> Send<T, TResult>(T templateData, EmailAddress from = null) where T : EmailTemplateBase;
	}
}
