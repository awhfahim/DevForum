using System.Net;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using Microsoft.AspNetCore.Http;
using Serilog;
using StackOverflow.Application.Contracts.Features.AwsManagementServices;

namespace StackOverflow.Application.Features.AwsManagementServices;

public class ImageManagementService : IImageManagementService
{
    private readonly IAmazonS3 _s3Client;
    public ImageManagementService(IAmazonS3 s3Client)
    {
          _s3Client = s3Client;   
    }
    
    public async Task<HttpStatusCode> UploadImageAsync(IFormFile imageFile, string bucketName, string key)
    {
        try
        {
            var bucketExists = await AmazonS3Util.DoesS3BucketExistV2Async(_s3Client, bucketName);
            if (!bucketExists)
            {
                throw new Exception($"Bucket {bucketName} does not exist");
            }

            if (imageFile == null || imageFile.Length == 0)
            {
                throw new ArgumentNullException(nameof(imageFile));
            }

            var uploadRequest = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = key,
                InputStream = imageFile.OpenReadStream(),
                ContentType = imageFile.ContentType
            };

            var response = await _s3Client.PutObjectAsync(uploadRequest);

            return response.HttpStatusCode;
        }
        catch (AmazonS3Exception e)
        {
            Log.Error($"Error uploading image to S3: {e.Message}");
            throw new Exception($"Error uploading image to S3: {e.Message}");
        }  
        catch (Exception e)
        {
            Log.Error($"Error uploading image to S3: {e.Message}");
            throw new Exception($"Error uploading image to S3: {e.Message}");
        }
    }
    
    public async Task<(bool Success, string Url, string ErrorMessage)> 
        GetPresignedUrlAsync(string bucketName, string key, DateTime expiration)
    {
        try
        {
            var bucketExists = await AmazonS3Util.DoesS3BucketExistV2Async(_s3Client, bucketName);
            if (!bucketExists)
            {
                return (false, null, $"Bucket {bucketName} does not exist")!;
            }

            var presignedUrlRequest = new GetPreSignedUrlRequest()
            {
                BucketName = bucketName,
                Key = key,
                Expires = expiration,
                Verb = HttpVerb.GET
            };

            var url = await _s3Client.GetPreSignedURLAsync(presignedUrlRequest);
            return (true, url, null)!;
        }
        catch (AmazonS3Exception ex)
        {
            Log.Error($"Error generating presigned URL: {ex.Message}");
            return (false, null, ex.Message)!;
        }
        catch (Exception ex)
        {
            Log.Error($"Unexpected error generating presigned URL: {ex.Message}");
            return (false, null, ex.Message)!;
        }
    }
    
    public async Task CreateBucketIfNotExistsAsync(string bucketName)
    {
        try
        {
            var bucketExists = await AmazonS3Util.DoesS3BucketExistV2Async(_s3Client, bucketName);
            if (!bucketExists)
            {
                var putBucketRequest = new PutBucketRequest
                {
                    BucketName = bucketName,
                    UseClientRegion = true
                };

                var response = await _s3Client.PutBucketAsync(putBucketRequest);
                if (response.HttpStatusCode != HttpStatusCode.OK)
                {
                    throw new Exception($"Failed to create bucket {bucketName}");
                }
            }
        }
        catch (AmazonS3Exception e)
        {
            Log.Error($"Error creating bucket: {e.Message}");
            throw new Exception($"Error creating bucket: {e.Message}");
        }
        catch (Exception e)
        {
            Log.Error($"Unexpected error creating bucket: {e.Message}");
            throw new Exception($"Unexpected error creating bucket: {e.Message}");
        }
    }
}