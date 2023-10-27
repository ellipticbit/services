//-----------------------------------------------------------------------------
// Copyright (c) 2020 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using System;

namespace EllipticBit.Services.Secret
{
	public enum KeyCurve
	{
		None,
		P256,
		P256K,
		P384,
		P521,
	}

	[Flags]
	public enum KeyOperation
	{
		Encrypt = 1,
		Decrypt = 2,
		Sign = 4,
		Verify = 8,
		Wrap = 16,
		Unwrap = 32,
	}

	public abstract class KeyProperties
	{
		public DateTimeOffset? ExpiresOn { get; }
		public DateTimeOffset? NotBefore { get; }
		public DateTimeOffset? CreatedOn { get; }
		public DateTimeOffset? UpdatedOn { get; }
		public Uri KeyUri { get; }

		protected KeyProperties(DateTimeOffset? expiresOn, DateTimeOffset? notBefore) {
			this.ExpiresOn = expiresOn;
			this.NotBefore = notBefore;
			this.CreatedOn = null;
			this.UpdatedOn = null;
			this.KeyUri = null;
		}

		protected KeyProperties(DateTimeOffset? expiresOn, DateTimeOffset? notBefore, DateTimeOffset? createdOn, DateTimeOffset? updatedOn, Uri keyUri) {
			this.ExpiresOn = expiresOn;
			this.NotBefore = notBefore;
			this.CreatedOn = createdOn;
			this.UpdatedOn = updatedOn;
			this.KeyUri = keyUri;
		}
	}

	public class CreateKeyProperties : KeyProperties
	{
		public KeyOperation Operations { get; }
		public KeyCurve Curve { get; }
		public ushort KeyLength { get; }
		public bool HardwareProtected { get; }

		public CreateKeyProperties(ushort keyLength, KeyOperation operations, bool hardwareProtected = false, DateTimeOffset? expiresOn = null, DateTimeOffset? notBefore = null) : base(expiresOn, notBefore) {
			this.Operations = operations;
			this.KeyLength = keyLength;
			this.HardwareProtected = hardwareProtected;
			this.Curve = KeyCurve.None;
		}

		public CreateKeyProperties(KeyCurve curve, KeyOperation operations, bool hardwareProtected = false, DateTimeOffset? expiresOn = null, DateTimeOffset? notBefore = null) : base(expiresOn, notBefore) {
			this.Operations = operations;
			this.Curve = curve;
			this.HardwareProtected = hardwareProtected;
			this.KeyLength = 0;
		}
	}
}
