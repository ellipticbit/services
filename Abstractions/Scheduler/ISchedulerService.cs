//-----------------------------------------------------------------------------
// Copyright (c) 2023 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using System.Threading.Tasks;

namespace EllipticBit.Services.Scheduler
{
	public interface ISchedulerService
	{
		/// <summary>
		/// Starts the scheduler service.
		/// </summary>
		void Start();
		/// <summary>
		/// Stops the scheduler service.
		/// </summary>
		void Stop();
		/// <summary>
		/// Marks the action as eligible for execution.
		/// </summary>
		void Enable(int actionId);
		/// <summary>
		/// Marks the action as ineligible for execution.
		/// </summary>
		void Disable(int actionId);
		/// <summary>
		/// Manually execute the specified action.
		/// </summary>
		Task Execute(int actionId);
	}
}
