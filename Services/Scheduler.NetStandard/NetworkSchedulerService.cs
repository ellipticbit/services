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
	internal class NetworkSchedulerService<TInstance, TNetwork> : ISchedulerService
		where TInstance : class, ISchedulerSynchronizationContext
		where TNetwork : class, ISchedulerSynchronizationContext
	{
		private static FrozenDictionary<string, ISchedulerAction> actions;
		private static ImmutableDictionary<string, ActionExecution> enabledActions = ImmutableDictionary<string, ActionExecution>.Empty;

#if WPF
		private static readonly DispatcherTimer itimer = new DispatcherTimer(DispatcherPriority.Background);
		private static readonly DispatcherTimer ntimer = new DispatcherTimer(DispatcherPriority.Background);

		static NetworkSchedulerService() {
			itimer.Interval = TimeSpan.FromSeconds(1);
			itimer.Tick += ITimer_Tick;
			ntimer.Interval = TimeSpan.FromSeconds(10);
			ntimer.Tick += NTimer_Tick;
		}

		private static async void ITimer_Tick(object sender, EventArgs e)
		{
			await RunInstanceScheduled().ConfigureAwait(true);
		}

		private static async void NTimer_Tick(object sender, EventArgs e)
		{
			await RunInstanceScheduled().ConfigureAwait(true);
		}

#else
		private static readonly Timer itimer = new Timer(1000);
		private static readonly Timer ntimer = new Timer(10000);

		static NetworkSchedulerService() {
			itimer.Elapsed += ITimer_Elapsed;
			ntimer.Elapsed += NTimer_Elapsed;
		}

		private static async void ITimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			await RunInstanceScheduled().ConfigureAwait(false);
		}

		private static async void NTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			await RunNetworkScheduled().ConfigureAwait(false);
		}
#endif

		private static Task RunInstanceScheduled()
		{
			var el = enabledActions.Values.Where(a => a.IntervalMode != SchedulerActionIntervalMode.Manual && a.Mode != SchedulerActionSynchronizationMode.Network && a.Next < DateTimeOffset.UtcNow).ToList();

			return Task.WhenAll(el.Select(action => action.Execute()).ToArray());
		}

		private static Task RunNetworkScheduled()
		{
			var el = enabledActions.Values.Where(a => a.IntervalMode != SchedulerActionIntervalMode.Manual && a.Mode == SchedulerActionSynchronizationMode.Network).ToList();

			return Task.WhenAll(el.Select(action => action.Execute()).ToArray());
		}

		private readonly ISchedulerSynchronizationContext instanceSync;
		private readonly ISchedulerSynchronizationContext networkSync;

		public NetworkSchedulerService(IEnumerable<ISchedulerAction> actions, TInstance instanceSync, TNetwork networkSync) {
			var schedulerActions = actions as ISchedulerAction[] ?? actions.ToArray();
			if (schedulerActions.GroupBy(a => a.Id).Any(b => b.Count() > 1)) {
				throw new InvalidOperationException($"Unable to register actions. Multiple actions with the same ID found.");
			}

			NetworkSchedulerService<TInstance, TNetwork>.actions = schedulerActions.ToFrozenDictionary(a => a.GetType().FullName);
			this.instanceSync = instanceSync;
			this.networkSync = networkSync;
		}

		public async void Start() {
			if (enabledActions.Any(a => a.Value.ExecuteOnStart))
			{
				await Task.WhenAll(enabledActions.Where(a => a.Value.ExecuteOnStart).Select(ea => ea.Value.Execute()).ToArray()).ConfigureAwait(true);
			}

			itimer.Start();
			ntimer.Start();
		}

		public void Stop() {
			itimer.Stop();
			ntimer.Stop();
		}

		public void Enable<TAction>() where TAction : class, ISchedulerAction {
			var typeStr = typeof(TAction).FullName;
			if (!actions.TryGetValue(typeStr, out ISchedulerAction action)) {
				throw new ArgumentOutOfRangeException(nameof(TAction), $"No registered action of type: {typeStr}");
			}

			if (enabledActions.ContainsKey(typeStr)) return;

			var ctx = action.SynchronizationMode == SchedulerActionSynchronizationMode.Network ? networkSync : action.SynchronizationMode == SchedulerActionSynchronizationMode.Instance ? instanceSync : null;
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

			var ctx = action.SynchronizationMode == SchedulerActionSynchronizationMode.Network ? networkSync : action.SynchronizationMode == SchedulerActionSynchronizationMode.Instance ? instanceSync : null;
			var execution = new ActionExecution(action, ctx);
			return execution.Execute();
		}
	}
}
