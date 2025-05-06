//-----------------------------------------------------------------------------
// Copyright (c) 2023-2025 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using System.Threading.Tasks;

namespace EllipticBit.Services.Scheduler
{
	/// <summary>
	/// Defines the interface the Scheduler Service.
	/// </summary>
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
		/// <typeparam name="TAction">The Action implementation class type.</typeparam>
		void Enable<TAction>()
			where TAction : class, ISchedulerAction;

		/// <summary>
		/// Marks the action as ineligible for execution.
		/// </summary>
		/// <typeparam name="TAction">The Action implementation class type.</typeparam>
		void Disable<TAction>()
			where TAction : class, ISchedulerAction;

		/// <summary>
		/// Manually execute the specified action.
		/// </summary>
		/// <typeparam name="TAction">The Action implementation class type.</typeparam>
		Task Execute<TAction>()
			where TAction : class, ISchedulerAction;
	}
}
