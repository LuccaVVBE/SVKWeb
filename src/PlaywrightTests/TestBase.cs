using System.Threading.Tasks;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace Svk.PlaywrightTests;

using PlaywrightSharp = Microsoft.Playwright;

public abstract class TestBase : PageTest
{
    protected const string ServerBaseUrl = "https://localhost:5001";
    protected PlaywrightSharp.IPlaywright _playwright;
    protected PlaywrightSharp.IBrowser _browser;
    protected PlaywrightSharp.IBrowserContext _context;

    protected const string Email = "nico@test.test";
    protected const string Password = "RbaToKSYPNTj2gY7";

    [SetUp]
    public async Task Setup()
    {
        _playwright = await PlaywrightSharp.Playwright.CreateAsync();
        _browser = await _playwright.Chromium.LaunchAsync(new PlaywrightSharp.BrowserTypeLaunchOptions
            { Headless = false });
        _context = await _browser.NewContextAsync();

        await Page.GotoAsync($"{ServerBaseUrl}/authentication/login");
        await Page.GetByLabel("Email address").ClickAsync();
        await Page.GetByLabel("Email address").FillAsync(Email);
        await Page.GetByLabel("Password").ClickAsync();
        await Page.GetByLabel("Password").FillAsync(Password);
        await Page.GetByRole(PlaywrightSharp.AriaRole.Button, new() { Name = "Continue", Exact = true }).ClickAsync();
    }

    [TearDown]
    public async Task TearDown()
    {
        await _context.CloseAsync();
        await _browser.CloseAsync();
        _playwright.Dispose();
    }
}