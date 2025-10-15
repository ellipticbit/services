//-----------------------------------------------------------------------------
// Copyright (c) 2020-2024 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using System;
using Azure.Storage.Blobs;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs.Models;
using System.Linq;

namespace EllipticBit.Services.Storage
{
	public class AzureBlobStorageService : IRemoteStorageService
	{
		private readonly BlobServiceClient _blobClient;
		public AzureBlobStorageService(BlobServiceClient blobClient) {
			this._blobClient = blobClient;
		}

		public async Task<Stream> Download(string path) {
			var srcparts = path.Split(new[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries);
			if (srcparts.Length < 2) throw new ArgumentException("No blob container specified. Use the format <container>\\<blob name>.", nameof(path));

			var blob = this._blobClient.GetBlobContainerClient(srcparts[0]).GetBlobClient(string.Join("\\", srcparts.Skip(1)));
			if (blob == null) {
				return null;
			}

			var b = (await blob.DownloadAsync())?.Value;
			if (b == null) return null;
			var ms = new MemoryStream();
			await b.Content.CopyToAsync(ms);
			return ms;
		}

		public async Task Upload(string path, Stream data, bool overwrite = false) {
			var srcparts = path.Split(new[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries);
			if (srcparts.Length < 2) throw new ArgumentException("No blob container specified. Use the format <container>\\<blob name>.", nameof(path));

			var container = this._blobClient.GetBlobContainerClient(srcparts[0]);
			var blob = container.GetBlobClient(string.Join("\\", srcparts.Skip(1)));
			if (!overwrite && await blob.ExistsAsync()) {
				throw new IOException($"The specified target file already exists: {blob.Name}");
			}
			if (overwrite) {
				await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
			}

			await container.UploadBlobAsync(string.Join("\\", srcparts.Skip(1)), data);
		}

		public Task Delete(string path) {
			var srcparts = path.Split(new[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries);
			if (srcparts.Length < 2) throw new ArgumentException("No blob container specified. Use the format <container>\\<blob name>.", nameof(path));

			return this._blobClient.GetBlobContainerClient(srcparts[0]).DeleteBlobIfExistsAsync(string.Join("\\", srcparts.Skip(1)));
		}

		public async Task<bool> Exists(string path) {
			var srcparts = path.Split(new[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries);
			if (srcparts.Length < 2) throw new ArgumentException("No blob container specified. Use the format <container>\\<blob name>.", nameof(path));

			var blob = this._blobClient.GetBlobContainerClient(srcparts[0]).GetBlobClient(string.Join("\\", srcparts.Skip(1)));
			var result = await blob.ExistsAsync();
			return result.Value;
		}

		public async Task<bool> Move(string sourcePath, string targetPath, bool overwrite = false) {
			var srcparts = sourcePath.Split(new[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries);
			var tgtparts = targetPath.Split(new[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries);
			if (srcparts.Length < 2) throw new ArgumentException("No blob container specified. Use the format <container>\\<blob name>.", nameof(sourcePath));
			if (tgtparts.Length < 2) throw new ArgumentException("No blob container specified. Use the format <container>\\<blob name>.", nameof(targetPath));

			var src = this._blobClient.GetBlobContainerClient(srcparts[0]).GetBlobClient(string.Join("\\", srcparts.Skip(1)));
			var exists = await src.ExistsAsync();
			if (!exists) return false;

			var tgt = this._blobClient.GetBlobContainerClient(tgtparts[0]).GetBlobClient(string.Join("\\", tgtparts.Skip(1)));
			if (!overwrite && await tgt.ExistsAsync()) {
				throw new IOException($"The specified target file already exists: {tgt.Name}");
			}
			if (overwrite) {
				await tgt.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
			}

			var copy = await tgt.StartCopyFromUriAsync(src.Uri);
			await copy.WaitForCompletionAsync();

			await src.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);

			return true;
		}

		public async Task<bool> Copy(string sourcePath, string targetPath, bool overwrite = false) {
			var srcparts = sourcePath.Split(new[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries);
			var tgtparts = targetPath.Split(new[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries);
			if (srcparts.Length < 2) throw new ArgumentException("No blob container specified. Use the format <container>\\<blob name>.", nameof(sourcePath));
			if (tgtparts.Length < 2) throw new ArgumentException("No blob container specified. Use the format <container>\\<blob name>.", nameof(targetPath));

			var src = this._blobClient.GetBlobContainerClient(srcparts[0]).GetBlobClient(string.Join("\\", srcparts.Skip(1)));
			var exists = await src.ExistsAsync();
			if (!exists) return false;

			var tgt = this._blobClient.GetBlobContainerClient(tgtparts[0]).GetBlobClient(string.Join("\\", tgtparts.Skip(1)));
			if (!overwrite && await tgt.ExistsAsync()) {
				throw new IOException($"The specified target file already exists: {tgt.Name}");
			}
			if (overwrite) {
				await tgt.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
			}

			var copy = await tgt.StartCopyFromUriAsync(src.Uri);
			await copy.WaitForCompletionAsync();

			return true;
		}
	}
}
