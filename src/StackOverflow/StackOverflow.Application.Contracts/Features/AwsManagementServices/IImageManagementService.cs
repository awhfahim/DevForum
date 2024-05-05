using System.Net;
using Microsoft.AspNetCore.Http;

namespace StackOverflow.Application.Contracts.Features.AwsManagementServices;

public interface IImageManagementService
{
    Task<HttpStatusCode> UploadImageAsync(IFormFile imageFile, string bucketName, string key);

    Task<(bool Success, string Url, string ErrorMessage)>
        GetPresignedUrlAsync(string bucketName, string key, DateTime expiration);

    Task CreateBucketIfNotExistsAsync(string bucketName);
}