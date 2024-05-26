namespace Svk.Fakers.Common;

public class LoadingSlipFaker : EntityFaker<LoadingSlip>
{
    public LoadingSlipFaker(string locale = "nl") : base(locale)
    {
        CustomInstantiator(f => new LoadingSlip(int.Parse(f.Random.ReplaceNumbers("#########")),
            new Address(f.Address.StreetName(), f.Address.ZipCode(), f.Address.BuildingNumber(),
                f.Address.CountryCode())));
    }
}