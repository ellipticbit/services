namespace EllipticBit.Services.Scheduler
{
	public interface ISchedulerServiceBuilder
	{
		ISchedulerServiceBuilder AddAction<T>() where T : class, ISchedulerAction;
	}
}
