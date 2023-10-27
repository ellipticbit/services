using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace EllipticBit.Services.Database
{
	internal class DatabaseServiceFactory<TDefaultDetector, TDefaultResolver> : IDatabaseServiceFactory, IDatabaseServiceBuilder
		where TDefaultDetector : class, IDatabaseConflictDetection
		where TDefaultResolver : class, IDatabaseConflictResolver
	{
		private static ImmutableDictionary<string, IDatabaseServiceOptions> options = ImmutableDictionary<string, IDatabaseServiceOptions>.Empty;

		private readonly IEnumerable<IDatabaseConflictDetection> detectors;
		private readonly IEnumerable<IDatabaseConflictResolver> resolvers;

		public DatabaseServiceFactory(IEnumerable<IDatabaseConflictDetection> detectors, IEnumerable<IDatabaseConflictResolver> resolvers) {
			this.detectors = detectors;
			this.resolvers = resolvers;
		}

		public TDatabase Create<TDatabase>(string name) where TDatabase : class, IDatabaseService<TDatabase>, new()
		{
			if (!options.TryGetValue(name, out IDatabaseServiceOptions opts)) throw new IndexOutOfRangeException($"Unable to locate the specified database service: {name}");

			var detector = detectors.OfType<TDefaultDetector>().First();
			var resolver = resolvers.OfType<TDefaultResolver>().First();
			var dbc = new TDatabase();

			return dbc.GetDatabase(opts, detector, resolver);
		}

		public TDatabase Create<TDatabase, TDetector, TResolver>(string name) where TDatabase : class, IDatabaseService<TDatabase>, new() where TDetector : class, IDatabaseConflictDetection where TResolver : class, IDatabaseConflictResolver
		{
			if (!options.TryGetValue(name, out IDatabaseServiceOptions opts)) throw new IndexOutOfRangeException($"Unable to locate the specified database service: {name}");

			var detector = detectors.OfType<TDetector>().First();
			var resolver = resolvers.OfType<TResolver>().First();
			var dbc = new TDatabase();

			return dbc.GetDatabase(opts, detector, resolver);
		}

		//
		// IDatabaseServiceBuilder Implementation
		//

		private readonly IServiceCollection services;

		public DatabaseServiceFactory(IServiceCollection services) {
			this.services = services;
		}

		public IDatabaseServiceBuilder AddDatabaseService(string name, IDatabaseServiceOptions opts) {
			options = options.Add(name, opts);
			return this;
		}

		public IDatabaseServiceBuilder AddDatabaseConflictDetector<T>() where T : class, IDatabaseConflictDetection {
			this.services.AddTransient<IDatabaseConflictDetection, T>();
			return this;
		}

		public IDatabaseServiceBuilder AddDatabaseConflictResolver<T>() where T : class, IDatabaseConflictResolver {
			this.services.AddTransient<IDatabaseConflictResolver, T>();
			return this;
		}
	}
}
