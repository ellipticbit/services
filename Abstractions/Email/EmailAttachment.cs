//-----------------------------------------------------------------------------
// Copyright (c) 2020 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using System;

namespace EllipticBit.Services.Email
{
	public sealed class EmailAttachment
	{
		public string Id { get; }
		public string Content { get; }
		public string FileName { get; }
		public string Type { get; }

		public EmailAttachment(byte[] contentBytes, string contentType, string fileName, string id = null) {
			this.Id = id;
			this.Content = Convert.ToBase64String(contentBytes);
			this.Type = contentType;
			this.FileName = fileName;
		}
	}
}
