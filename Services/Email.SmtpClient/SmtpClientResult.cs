//-----------------------------------------------------------------------------
// Copyright (c) 2024 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

namespace EllipticBit.Services.Email
{
	public class SmtpClientResult : IEmailResult
	{
		public bool IsSuccess { get; }

		internal SmtpClientResult(bool isSuccess) {
			this.IsSuccess = isSuccess;
		}
	}
}
