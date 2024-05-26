using Svk.Domain.Common;

namespace Svk.Domain.Files;

public class Image : Entity
{
    public Image(string bucket, string fileName)
    {
        Bucket = bucket;
        Extension = ".jpg";
        FileName = fileName;
    }

    public Image()
    {
    }

    public string Bucket { get; set; }
    public string FileName { get; set; }
    public string Extension { get; set; }

    public string FilePath => $"{FileName}{Extension}";
}