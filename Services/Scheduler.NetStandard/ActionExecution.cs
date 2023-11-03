using System;
using System.Threading.Tasks;

namespace EllipticBit.Services.Scheduler
{
	internal class ActionExecution
	{
		private readonly ISchedulerAction action;
		private readonly ISchedulerSynchronizationContext context;
		private Exception error;
		private DateTimeOffset started;
		public DateTimeOffset Next { get; set; }
		public SchedulerActionSynchronizationMode Mode => action.SynchronizationMode;
		public SchedulerActionIntervalMode IntervalMode => action.IntervalMode;
		public bool ExecuteOnStart => action.ExecuteOnStart;

		public ActionExecution(ISchedulerAction action, ISchedulerSynchronizationContext sync) {
			if (action.IntervalMode == SchedulerActionIntervalMode.Second && action.SynchronizationMode == SchedulerActionSynchronizationMode.Network) throw new NotSupportedException("Network Synchronization does not support SchedulerActionIntervalMode.Second");
			this.action = action;
			this.context = sync;
			this.started = DateTimeOffset.UtcNow;
			SetNext();
		}

		public async Task Execute()
		{
			try
			{
				started = DateTimeOffset.UtcNow;
				SetNext();
				if (await (context?.Acquire(action) ?? Task.FromResult(true)))
				{
					await action.Execute();
				}
			}
			catch (Exception ex)
			{
				error = ex;
			}
			finally
			{
				await (context?.Completed(action, started, error) ?? Task.CompletedTask);
				error = null;
			}
		}

		private void SetNext() {
			if (action.IntervalMode == SchedulerActionIntervalMode.Second) {
				Next = new DateTimeOffset(started.Year, started.Month, started.Day, started.Hour, started.Minute, started.Second, 0, TimeSpan.Zero);
				Next = Next.AddSeconds(action.Interval);
			}
			else if (action.IntervalMode == SchedulerActionIntervalMode.Minute)
			{
				Next = new DateTimeOffset(started.Year, started.Month, started.Day, started.Hour, started.Minute, 0, TimeSpan.Zero);
				Next = Next.AddMinutes(action.Interval);
			}
			else if (action.IntervalMode == SchedulerActionIntervalMode.Hour)
			{
				Next = new DateTimeOffset(started.Year, started.Month, started.Day, started.Hour, 0, 0, TimeSpan.Zero);
				Next = Next.AddHours(action.Interval);
				Next += (action.IntervalOffset ?? TimeSpan.Zero);
			}
			else if (action.IntervalMode == SchedulerActionIntervalMode.Day)
			{
				Next = new DateTimeOffset(started.Year, started.Month, started.Day, 0, 0, 0, TimeSpan.Zero);
				Next = Next.AddDays(action.Interval);
				Next += (action.IntervalOffset ?? TimeSpan.Zero);
			}
		}
	}
}
