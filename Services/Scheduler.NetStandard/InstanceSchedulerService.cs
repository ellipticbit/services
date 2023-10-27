using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
#if WPF
using System.Windows.Threading;
#else
using System.Timers;
#endif
namespace EllipticBit.Services.Scheduler
{
	internal class InstanceSchedulerService<TInstance> : ISchedulerService
		where TInstance : class, ISchedulerSynchronizationContext
	{
		private static ImmutableDictionary<int, ISchedulerAction> actions = ImmutableDictionary<int, ISchedulerAction>.Empty;
		private static ImmutableDictionary<int, ActionExecution> enabledActions = ImmutableDictionary<int, ActionExecution>.Empty;

#if WPF
		private static readonly DispatcherTimer timer = new DispatcherTimer(DispatcherPriority.Background);

		static InstanceSchedulerService() {
			timer.Interval = TimeSpan.FromSeconds(1);
			timer.Tick += Timer_Tick;
		}

		private static async void Timer_Tick(object sender, EventArgs e)
		{
			await RunScheduled().ConfigureAwait(true);
		}

#else
		private static readonly Timer timer = new Timer(1000);

		static InstanceSchedulerService() {
			timer.Elapsed += Timer_Elapsed;
		}

		private static async void Timer_Elapsed(object sender, ElapsedEventArgs e) {
			await RunScheduled().ConfigureAwait(false);
		}
#endif

		private static Task RunScheduled() {
			var el = enabledActions.Values.Where(a => a.IntervalMode != SchedulerActionIntervalMode.Command && a.Next < DateTimeOffset.UtcNow).ToList();

			return Task.WhenAll(el.Select(action => action.Execute()).ToArray());
		}

		private readonly ISchedulerSynchronizationContext instanceSync;

		public InstanceSchedulerService(IEnumerable<ISchedulerAction> actions, TInstance instanceSync) {
			InstanceSchedulerService<TInstance>.actions = actions.ToImmutableDictionary(a => a.Id);
			this.instanceSync = instanceSync;
		}

		public void Start() {
			timer.Start();
		}

		public void Stop() {
			timer.Stop();
		}

		public void Enable(int actionId) {
			if (!actions.TryGetValue(actionId, out ISchedulerAction action)) {
				throw new ArgumentOutOfRangeException(nameof(actionId), $"No action registered with ID: {actionId}");
			}

			if (enabledActions.ContainsKey(actionId)) return;
			if (action.SynchronizationMode == SchedulerActionSynchronizationMode.Network) throw new ArgumentOutOfRangeException($"Unable to enable service ID: {actionId}. Service {actionId} is registered as a network service and no network synchronization context has been registered with this scheduler.");

			var ctx = action.SynchronizationMode == SchedulerActionSynchronizationMode.Instance ? instanceSync : null;
			enabledActions = enabledActions.Add(action.Id, new ActionExecution(action, ctx));
		}

		public void Disable(int actionId) {
			enabledActions = enabledActions.Remove(actionId);
		}

		public Task Execute(int actionId) {
			if (!actions.TryGetValue(actionId, out ISchedulerAction action))
			{
				throw new ArgumentOutOfRangeException(nameof(actionId), $"No action registered with ID: {actionId}");
			}

			if (action.SynchronizationMode == SchedulerActionSynchronizationMode.Network) throw new ArgumentOutOfRangeException($"Unable to enable service ID: {actionId}. Service {actionId} is registered as a network service and no network synchronization context has been registered with this scheduler.");

			var ctx = action.SynchronizationMode == SchedulerActionSynchronizationMode.Instance ? instanceSync : null;
			var execution = new ActionExecution(action, ctx);
			return execution.Execute();
		}
	}
}
