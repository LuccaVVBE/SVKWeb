using Svk.Fakers.TransportControl;

namespace Svk.Persistence;

public class FakeSeeder
{
    private readonly SvkDbContext dbContext;

    public FakeSeeder(SvkDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public void Seed()
    {
        SeedLoaders();
        SeedManagers();
        SeedTransportControls();
    }

    private void SeedLoaders()
    {
        var loaders = new List<Loader>();
        var testLader = new Loader("lader@test.test", "Fluppe", "6584748e61f1db9d4005683c"); //Tn4PUbMzvR2BihuQ
        var testLader2 = new Loader("svk@test.test", "svk", "658474d81d2776d4898c51b3"); //V36z6E4eQoeQ56YE
        loaders.Add(testLader);
        loaders.Add(testLader2);
        dbContext.Loaders.AddRange(loaders);
        dbContext.SaveChanges();
    }

    private void SeedTransportControls()
    {
        var transportControls = new TransportControlFaker().AsTransient().Generate(50);
        dbContext.TransportControls.AddRange(transportControls);
        dbContext.SaveChanges();
    }


    private void SeedManagers()
    {
        var adminManager = new Manager("Nico admin", "nico@test.test", "6584751f61f1db9d40056883"); //RbaToKSYPNTj2gY7
        dbContext.Managers.Add(adminManager);
        dbContext.SaveChanges();
    }
}