namespace Svk.Shared.Misc;

public static class Result
{
    public class GetItemsPaginated<T>
    {
        public IEnumerable<T> Items { get; set; } = default!;
        public int TotalItems { get; set; }
    }
}