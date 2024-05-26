using System.Threading.Tasks;
using NUnit.Framework;

namespace Svk.PlaywrightTests;

[Parallelizable(ParallelScope.Self)]
public class LoaderPage : TestBase
{
    [Test]
    public async Task CheckAllLoaderProperties()
    {
        await Page.GotoAsync($"{ServerBaseUrl}/laders");

        var dataGrid = await Page.WaitForSelectorAsync("[data-cy=dataGrid]");
        Assert.IsNotNull(dataGrid, "DataGrid is not available");

        var title = await Page.QuerySelectorAsync("[data-label=Naam]");
        Assert.IsNotNull(title, "Naam is not available");

        var columnId = await Page.QuerySelectorAsync("[data-label=Nr]");
        Assert.IsNotNull(columnId, "Nr is not available");
    }
}