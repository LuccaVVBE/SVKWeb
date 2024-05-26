using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;

namespace Svk.Fakers.Images;

public class ImageGen
{
    private readonly IMinioClient minio;

    public ImageGen(IMinioClient minio)
    {
        this.minio = minio;
        for (int i = 1; i <= 21; i++)
        {
            Run("seed", i.ToString());
        }
    }

    private async void Run(string bucket, string fileName)
    {
        try
        {
            BucketExistsArgs bucketExistsArgs = new BucketExistsArgs()
                .WithBucket(bucket);
            bool found = await minio.BucketExistsAsync(bucketExistsArgs);
            if (!found)
            {
                MakeBucketArgs makeBucketArgs = new MakeBucketArgs()
                    .WithBucket(bucket);
                await minio.MakeBucketAsync(makeBucketArgs);
            }

            PutObjectArgs putObjectArgs = new PutObjectArgs()
                .WithBucket(bucket)
                .WithObject($"./{fileName}.jpg")
                .WithFileName($"../Svk.Fakers/Images/{fileName}.jpg")
                .WithContentType("application/octet-stream");
            await minio.PutObjectAsync(putObjectArgs);
            Console.WriteLine($"{fileName}.jpg is uploaded successfully to {bucket}");
        }
        catch (MinioException e)
        {
            Console.WriteLine("Error occurred: " + e);
        }
    }
}