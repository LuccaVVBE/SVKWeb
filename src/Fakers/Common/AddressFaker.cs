namespace Svk.Fakers.Common;

public class AddressFaker : Faker<Address>
{
    public AddressFaker(string locale = "nl") : base(locale)
    {
        CustomInstantiator(f => new Address(
            f.Address.StreetAddress(),
            f.Address.ZipCode(),
            f.Address.BuildingNumber(),
            f.Address.CountryCode()
        ));
    }
}