using FluentValidation;

namespace Svk.Shared.Controles;

public static class ControleDto
{
    public class Index
    {
        public int Id { get; set; }
        public IEnumerable<LaadBonDto.Index>? Laadbonnen { get; set; }
        public string? Transporteur { get; set; }
        public IEnumerable<string>? Routenummers { get; set; }

        public string? Nummerplaat { get; set; }

        //TODO ask if needs to be nullable
        public DateTime Datum { get; set; }
    }

    public class Detail
    {
        public int Id { get; set; }
        public IEnumerable<int>? Laadbonnummers { get; set; }
        public string? Transporteur { get; set; }
        public IEnumerable<string>? Routenummers { get; set; }
        public string? Nummerplaat { get; set; }
        public DateTime Datum { get; set; }
        public IEnumerable<string>? Fotos { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
        //TODO ask what it is
        //public Logging? ActionLog { get; set; }
    }

    public class Create
    {
        public IEnumerable<LaadBonDto.Index>? Laadbonnummers { get; set; }
        public string? Transporteur { get; set; }
        public IEnumerable<string>? Routenummers { get; set; }
        public string? Nummerplaat { get; set; }
        public DateTime? Datum { get; set; }
        public int Fotos { get; set; }

        public string? auth0id { get; set; }


        public class Validator : AbstractValidator<Create>
        {
            public Validator()
            {
                RuleFor(x => x.Datum).NotEmpty();
                RuleFor(x => x.Fotos).NotNull();
                RuleFor(x => x.Laadbonnummers).NotEmpty();
                RuleFor(x => x.Nummerplaat).NotEmpty();
                RuleFor(x => x.Routenummers).NotEmpty();
                RuleFor(x => x.Transporteur).NotEmpty();
            }
        }
    }

    public class Edit
    {
        public int Fotos { get; set; }
    }
}