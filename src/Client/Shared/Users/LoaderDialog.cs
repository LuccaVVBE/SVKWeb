using Microsoft.AspNetCore.Components;
using MudBlazor;
using Svk.Shared.Users;

namespace Svk.Client.Shared.Users;

public partial class LoaderDialog
{
    [Inject] public IDialogService DialogService { get; set; } = default!;
    [Inject] public IAuthService AuthService { get; set; } = default!;

    [Inject] public HttpClient HttpClient { get; set; } = default!;

    [Inject] public ISnackbar Snackbar { get; set; } = default!;

    [CascadingParameter] MudDialogInstance MudDialog { get; set; }

    [Parameter] public LoaderDto.Mutate Loader { get; set; } = new LoaderDto.Mutate();
    [Parameter] public EventCallback<LoaderDto.Detail> OnLoaderUpdated { get; set; }
    [Parameter] public EventCallback OnClose { get; set; }
    [Parameter] public MudDataGrid<LoaderDto.Index>? grid {get; set;}

    private int _countdownValue = 5; // Start countdown from 5 seconds
    private CancellationTokenSource _cts;
    private bool _dialogIsOpen = false;
    private string _result;
    private bool _isCreatingUser = false;
    void Cancel() => MudDialog.Cancel();

    private async Task CloseDialog()
    {
        _cts = new CancellationTokenSource();

        while (_countdownValue > 0)
        {
            await Task.Delay(1000, _cts.Token); // Wait for 1 second
            _countdownValue--;
            await InvokeAsync(StateHasChanged);
        }

        _dialogIsOpen = false;
        await InvokeAsync(StateHasChanged);
    }

    private async Task Submit()
    {
        try
        {
            _isCreatingUser = true;
            _result = await AuthService.CreateLoaderAsync(Loader);
            Snackbar.Add($"Lader {Loader.Name} werd toegevoegd.", Severity.Success);
            grid.ReloadServerData();
        }
        catch (HttpRequestException httpEx)
        {
            Snackbar.Add(httpEx.Message, Severity.Error);
            _isCreatingUser = false;
            await InvokeAsync(StateHasChanged);
            return;
        }

        await CloseDialog();
    }
}