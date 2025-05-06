//-----------------------------------------------------------------------------
// Copyright (c) 2023-2025 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

namespace EllipticBit.Services.Scheduler
{
	/// <summary>
	/// Specifies the interface for the Scheduler Service DI Builder.
	/// </summary>
	public interface ISchedulerServiceBuilder
	{
		/// <summary>
		/// Registers an action with the Scheduler.
		/// NOTE: This does not enable the action. The action must be enabled separately.
		/// </summary>
		/// <typeparam name="TAction">The Action implementation class type.</typeparam>
		/// <returns>The scheduler builder instance.</returns>
		ISchedulerServiceBuilder AddAction<TAction>()
			where TAction : class, ISchedulerAction;

		/// <summary>
		/// Registers an action with the Scheduler. Additionally registers a single instance of the options type used by the Action.
		/// NOTE: This does not enable the action. The action must be enabled separately.
		/// </summary>
		/// <param name="options">An instance of the Options type used by this action.</param>
		/// <typeparam name="TAction">The Action implementation class type.</typeparam>
		/// <typeparam name="TOptions">The Options implementation class type.</typeparam>
		/// <returns>The scheduler builder instance.</returns>
		ISchedulerServiceBuilder AddAction<TAction, TOptions>(TOptions options)
			where TAction : class, ISchedulerAction
			where TOptions : class;
	}
}
