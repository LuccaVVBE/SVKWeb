namespace Svk.Fakers.Common;

public class TransporterFaker : EntityFaker<Transporter>
{
    public TransporterFaker(string locale = "nl") : base(locale)
    {
        CustomInstantiator(f => new Transporter(f.Name.FullName()));
    }
}