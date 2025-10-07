////////////// Broward County Application Services Group //////////////////////
///////////////////////////////////////////////////////////////////////////////
///// Ver:

///// Project: BQPT
///////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Bqpt.Common;
using Newtonsoft.Json;
using RestSharp;

namespace Bqpt.Infrastructure
{
    public class FilesConnectedServices : IFilesConnectedServices
    {
        private readonly string _filesApiBaseurl = AppConstants.FilesApiUrl;
        private readonly string _tenantId = AppConstants.TenantId;
        private readonly string _clientId = AppConstants.EIMSClientId;
        private readonly string _clientSecret = AppConstants.EIMSClientSecret;

        public async Task<bool> CleanAllFilesInFolder(CleanFolderDto payload, CancellationToken cancellationToken)
        {
            var token = await GetToken(cancellationToken);

            if (string.IsNullOrEmpty(token))
                return false;

            var client = new RestClient(_filesApiBaseurl);
            var request = new RestRequest("blobs", Method.DELETE, DataFormat.Json);

            request.AddJsonBody(payload);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", $"Bearer {token}");

            var response = await client.ExecuteAsync(request, cancellationToken);

            return response.IsSuccessful;
        }

        /// <summary>
        /// Disable a file in Blob Services
        /// </summary>
        /// <param name="domainKey"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> DeleteFile(string domainKey, CancellationToken cancellationToken)
        {
            var token = await GetToken(cancellationToken);
            /*
                        if (string.IsNullOrEmpty(token))
                            return null;
                        */
            if (string.IsNullOrEmpty(token))
            {
                throw new Exception("Token retrieval failed.");
            }

            var client = new RestClient(_filesApiBaseurl);
            var request = new RestRequest($"blobs/{domainKey}", Method.DELETE, DataFormat.Json);

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", $"Bearer {token}");

            var response = await client.ExecuteAsync(request, cancellationToken);
            if (!response.IsSuccessful)
            {
                throw new Exception($"DeleteFile failed: {response.StatusCode} - {response.Content}");
            }

            return response.IsSuccessful;
        }

        /// <summary>
        /// Produce a request to fullfil files validation, by checking status on Api
        /// </summary>
        /// <param name="payload"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IEnumerable<BlobEnvelop>> FilesValidation(IEnumerable<FileValidationDto> payload, CancellationToken cancellationToken)
        {
            var token = await GetToken(cancellationToken);

            if (string.IsNullOrEmpty(token))
                return null;

            var client = new RestClient(_filesApiBaseurl);
            var request = new RestRequest("blobs/validations", Method.POST, DataFormat.Json);

            request.AddJsonBody(payload);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", $"Bearer {token}");

            var response = await client.ExecuteAsync(request, cancellationToken);

            return response.IsSuccessful
                            ? JsonConvert.DeserializeObject<IEnumerable<BlobEnvelop>>(response.Content)
                            : Enumerable.Empty<BlobEnvelop>();
        }

        /// <summary>
        /// Get File From Api
        /// </summary>
        /// <param name="domainKey"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<BlobFileEnvelop> GetFile(string domainKey, CancellationToken cancellationToken = default)
        {
            var token = await GetToken(cancellationToken);

            if (string.IsNullOrEmpty(token))
                return null;

            var client = new RestClient(_filesApiBaseurl);
            var request = new RestRequest($"blobs/{domainKey}", Method.GET);

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", $"Bearer {token}");

            var response = await client.ExecuteAsync(request, cancellationToken);

            return response.IsSuccessful
                            ? JsonConvert.DeserializeObject<BlobFileEnvelop>(response.Content)
                            : null;
        }

        /// <summary>
        /// Will produce a request to save/post a file to Api
        /// </summary>
        /// <param name="file"></param>
        /// <param name="folderName"></param>
        /// <param name="fileName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<BlobEnvelop> SaveFile(HttpPostedFileBase file, string folderName, string fileName, CancellationToken cancellationToken)
        {
            var token = await GetToken(cancellationToken);

            if (string.IsNullOrEmpty(token))
                return null;

            byte[] data;

            using (var reader = new BinaryReader(file.InputStream))
            {
                data = reader.ReadBytes((int)file.InputStream.Length);
            }

            var stream = new MemoryStream(data);
            var client = new RestClient(_filesApiBaseurl);

            client.ClearHandlers();

            var request = new RestRequest("blobs", Method.POST, DataFormat.Json);

            request.AddHeader("Accept", "application/json");
            request.Parameters.Clear();
            request.AddHeader("Content-Type", "multipart/form-data");
            request.AddHeader("Authorization", $"Bearer {token}");

            request.Files.Add(new FileParameter
            {
                Name = "File",
                Writer =
                        (s) =>
                        {
                            stream.CopyTo(s);
                        },
                FileName = fileName,
                ContentLength = stream.Length
            });

            request.AddParameter("FileName", fileName, ParameterType.GetOrPost);
            request.AddParameter("Folder", folderName, ParameterType.GetOrPost);
            request.AddParameter("TenantId", _tenantId, ParameterType.GetOrPost);

            var response = await client.ExecuteAsync(request);

            return response.IsSuccessful ? JsonConvert.DeserializeObject<BlobEnvelop>(response.Content) : null;
        }

        /// <summary>
        /// Create a Random File Name to be used in Api File Submission
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string SetFileName(string fileName) => $"bcblob-{Guid.NewGuid()}{Path.GetExtension(fileName)}".ToLowerInvariant();

        /// <summary>
        /// Private Helper to authenticate requests
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<string> GetToken(CancellationToken cancellationToken)
        {
            var client = new RestClient(_filesApiBaseurl);
            var request = new RestRequest("token", Method.POST, DataFormat.Json);

            request.AddJsonBody(new Credentials { ClientId = _clientId, ClientSecret = _clientSecret, TenantId = _tenantId });
            request.AddHeader("Content-Type", "application/json");

            var response = await client.ExecuteAsync(request, cancellationToken);
            return response.IsSuccessful ? JsonConvert.DeserializeObject<string>(response.Content) : string.Empty;
        }
    }
}