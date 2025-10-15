//-----------------------------------------------------------------------------
// Copyright (c) 2020-2025 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using System.IO;
using System.Threading.Tasks;

namespace EllipticBit.Services.Storage {
	public interface IRemoteStorageService {
		Task<bool> Exists(string path);
		Task<Stream> Download(string path);
		Task Upload(string path, Stream data, bool overwrite = false);
		Task Delete(string path);
		Task<bool> Move(string sourcePath, string targetPath, bool overwrite = false);
		Task<bool> Copy(string sourcePath, string targetPath, bool overwrite = false);
	}
}
