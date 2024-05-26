namespace Svk.Fakers.Common;

public class LoaderFaker : UserFaker<Loader>
{
    public LoaderFaker(string locale = "nl") : base(locale)
    {
        CustomInstantiator(f => new Loader(f.Internet.Email(), f.Name.FullName(), f.Random.Word()));
    }
}