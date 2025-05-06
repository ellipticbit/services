using System;
using System.Collections.Frozen;
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
		private static FrozenDictionary<string, ISchedulerAction> actions;
		private static ImmutableDictionary<string, ActionExecution> enabledActions = ImmutableDictionary<string, ActionExecution>.Empty;

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
			var el = enabledActions.Values.Where(a => a.IntervalMode != SchedulerActionIntervalMode.Manual && a.Next < DateTimeOffset.UtcNow).ToList();

			return Task.WhenAll(el.Select(action => action.Execute()).ToArray());
		}

		private readonly ISchedulerSynchronizationContext instanceSync;

		public InstanceSchedulerService(IEnumerable<ISchedulerAction> actions, TInstance instanceSync) {
			InstanceSchedulerService<TInstance>.actions = actions.ToFrozenDictionary(a => a.GetType().FullName);
			this.instanceSync = instanceSync;
		}

		public async void Start() {
			if (enabledActions.Any(a => a.Value.ExecuteOnStart)) {
				await Task.WhenAll(enabledActions.Where(a => a.Value.ExecuteOnStart).Select(ea => ea.Value.Execute()).ToArray()).ConfigureAwait(true);
			}

			timer.Start();
		}

		public void Stop() {
			timer.Stop();
		}

		public void Enable<TAction>() where TAction : class, ISchedulerAction {
			var typeStr = typeof(TAction).FullName;
			if (!actions.TryGetValue(typeStr, out ISchedulerAction action)) {
				throw new ArgumentOutOfRangeException(nameof(TAction), $"No registered action of type: {typeStr}");
			}

			if (enabledActions.ContainsKey(typeStr)) return;
			if (action.SynchronizationMode == SchedulerActionSynchronizationMode.Network) throw new ArgumentOutOfRangeException($"Unable to enable service: {typeStr}. Service '{typeStr}' is registered as a network service and no network synchronization context has been registered with this scheduler.");

			var ctx = action.SynchronizationMode == SchedulerActionSynchronizationMode.Instance ? instanceSync : null;
			enabledActions = enabledActions.Add(typeStr, new ActionExecution(action, ctx));
		}

		public void Disable<TAction>() where TAction : class, ISchedulerAction {
			enabledActions = enabledActions.Remove(typeof(TAction).FullName);
		}

		public Task Execute<TAction>() where TAction : class, ISchedulerAction {
			var typeStr = typeof(TAction).FullName;
			if (!actions.TryGetValue(typeStr, out ISchedulerAction action)) {
				throw new ArgumentOutOfRangeException(nameof(TAction), $"No action registered of type: {typeStr}");
			}

			if (action.SynchronizationMode == SchedulerActionSynchronizationMode.Network) throw new ArgumentOutOfRangeException($"Unable to enable service: {typeStr}. Service '{typeStr}' is registered as a network service and no network synchronization context has been registered with this scheduler.");

			var ctx = action.SynchronizationMode == SchedulerActionSynchronizationMode.Instance ? instanceSync : null;
			var execution = new ActionExecution(action, ctx);
			return execution.Execute();
		}
	}
}
