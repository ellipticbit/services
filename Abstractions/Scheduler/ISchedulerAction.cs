using System;
using System.Threading.Tasks;

namespace EllipticBit.Services.Scheduler
{
	public enum SchedulerActionSynchronizationMode
	{
		/// <summary>
		/// This action will run on schedule in the current instance even if another execution is currently running the instance.
		/// </summary>
		None,
		/// <summary>
		/// If the action is already running then the new execution will be skipped. This action is run in the current instance and no synchronization with other instances or nodes is performed.
		/// </summary>
		Instance,
		/// <summary>
		/// This action is run once network-wide using a shared data system to synchronize the execution and block other instances from executing this action.
		/// </summary>
		Network,
	}

	public enum SchedulerActionIntervalMode
	{
		Command,
		Second,
		Minute,
		Hour,
		Day,
	}

	public interface ISchedulerAction
	{
		int Id { get; }
		string Name { get; }
		SchedulerActionSynchronizationMode SynchronizationMode { get; }
		SchedulerActionIntervalMode IntervalMode { get; }
		int Interval { get; }
		TimeSpan? IntervalOffset { get; }

		Task Execute();
	}
}
