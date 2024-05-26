using MudBlazor;

namespace Svk.Client.Shared;

public partial class MainLayout
{
    private bool _isDarkMode = false;

    private MudTheme _theme = new MudTheme()
    {
        Palette = new PaletteLight()
        {
            Primary = "#cd171a"
        },
        PaletteDark = new PaletteDark()
        {
            Primary = "#cd171a"
        },
        Typography = new Typography()
        {
            H1 = new H1()
            {
                FontFamily = new[] { "baron" },
            },
            H2 = new H2()
            {
                FontFamily = new[] { "Roboto", "sans-serif" },
                FontWeight = 700,
                FontSize = "14px"
            },
            H3 = new H3()
            {
                FontFamily = new[] { "Roboto", "sans-serif" },
                FontWeight = 500,
                FontSize = "12px"
            },
            Default = new Default()
            {
                FontFamily = new[] { "Roboto", "sans-serif" },
                FontWeight = 300,
                FontSize = "11px"
            }
        }
    };
}