using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace EllipticBit.Services.Scheduler
{
	internal class DefaultInstanceSynchronizationContext : ISchedulerSynchronizationContext
	{
		private static ImmutableDictionary<string, ISchedulerAction> activeActions = ImmutableDictionary<string, ISchedulerAction>.Empty;

		private readonly ILogger logger;

		public DefaultInstanceSynchronizationContext(ILogger<DefaultInstanceSynchronizationContext> logger) { this.logger = logger; }

		public Task<bool> Acquire(ISchedulerAction action) {
			return Task.FromResult(ImmutableInterlocked.TryAdd(ref activeActions, action.Name, action));
		}

		public  Task Completed(ISchedulerAction action, DateTimeOffset startedAt, Exception ex = null) {
			activeActions = activeActions.Remove(action.Name);

			if (ex == null)
			{
				logger.LogInformation("Instance Scheduled job '{Name}' started at {StartedAt} and completed at {CompletedAt}.", action.Name, startedAt.ToString("G"), DateTime.UtcNow.ToString("G"));
			}
			else
			{
				logger.LogError(ex, "Instance Scheduled job '{Name}' started at {StartedAt} and failed at {CompletedAt}.", action.Name, startedAt.ToString("G"), DateTime.UtcNow.ToString("G"));
			}

			return Task.CompletedTask;
		}
	}
}
