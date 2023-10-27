using System;
using System.Threading.Tasks;

namespace EllipticBit.Services.Scheduler
{
	public interface ISchedulerSynchronizationContext
	{
		Task<bool> Acquire(ISchedulerAction action);
		Task Completed(ISchedulerAction action, DateTimeOffset startedAt, Exception ex = null);
	}
}
