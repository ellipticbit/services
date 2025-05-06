//-----------------------------------------------------------------------------
// Copyright (c) 2023-2025 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using System;
using System.Threading.Tasks;

namespace EllipticBit.Services.Scheduler
{
	/// <summary>
	/// Specifies the synchronization mode used by an ISchedulerAction.
	/// </summary>
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

	/// <summary>
	/// Specifies the interval on which an ISchedulerAction is executed.
	/// </summary>
	public enum SchedulerActionIntervalMode
	{
		/// <summary>
		/// Action will not execute unless requested manually.
		/// </summary>
		Manual,
		/// <summary>
		/// Interval is expressed in seconds.
		/// </summary>
		Second,
		/// <summary>
		/// Interval is expressed in minutes.
		/// </summary>
		Minute,
		/// <summary>
		/// Interval is expressed in hours.
		/// </summary>
		Hour,
		/// <summary>
		/// Interval is expressed in days.
		/// </summary>
		Day,
	}

	/// <summary>
	/// Required implementation interface for all Actions.
	/// </summary>
	public interface ISchedulerAction
	{
		/// <summary>
		/// ID used by the service to identify executions.
		/// </summary>
		int Id { get; }
		/// <summary>
		/// Friendly name used by the scheduler service to identify executions.
		/// </summary>
		string Name { get; }
		/// <summary>
		/// The synchronization level required by this Action.
		/// </summary>
		SchedulerActionSynchronizationMode SynchronizationMode { get; }
		/// <summary>
		/// The synchronization method used by this action.
		/// </summary>
		SchedulerActionIntervalMode IntervalMode { get; }
		/// <summary>
		/// The interval between executions. Executions may overlap if the prior execution has not completed when the next execution begins.
		/// </summary>
		int Interval { get; }
		/// <summary>
		/// Delay past the interval ordinal.
		/// </summary>
		TimeSpan? IntervalOffset { get; }
		/// <summary>
		/// Run the Action immediately after starting the scheduler, then execute as scheduled.
		/// </summary>
		bool ExecuteOnStart { get; }

		/// <summary>
		/// Executes the Action manually.
		/// </summary>
		/// <returns>A Task representing the running Action.</returns>
		Task Execute();
	}
}
