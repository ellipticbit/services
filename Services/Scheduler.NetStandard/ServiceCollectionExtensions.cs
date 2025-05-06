using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EllipticBit.Services.Scheduler
{
	public static class ServiceCollectionExtensions
	{
		public static ISchedulerServiceBuilder AddInstanceSchedulerServices(this IServiceCollection collection)
			=> AddInstanceSchedulerServices<DefaultInstanceSynchronizationContext>(collection);

		public static ISchedulerServiceBuilder AddInstanceSchedulerServices<TInstance>(this IServiceCollection collection)
			where TInstance : class, ISchedulerSynchronizationContext
		{
			collection.AddTransient<DefaultInstanceSynchronizationContext>();
			collection.TryAddTransient<ISchedulerService, InstanceSchedulerService<TInstance>>();
			return new ScheduleServiceBuilder(collection);
		}

		public static ISchedulerServiceBuilder AddNetworkSchedulerServices<TNetwork>(this IServiceCollection collection)
			where TNetwork : class, ISchedulerSynchronizationContext
			=> AddNetworkSchedulerServices <DefaultInstanceSynchronizationContext, TNetwork>(collection);

		public static ISchedulerServiceBuilder AddNetworkSchedulerServices<TInstance, TNetwork>(this IServiceCollection collection)
			where TInstance : class, ISchedulerSynchronizationContext
			where TNetwork : class, ISchedulerSynchronizationContext
		{
			collection.AddTransient<DefaultInstanceSynchronizationContext>();
			collection.TryAddTransient<ISchedulerService, NetworkSchedulerService<TInstance, TNetwork>>();
			return new ScheduleServiceBuilder(collection);
		}
	}

	internal class ScheduleServiceBuilder : ISchedulerServiceBuilder
	{
		private readonly IServiceCollection collection;

		public ScheduleServiceBuilder(IServiceCollection collection) {
			this.collection = collection;
		}

		public ISchedulerServiceBuilder AddAction<TAction>() where TAction : class, ISchedulerAction {
			this.collection.AddTransient<ISchedulerAction, TAction>();
			return this;
		}

		public ISchedulerServiceBuilder AddAction<TAction, TOptions>(TOptions options) where TAction : class, ISchedulerAction where TOptions : class {
			this.collection.AddTransient<ISchedulerAction, TAction>();
			this.collection.AddSingleton(options);
			return this;
		}
	}
}
