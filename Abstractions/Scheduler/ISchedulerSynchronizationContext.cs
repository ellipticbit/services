//-----------------------------------------------------------------------------
// Copyright (c) 2023-2025 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using System;
using System.Threading.Tasks;

namespace EllipticBit.Services.Scheduler
{
	/// <summary>
	/// Defines the interface used by the Scheduler to synchronize the execution of ISchedulerAction tasks, preventing multiple executions.
	/// </summary>
	public interface ISchedulerSynchronizationContext
	{
		/// <summary>
		/// Acquires a lock to allow this instance to run the Action.
		/// </summary>
		/// <param name="action">The action to acquire a lock on.</param>
		/// <returns>A bool indicating whether or not the lock was successfully acquired.</returns>
		Task<bool> Acquire(ISchedulerAction action);
		/// <summary>
		/// Signals the completion of the Action execution.
		/// </summary>
		/// <param name="action">The action to acquire a lock on.</param>
		/// <param name="startedAt"></param>
		/// <param name="ex">An Exception instance that contains any error information from the execution of the Action.</param>
		/// <returns>A task indicating that the log data was written.</returns>
		Task Completed(ISchedulerAction action, DateTimeOffset startedAt, Exception ex = null);
	}
}
