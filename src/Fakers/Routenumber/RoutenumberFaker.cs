using Svk.Fakers.Common;

namespace Svk.Fakers.Routenumber;

public class RoutenumberFaker : EntityFaker<Domain.Routenumber>
{
    public RoutenumberFaker(string locale = "nl") : base(locale)
    {
        CustomInstantiator(f => new Domain.Routenumber(
            f.Random.ReplaceNumbers("#########")
        ));
    }
}