namespace EllipticBit.Services.Database
{
	public interface IDatabaseServiceFactory
	{

		TDatabase Create<TDatabase>(string name)
			where TDatabase : class, IDatabaseService<TDatabase>, new();

		TDatabase Create<TDatabase, TDetector, TResolver>(string name)
			where TDatabase : class, IDatabaseService<TDatabase>, new()
			where TDetector : class, IDatabaseConflictDetection
			where TResolver : class, IDatabaseConflictResolver;
	}
}
