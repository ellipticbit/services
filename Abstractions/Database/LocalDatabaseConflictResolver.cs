using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EllipticBit.Services.Database
{
	public sealed class LocalDatabaseConflictResolver : IDatabaseConflictResolver
	{
		public Task<IEnumerable<DataConflictResolution>> Resolve(IEnumerable<DataConflict> conflicts) {
			var rl = conflicts.Select(c => c.Resolve(c.Local)).ToList();
			return Task.FromResult(rl.AsEnumerable());
		}
	}
}
