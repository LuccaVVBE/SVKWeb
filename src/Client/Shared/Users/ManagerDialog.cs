using Microsoft.AspNetCore.Components;
using MudBlazor;
using Svk.Shared.Users;

namespace Svk.Client.Shared.Users;

public partial class ManagerDialog
{
    [Inject] public IManagerService ManagerService { get; set; } = default!;

    [Inject] public IAuthService AuthService { get; set; } = default!;

    [Inject] public HttpClient HttpClient { get; set; } = default!;

    [Inject] public ISnackbar Snackbar { get; set; } = default!;

    [CascadingParameter] MudDialogInstance MudDialog { get; set; }

    [Parameter] public ManagerDto.Mutate Manager { get; set; } = new ManagerDto.Mutate();
    [Parameter] public MudDataGrid<ManagerDto.Index>? grid { get; set; }

    private bool _dialogIsOpen = false;
    private bool _isCreatingManager = false;
    private string _result = "";

    private async Task Submit()
    {
        try
        {
            _isCreatingManager = true;
            _result = await AuthService.CreateManagerAsync(Manager);
            Snackbar.Add($"Manager {Manager.Name} werd aangemaakt.", Severity.Success);
            grid.ReloadServerData();
        }
        catch (HttpRequestException httpEx)
        {
            Snackbar.Add(httpEx.Message, Severity.Error);
            _isCreatingManager = false;
            await InvokeAsync(StateHasChanged);
        }
    }
}