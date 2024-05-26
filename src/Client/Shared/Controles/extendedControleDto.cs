namespace Svk.Client.Shared.Controles;

class extendedControleDto
{
    public string? RouteNummersString { get; set; }

    public IEnumerable<int>? Laadbonnummers { get; set; }

    public string? Transporteur { get; set; }

    public string? Nummerplaat { get; set; }

    public DateTime Datum { get; set; }

    public int Id { get; set; }
}