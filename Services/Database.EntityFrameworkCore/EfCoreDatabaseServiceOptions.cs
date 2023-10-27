using Microsoft.EntityFrameworkCore;

namespace EllipticBit.Services.Database
{
	public sealed class EfCoreDatabaseServiceOptions : IDatabaseServiceOptions
	{
		public DbContextOptions DbContextOptions { get; }

		public EfCoreDatabaseServiceOptions(DbContextOptions dbContextOptions) {
			DbContextOptions = dbContextOptions;
		}
	}
}
