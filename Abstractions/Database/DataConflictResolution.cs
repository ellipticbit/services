namespace EllipticBit.Services.Database
{
	public sealed class DataConflictResolution
	{
		public string Name { get; }
		public object Resolved { get; }

		internal DataConflictResolution(string name, object resolved)
		{
			this.Name = name;
			this.Resolved = resolved;
		}
	}
}
