﻿using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using WebApiProxy.Models;
using static HashLib.Utility.Cryptography;

namespace WebApiProxy.Utility
{
    public class HashFile : IHashFile
    {
        private readonly IOptions<HashOptions> _hashOptions;
        private readonly IDistributedCache _distributedCache;
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<HashFile> _logger;

        public HashFile(IOptions<HashOptions> hashOptions, IDistributedCache distributedCache, IHttpClientFactory clientFactory, ILogger<HashFile> logger)
        {
            _hashOptions = hashOptions;
            _distributedCache = distributedCache;
            _clientFactory = clientFactory;
            _logger = logger;
        }

        public async Task<string> GetHashFileBase64Async(string fileUri)
        {   
            var cacheFile = await _distributedCache.GetStringAsync(fileUri);
            var hashBase64 = "";

            if (cacheFile == null)
            {
                _logger.LogInformation($"GetHashFileBase64Async cacheFile is null, fileUri:{fileUri}");
                _logger.LogInformation($"request _hashOptions.Value.ApiEndPoint:{_hashOptions.Value.ApiEndPoint}");

                var request = new HttpRequestMessage(HttpMethod.Get, $"{_hashOptions.Value.ApiEndPoint}{fileUri}");
                var client = _clientFactory.CreateClient();
                var response = await client.SendAsync(request);

                _logger.LogInformation($"response.IsSuccessStatusCode:{response.IsSuccessStatusCode}; fileUri:{fileUri}");
                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    hashBase64 = responseString;
                }
               
                await _distributedCache.SetStringAsync(fileUri, hashBase64, new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(300) });
            }
            else
            {
                hashBase64 = cacheFile;
            }

            return hashBase64;
        }

        public async Task<string> SimulatorHashFileBase64Async(string fileUri)
        {
            _logger.LogInformation($"SimulatorHashFileBase64Async fileUri:{fileUri}");
            var fileByte = await FileUriToByteAsync(fileUri);
            var fileBase64 = GetFileBase64(fileByte);
            var hashBase64 = EncryptUtil.EncryptAES(fileBase64, _hashOptions.Value.AESKey, _hashOptions.Value.AES_IV);

            _logger.LogInformation($"SimulatorHashFileBase64Async Done fileUri:{fileUri}");
            return hashBase64;
        }

        private static string GetFileBase64(byte[] fileByte)
        {
            var result = string.Empty;

            if (fileByte != null && fileByte.Length > 0)
            {
                result = Convert.ToBase64String(fileByte);
            }

            return result;
        }

        private static async Task<byte[]> FileUriToByteAsync(string fileUri)
        {
            using (var client = new HttpClient())
            {
                using (var result = await client.GetAsync(fileUri))
                {
                    if (result.IsSuccessStatusCode)
                    {
                        return await result.Content.ReadAsByteArrayAsync();
                    }
                }
            }

            return null;
        }

    }
}