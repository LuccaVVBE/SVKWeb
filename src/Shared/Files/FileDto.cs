namespace Svk.Shared.Files;

public static class FileDto
{
    public class GetSignedUrlResult
    {
        public string Url { get; set; }
    }

    public class GetSignedUploadUrlResult
    {
        public string Url { get; set; }
    }

    public class IndexFileRequest
    {
        public string? Bucket { get; set; }
        public string? FileName { get; set; }
    }
}