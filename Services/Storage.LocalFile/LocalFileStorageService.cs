//-----------------------------------------------------------------------------
// Copyright (c) 2020-2024 EllipticBit, LLC All Rights Reserved.
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

		public Task Delete(string path) {
			var cp = Path.Combine(rootPath, path);
			if (File.Exists(cp)) {
				File.Delete(cp);
			}

			return Task.CompletedTask;
		}

		public Task<Stream> Read(string path) {
			var cp = Path.Combine(rootPath, path);

			if (!File.Exists(cp)) {
				throw new FileNotFoundException("Unable to locate specified file.", cp);
			}

			return Task.FromResult((Stream)File.OpenRead(cp));
		}

		public async Task Write(string path, Stream data) {
			var cp = Path.Combine(rootPath, path);
			var dir = Path.GetDirectoryName(cp);

			if (!Directory.Exists(dir)) {
				Directory.CreateDirectory(dir);
			}

			using (var fs = File.OpenWrite(cp)) {
				await data.CopyToAsync(fs);
			}
		}

		public Task<bool> Exists(string path) {
			var cp = Path.Combine(rootPath, path);
			return Task.FromResult(File.Exists(cp));
		}

		public Task<bool> Move(string sourcePath, string targetPath) {
			var src = Path.Combine(rootPath, sourcePath);
			var tgt = Path.Combine(rootPath, targetPath);
			File.Move(src, tgt);
			return Task.FromResult(true);
		}

		public Task<bool> Copy(string sourcePath, string targetPath) {
			var src = Path.Combine(rootPath, sourcePath);
			var tgt = Path.Combine(rootPath, targetPath);
			File.Copy(src, tgt);
			return Task.FromResult(true);
		}
	}
}
