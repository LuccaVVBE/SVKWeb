using Svk.Domain.Files;
using Svk.Fakers.Common;
using Svk.Fakers.Routenumber;

namespace Svk.Fakers.TransportControl;

public class TransportControlFaker : EntityFaker<Domain.TransportControl>
{
    public TransportControlFaker(string locale = "nl") : base(locale)
    {
        var seedBucket = "seed";
        var amountOfImages = 21;
        CustomInstantiator(f => new Domain.TransportControl(
            Enumerable.Range(1, 5)
                .Select(i => new RoutenumberFaker().AsTransient().Generate())
                .ToList(),
            new LoadingSlipFaker().AsTransient().Generate(5),
            new TransporterFaker().AsTransient().Generate(),
            f.Random.ReplaceNumbers("#########"),
            Enumerable.Range(1, amountOfImages)
                .Select(i => new Image(seedBucket, i.ToString()))
                .ToList(),
            new LoaderFaker().AsTransient().Generate(),
            f.Date.Past()
        ));
    }
}