//-----------------------------------------------------------------------------
// Copyright (c) 2024 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using System.Collections.Generic;
using System.Net;
using SendGrid;

namespace EllipticBit.Services.Email
{
	public class SendGridResult : IEmailResult
	{
		public bool IsSuccess { get; }

		public string BatchId { get; }

		public HttpStatusCode? StatusCode { get; }

		public IDictionary<string, string> Headers { get; }

		public uint ProcessedMessages { get; }

		internal SendGridResult(Response response, string batchId = null, uint processed = 1) {
			this.BatchId = batchId;
			this.ProcessedMessages = processed;
			this.StatusCode = response?.StatusCode;
			this.Headers = response?.DeserializeResponseHeaders(response.Headers);
			this.IsSuccess = response?.IsSuccessStatusCode ?? true;
		}
	}
}
