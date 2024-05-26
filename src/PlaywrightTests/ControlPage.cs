using System.Threading.Tasks;
using NUnit.Framework;

namespace Svk.PlaywrightTests;

[Parallelizable(ParallelScope.Self)]
public class ControlPage : TestBase
{
    [Test]
    public async Task TableShowsDataAfterAuth()
    {
        await Page.GotoAsync($"{ServerBaseUrl}/controles");

        var table = await Page.WaitForSelectorAsync("[data-cy=dataGrid]");
        var rows = await table!.QuerySelectorAllAsync("tr");
        Assert.That(rows, Has.Count.GreaterThan(1));
    }


    [Test]
    public async Task TableShowsPicturesAfterClickingNr()
    {
        await Page.GotoAsync($"{ServerBaseUrl}/controles");
        var table = await Page.WaitForSelectorAsync("[data-cy=dataGrid]");
        var rows = await table!.QuerySelectorAllAsync("tr");
        Assert.That(rows, Has.Count.GreaterThan(1));
        var cells = await table!.WaitForSelectorAsync("[data-label=Nr]");
        await cells!.ClickAsync();
        var laadbonAssert = await Page.WaitForSelectorAsync("[data-cy=laadbon]");
        Assert.IsNotNull(laadbonAssert, "Laadbon is not available");
        var pictures = await Page.QuerySelectorAllAsync("[data-cy=img]");
        Assert.That(pictures, Has.Count.GreaterThan(1));
    }

    [Test]
    public async Task TableLoadsLaadbonnenAfterClickingNr()
    {
        await Page.GotoAsync($"{ServerBaseUrl}/controles");

        var table = await Page.WaitForSelectorAsync("[data-cy=dataGrid]");
        var rows = await table!.QuerySelectorAllAsync("tr");
        Assert.That(rows, Has.Count.GreaterThan(1));
        var cells = await table!.WaitForSelectorAsync("[data-label=Nr]");
        await cells!.ClickAsync();
        var laadbonAssert = await Page.WaitForSelectorAsync("[data-cy=laadbon]");
        Assert.IsNotNull(laadbonAssert, "Laadbon is not available");
        var laadbonnen = await Page.QuerySelectorAllAsync("[data-cy=laadbon]");
        Assert.That(laadbonnen, Has.Count.GreaterThan(1));
    }

    [Test]
    public async Task CheckAllProperties()
    {
        await Page.GotoAsync($"{ServerBaseUrl}/controles");

        var dataGrid = await Page.WaitForSelectorAsync("[data-cy=dataGrid]");
        Assert.IsNotNull(dataGrid, "DataGrid is not available");

        var title = await Page.QuerySelectorAsync("[data-label=Nr]");
        Assert.IsNotNull(title, "Nr is not available");

        var columnId = await Page.QuerySelectorAsync("[data-label=RouteNummers]");
        Assert.IsNotNull(columnId, "RouteNummers is not available");

        var columnRouteNummers = await Page.QuerySelectorAsync("[data-label=Transporteur]");
        Assert.IsNotNull(columnRouteNummers, "Transporteur is not available");

        var columnTransporteur = await Page.QuerySelectorAsync("[data-label=Nummerplaat]");
        Assert.IsNotNull(columnTransporteur, "Nummerplaat is not available");

        var columnNummerplaat = await Page.QuerySelectorAsync("[data-label=Datum]");
        Assert.IsNotNull(columnNummerplaat, "ColumnNummerplaat is not available");

        var pager = await Page.QuerySelectorAsync("[data-cy=pager]");
        Assert.IsNotNull(pager, "Pager is not available");
    }
}