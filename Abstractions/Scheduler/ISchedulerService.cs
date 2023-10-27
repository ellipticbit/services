using System.Threading.Tasks;

namespace EllipticBit.Services.Scheduler
{
	public interface ISchedulerService
	{
		void Start();
		void Stop();
		void Enable(int actionId);
		void Disable(int actionId);
		Task Execute(int actionId);
	}
}
