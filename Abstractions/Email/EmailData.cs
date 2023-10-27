//-----------------------------------------------------------------------------
// Copyright (c) 2020 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

namespace EllipticBit.Services.Email
{
	public abstract class EmailDataBase
	{
		protected readonly object _data;

		public EmailAddress Address { get; }
		public object TemplateData => _data;

		protected internal EmailDataBase(EmailAddress address, object templateData) {
			this.Address = address;
			this._data = templateData;
		}
	}

	public sealed class EmailData<T> : EmailDataBase
		where T : class
	{
		public T Data => (T)this._data;

		public EmailData(EmailAddress address, T templateData) : base(address, templateData) { }
	}
}
