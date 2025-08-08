using Microsoft.Extensions.DependencyInjection;

namespace EllipticBit.Services.Database
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddEntityFrameworkExtensions(this IServiceCollection services)
		{
			return AddEntityFrameworkExtensions<DefaultDatabaseConflictDetection, RemoteDatabaseConflictResolver>(services);
		}

		public static IServiceCollection AddEntityFrameworkExtensions<TDefaultResolver>(this IServiceCollection services)
			where TDefaultResolver : class, IDatabaseConflictResolver, new()
		{
			return AddEntityFrameworkExtensions<DefaultDatabaseConflictDetection, TDefaultResolver>(services);
		}

		public static IServiceCollection AddEntityFrameworkExtensions<TDefaultDetection, TDefaultResolver>(this IServiceCollection services)
			where TDefaultDetection : class, IDatabaseConflictDetection, new()
			where TDefaultResolver : class, IDatabaseConflictResolver, new()
		{
			services.AddTransient<IDatabaseConflictDetection, TDefaultDetection>();
			services.AddTransient<IDatabaseConflictResolver, TDefaultResolver>();

			return services;
		}
	}
}
