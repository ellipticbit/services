//-----------------------------------------------------------------------------
// Copyright (c) 2020 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace EllipticBit.Services.Storage {
	public class LocalFileStorageService : ILocalStorageService {
		private readonly string rootPath;

		public LocalFileStorageService(IConfiguration configuration) {
			this.rootPath = configuration.GetSection("Storage").GetSection("File").GetSection("RootPath").Value;
		}

		public Task Delete(string path, string fileName) {
			var cp = Path.Combine(rootPath, path, fileName);
			if (File.Exists(cp)) {
				File.Delete(cp);
			}

			return Task.CompletedTask;
		}

		public Task<Stream> Read(string path, string fileName) {
			var cp = Path.Combine(rootPath, path, fileName);

			if (!File.Exists(cp)) {
				throw new FileNotFoundException("Unable to locate specified file.", cp);
			}

			return Task.FromResult((Stream)File.OpenRead(cp));
		}

		public Task<bool> Exists(string path, string fileName) {
			var cp = Path.Combine(rootPath, path, fileName);
			return Task.FromResult(File.Exists(cp));
		}

		public Task<bool> Move(string sourcePath, string sourceName, string targetPath, string targetName = null) {
			var dir = Path.Combine(rootPath, sourcePath);
			var tgtdir = Path.Combine(rootPath, targetPath);
			File.Move(Path.Combine(dir, sourceName), Path.Combine(tgtdir, targetName ?? sourceName));
			return Task.FromResult(true);
		}

		public Task<bool> Copy(string sourcePath, string sourceName, string targetPath, string targetName = null) {
			File.Copy(Path.Combine(sourcePath, sourceName), Path.Combine(targetPath, targetName ?? sourceName));
			return Task.FromResult(true);
		}

		public async Task Write(string path, string fileName, Stream data) {
			var dir = Path.Combine(rootPath, path);
			var cp = Path.Combine(rootPath, path, fileName);

			if (!Directory.Exists(dir)) {
				Directory.CreateDirectory(dir);
			}

			using (var fs = File.OpenWrite(cp)) {
				await data.CopyToAsync(fs);
			}
		}
	}
}
