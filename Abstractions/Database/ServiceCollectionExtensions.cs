using Microsoft.Extensions.DependencyInjection;

namespace EllipticBit.Services.Database
{
	public static class ServiceCollectionExtensions
	{
		public static IDatabaseServiceBuilder AddDatabaseServices(this IServiceCollection services) {
			return AddDatabaseServices<DefaultDatabaseConflictDetection, LocalDatabaseConflictResolver>(services);
		}

		public static IDatabaseServiceBuilder AddDatabaseServices<TDefaultDetection, TDefaultResolver>(this IServiceCollection services)
			where TDefaultDetection : class, IDatabaseConflictDetection, new()
			where TDefaultResolver : class, IDatabaseConflictResolver, new() {
			services.AddTransient<IDatabaseConflictDetection, DefaultDatabaseConflictDetection>();
			services.AddTransient<IDatabaseConflictResolver, LocalDatabaseConflictResolver>();
			services.AddTransient<IDatabaseConflictResolver, RemoteDatabaseConflictResolver>();
			services.AddTransient<IDatabaseServiceFactory, DatabaseServiceFactory<TDefaultDetection, TDefaultResolver>>();

			return new DatabaseServiceFactory<TDefaultDetection, TDefaultResolver>(services);
		}
	}
}
