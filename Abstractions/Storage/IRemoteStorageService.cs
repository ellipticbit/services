//-----------------------------------------------------------------------------
// Copyright (c) 2020 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using System.IO;
using System.Threading.Tasks;

namespace EllipticBit.Services.Storage {
	public interface IRemoteStorageService {
		Task<bool> Exists(string path, string fileName);
		Task<Stream> Download(string path, string fileName);
		Task Upload(string path, string fileName, Stream data);
		Task Delete(string path, string fileName);
		Task<bool> Move(string sourcePath, string sourceName, string targetPath, string targetName = null);
	}
}
