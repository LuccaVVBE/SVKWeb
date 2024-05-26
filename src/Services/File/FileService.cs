using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;
using Svk.Shared.Files;

namespace Svk.Services.File;

public class FileService : IFileService
{
    private readonly IMinioClient minioClient;

    public FileService(IMinioClient minioClient)
    {
        this.minioClient = minioClient;
    }

    public async Task<string> GetSignedUploadUrlAsync(string bucket, string fileName, int expiry = 60 * 60 * 24 * 7)
    {
        string url = "";
        try
        {
            PresignedPutObjectArgs args = new PresignedPutObjectArgs()
                .WithBucket(bucket)
                .WithObject(fileName + ".jpg")
                .WithExpiry(expiry);
            url = await minioClient.PresignedPutObjectAsync(args);
        }
        catch (MinioException e)
        {
            Console.WriteLine("Error occurred: " + e);
        }

        return url;
    }

    public async Task<string> GetSignedDownloadUrlAsync(string bucket, string fileName, int expiry = 60 * 60 * 24)
    {
        string url = "";
        try
        {
            PresignedGetObjectArgs args = new PresignedGetObjectArgs()
                .WithBucket(bucket)
                .WithObject(fileName)
                .WithExpiry(expiry);
            url = await minioClient.PresignedGetObjectAsync(args);
        }
        catch (MinioException e)
        {
            Console.WriteLine("Error occurred: " + e);
        }

        return url;
    }

    public async Task<bool> MakeBucketAsync(string bucket)
    {
        try
        {
            bool found = BucketExistsAsync(bucket).Result;
            if (found)
            {
                Console.WriteLine("mybucket already exists");
                return false;
            }

            MakeBucketArgs args = new MakeBucketArgs()
                .WithBucket(bucket);
            await minioClient.MakeBucketAsync(args);
            Console.WriteLine("mybucket is created successfully");
            return true;
        }
        catch (MinioException e)
        {
            Console.WriteLine("Error occurred: " + e);
            return false;
        }
    }

    public async Task<bool> RemoveBucketAsync(string bucket)
    {
        try
        {
            bool found = BucketExistsAsync(bucket).Result;
            if (found)
            {
                RemoveBucketArgs args = new RemoveBucketArgs()
                    .WithBucket(bucket);
                await minioClient.RemoveBucketAsync(args);
                Console.WriteLine("mybucket is removed successfully");
                return true;
            }

            Console.WriteLine("mybucket does not exist");
            return false;
        }
        catch (MinioException e)
        {
            Console.WriteLine("Error occurred: " + e);
            return false;
        }
    }

    public async Task<bool> BucketExistsAsync(string bucket)
    {
        try
        {
            BucketExistsArgs args = new BucketExistsArgs().WithBucket(bucket);
            bool found = await minioClient.BucketExistsAsync(args);
            Console.WriteLine("bucket-name " + ((found == true) ? "exists" : "does not exist"));
            return found;
        }
        catch (MinioException e)
        {
            Console.WriteLine("[Bucket]  Exception: {0}", e);
            return false;
        }
    }
}