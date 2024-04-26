//-----------------------------------------------------------------------------
// Copyright (c) 2020-2024 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using System.IO;
using System.Threading.Tasks;

namespace EllipticBit.Services.Storage {
	public interface IRemoteStorageService {
		Task<bool> Exists(string path);
		Task<Stream> Download(string path);
		Task Upload(string path, Stream data);
		Task Delete(string path);
		Task<bool> Move(string sourcePath, string targetPath);
		Task<bool> Copy(string sourcePath, string targetPath);
	}
}
