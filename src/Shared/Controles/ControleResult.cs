namespace Svk.Shared.Controles;

public static class ControleResult
{
    public class Create
    {
        public int Id { get; set; }
        public IEnumerable<string>? SignedUrls { get; set; }
    }

    public class Edit
    {
        public int Id { get; set; }
        public IEnumerable<string>? SignedUrls{ get; set; }
    }
}