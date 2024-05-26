using FluentValidation;

namespace Svk.Shared.Controles;

public class AddresDto
{
    public string Street { get; set; }
    public string Postcode { get; set; }
    public string HouseNr { get; set; }
    public string CountryIsoCode { get; set; }

    public class Validator : AbstractValidator<AddresDto>
    {
        public Validator()
        {
            RuleFor(x => x.Street).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Postcode).NotEmpty().MaximumLength(10);
            RuleFor(x => x.HouseNr).NotEmpty().MaximumLength(10);
            RuleFor(x => x.CountryIsoCode).NotEmpty().MaximumLength(2);
        }
    }
}