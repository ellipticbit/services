﻿//-----------------------------------------------------------------------------
// Copyright (c) 2020-2025 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

namespace EllipticBit.Services.Email
{
	public sealed class EmailAttachment : IEmailAttachment
	{
		public string Id { get; }
		public byte[] Content { get; }
		public string FileName { get; }
		public string Type { get; }

		public EmailAttachment(byte[] contentBytes, string contentType, string fileName, string id = null) {
			this.Id = id;
			this.Content = contentBytes;
			this.Type = contentType;
			this.FileName = fileName;
		}
	}
}
