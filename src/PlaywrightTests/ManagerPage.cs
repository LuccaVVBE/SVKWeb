using System.Threading.Tasks;
using NUnit.Framework;

namespace Svk.PlaywrightTests;

[Parallelizable(ParallelScope.Self)]
public class ManagerPage : TestBase
{
    [Test]
    public async Task CheckAllManagerProperties()
    {
        await Page.GotoAsync($"{ServerBaseUrl}/managers");

        var dataGrid = await Page.WaitForSelectorAsync("[data-cy=dataGrid]");
        Assert.IsNotNull(dataGrid, "DataGrid is not available");

        var title = await Page.QuerySelectorAsync("[data-label=Naam]");
        Assert.IsNotNull(title, "Naam is not available");

        var columnId = await Page.QuerySelectorAsync("[data-label=Nr]");
        Assert.IsNotNull(columnId, "Nr is not available");
    }
}