namespace EllipticBit.Services.Database
{
	public enum DatabaseValueType
	{
		Bool,
		Byte,
		Short,
		UShort,
		Int,
		UInt,
		Long,
		ULong,
		Float,
		Double,
		Decimal,
		ByteArray,
		String,
		Guid,
		TimeSpan,
		DateTime,
		DateTimeOffset,
		Object,
		List,
	}

	public sealed class DataValue
	{
		public DatabaseValueType DataType { get; }
		public bool IsNullable { get; }
		public string Name { get; }
		public string Title { get; }
		public object Value { get; }

		public DataValue(DatabaseValueType dataType, bool isNullable, object value, string name, string title = null) {
			this.DataType = dataType;
			this.IsNullable = isNullable;
			this.Value = value;
			this.Name = name;
			this.Title = title ?? name;
		}
	}
}
