namespace EllipticBit.Services.Database
{
	public interface IDatabaseServiceBuilder
	{
		IDatabaseServiceBuilder AddDatabaseService(string name, IDatabaseServiceOptions options);
		IDatabaseServiceBuilder AddDatabaseConflictDetector<T>() where T : class, IDatabaseConflictDetection;
		IDatabaseServiceBuilder AddDatabaseConflictResolver<T>() where T : class, IDatabaseConflictResolver;
	}
}
