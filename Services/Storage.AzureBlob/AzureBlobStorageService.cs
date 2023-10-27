//-----------------------------------------------------------------------------
// Copyright (c) 2020-2023 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using Azure.Storage.Blobs;
using System.IO;
using System.Threading.Tasks;

namespace EllipticBit.Services.Storage
{
	public class AzureBlobStorageService : IRemoteStorageService
	{
		private readonly BlobServiceClient _blobClient;
		public AzureBlobStorageService(BlobServiceClient blobClient) {
			this._blobClient = blobClient;
		}

		public async Task<Stream> Download(string path, string fileName) {
			var blob = this._blobClient.GetBlobContainerClient(path).GetBlobClient(fileName);
			if (blob == null) {
				return null;
			}

			var b = (await blob.DownloadAsync())?.Value;
			if (b == null) return null;
			var ms = new MemoryStream();
			await b.Content.CopyToAsync(ms);
			return ms;
		}

		public Task Upload(string path, string fileName, Stream data) {
			return this._blobClient.GetBlobContainerClient(path).UploadBlobAsync(fileName, data);
		}

		public Task Delete(string path, string fileName) {
			return this._blobClient.GetBlobContainerClient(path).DeleteBlobIfExistsAsync(fileName);
		}

		public async Task<bool> Exists(string path, string fileName) {
			var blob = this._blobClient.GetBlobContainerClient(path).GetBlobClient(fileName);
			var result = await blob.ExistsAsync();
			return result.Value;
		}

		public async Task<bool> Move (string sourcePath, string sourceName, string targetPath, string targetName = null) {
			var src = this._blobClient.GetBlobContainerClient(sourcePath).GetBlobClient(sourceName);
			var exists = await src.ExistsAsync();
			if (!exists) return false;

			var tgt = this._blobClient.GetBlobContainerClient(targetPath).GetBlobClient(targetName ?? sourceName);
			var copy = await tgt.StartCopyFromUriAsync(src.Uri);
			await copy.WaitForCompletionAsync();

			return true;
		}
	}
}
