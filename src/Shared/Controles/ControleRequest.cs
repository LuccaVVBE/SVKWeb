using Svk.Shared.Misc;

namespace Svk.Shared.Controles;

public static class ControleRequest
{
    public class Index
    {
        public Pagination.Index Pagination { get; set; } = new();
        public int? Routenummer { get; set; }

        public string? Lader { get; set; }
        public int? Laadbonnummer { get; set; }
        public string? Transporteur { get; set; }
        public string? Nummerplaat { get; set; }
        public DateTime? StartDateRange { get; set; }
        public DateTime? EndDateRange { get; set; }

        //Sorting make sure to use the same name as the property you want to sort on
        public sortableFields? SortBy { get; set; }
        public bool SortDescending { get; set; } = false;

        public int Page { get; set; } = 0;
        public int PageSize { get; set; } = 10;
    }
}