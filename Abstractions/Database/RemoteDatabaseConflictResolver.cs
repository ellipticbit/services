using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EllipticBit.Services.Database
{
	public sealed class RemoteDatabaseConflictResolver : IDatabaseConflictResolver
	{
		public Task<IEnumerable<DataConflictResolution>> Resolve(IEnumerable<DataConflict> conflicts)
		{
			var rl = conflicts.Select(c => c.Resolve(c.Remote)).ToList();
			return Task.FromResult(rl.AsEnumerable());
		}
	}
}