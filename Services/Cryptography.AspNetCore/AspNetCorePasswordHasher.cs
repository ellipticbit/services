﻿//----------------------------------------------------------------
// Copyright (c) 2021-2023 EllipticBit, LLC - All Rights Reserved.
//----------------------------------------------------------------

using Microsoft.AspNetCore.Identity;

namespace EllipticBit.Services.Cryptography
{
	public class AspNetCorePasswordHasher<TUser> : IPasswordHasher<TUser> where TUser : class
	{
		private readonly ICryptographyService cryptographyService;

		public AspNetCorePasswordHasher(ICryptographyService cryptographyService) {
			this.cryptographyService = cryptographyService;
		}

		public string HashPassword(TUser user, string password) {
			return cryptographyService.SecurePassword(password, null).ToString();
		}

		public PasswordVerificationResult VerifyHashedPassword(TUser user, string hashedPassword, string providedPassword) {
			var result = cryptographyService.VerifyPassword(hashedPassword, HashedPassword.FromBase64(providedPassword), null);
			return result == VerifyPasswordResult.Failure ? PasswordVerificationResult.Failed : result == VerifyPasswordResult.Success ? PasswordVerificationResult.Success : PasswordVerificationResult.SuccessRehashNeeded;
		}
	}
}
