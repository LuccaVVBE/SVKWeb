namespace Svk.Shared.Files;

public interface IFileService
{
    /**
     * <summary>
     *   Uploads a file to the specified bucket
     * </summary>
     * <param name="bucket">The bucket to upload the file to</param>
     * <param name="fileName">The name of the file</param>
     * <param name="expiry">The expiry time of the signed url default is 7 days</param>
     */
    Task<string> GetSignedUploadUrlAsync(string bucket, string fileName, int expiry = 60 * 60 * 24 * 7);

    /**
     * <summary>Gets a signed download url for the specified file</summary>
     * <param name="bucket">The bucket to download the file from</param>
     * <param name="fileName">The name of the file</param>
     * <param name="expiry">The expiry time of the signed url default 1 day</param>
     */
    Task<string> GetSignedDownloadUrlAsync(string bucket, string fileName, int expiry = 60 * 60 * 24);

    /**
     * <summary>Creates a bucket with the specified name</summary>
     * <param name="bucket">The name of the bucket to create</param>
     */
    Task<bool> MakeBucketAsync(string bucket);

    /**
     * <summary>Removes the specified bucket</summary>
     * <param name="bucket">The name of the bucket to remove</param>
     */
    Task<bool> RemoveBucketAsync(string bucket);

    /**
     * <summary>Checks if the specified bucket exists</summary>
     * <param name="bucket">The name of the bucket to check</param>
     * <returns>True if the bucket exists, false otherwise</returns>
     */
    Task<bool> BucketExistsAsync(string bucket);
}