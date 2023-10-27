using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EllipticBit.Services.Scheduler
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddSchedulerSqlServerSynchronization (this IServiceCollection builder, SqlServerNetworkSynchronizationOptions options) {
			builder.AddSingleton(options);
			builder.AddTransient<SqlSeverSchedulerSynchronization>();
			return builder;
		}
	}
}
