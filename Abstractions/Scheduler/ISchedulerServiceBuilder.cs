//-----------------------------------------------------------------------------
// Copyright (c) 2023 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

namespace EllipticBit.Services.Scheduler
{
	public interface ISchedulerServiceBuilder
	{
		/// <summary>
		/// Registers an action with the Scheduler. NOTE: This does not enable the action. The action must be enabled separately.
		/// </summary>
		/// <typeparam name="T">The Action implementation class type</typeparam>
		/// <returns>The scheduler builder object</returns>
		ISchedulerServiceBuilder AddAction<T>() where T : class, ISchedulerAction;
	}
}
