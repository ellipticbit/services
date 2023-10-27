//-----------------------------------------------------------------------------
// Copyright (c) 2020 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

namespace EllipticBit.Services.Secret
{
	public class AzureKeyVaultKeyProperties : KeyProperties
	{
		public AzureKeyVaultKeyProperties(Azure.Security.KeyVault.Keys.KeyProperties props) : base(props.ExpiresOn, props.NotBefore, props.CreatedOn, props.UpdatedOn, props.VaultUri) { }
	}
}
