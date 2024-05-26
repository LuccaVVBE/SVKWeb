using Microsoft.AspNetCore.Components;
using MudBlazor;
using Svk.Shared.Users;

namespace Svk.Client.Shared.Users;

public partial class ManagersTable
{
    [Inject] public IManagerService ManagerService { get; set; } = default!;
    [Inject] public IAuthService AuthService { get; set; } = default!;
    private List<ManagerDto.Index> _managers = new();
    private SortMode _sortMode = SortMode.Multiple;
    private bool _filterable = false;
    private bool _hover = true;
    private bool _striped = true;
    private int _currentPage = 0;
    private bool _isLoading = false;
    private int _rowsPerPage = 10;
    private string _searchString = "";
    private bool _MultiSelection = true;
    private MudDataGrid<ManagerDto.Index> _grid = default;

    private void StartedEditingItem(ManagerDto.Index item)
    {
    }

    private void CanceledEditingItem(ManagerDto.Index item)
    {
    }

    private async Task StartDeleteItemAsync(ManagerDto.Index item, String name)
    {
        bool? result = await DialogService.ShowMessageBox(
            "Bevestig",
            $"Ben je zeker dat je {item.Name} wilt verwijderen?",
            yesText: "Verwijder", cancelText: "Annuleer");


        if (result == true)
        {
            try
            {
                var manager = await ManagerService.GetDetailAsync(item.Id);
                if (manager is null)
                    throw new Exception("Manager niet gevonden");
                await ManagerService.DeleteAsync(item.Id);
                await AuthService.DeleteUserAsync("auth0id|" + manager.Auth0Id!);
                Snackbar.Add($"Manager {manager.Name} is verwijderd.", Severity.Success);

                await _grid.ReloadServerData();
            }catch (Exception ex)
            {
                Snackbar.Add(ex.Message, Severity.Error);   
            }
        }
        
    }

    private async Task CommittedItemChanges(ManagerDto.Index item)
    {
        try
        {
            var manager = await ManagerService.GetDetailAsync(item.Id);
            if (manager is null)
                throw new Exception("Manager niet gevonden!");

            var managerMutate = new ManagerDto.Mutate()
            {
                Email = manager.Email,
                Name = item.Name,
                Auth0Id = manager.Auth0Id,
            };

            await ManagerService.EditAsync(item.Id, managerMutate);

            Snackbar.Add($"Manager {manager.Name} werd gewijzigd.", Severity.Success);
            await _grid.ReloadServerData();
        } catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
    }

    private Func<ManagerDto.Index, bool> _quickFilter => x =>
    {
        if (string.IsNullOrWhiteSpace(_searchString))
            return true;

        if (x.Name.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
            return true;


        return false;
    };

    private async Task<GridData<ManagerDto.Index>> LoadGridData(GridState<ManagerDto.Index> state)
    {
        _isLoading = true;
        string? sortField = null;
        SortableFields? sortBy = null;
        bool? descending = null;
        //column sorting
        if (state.SortDefinitions?.Count > 0)
        {
            SortDefinition<ManagerDto.Index> sortOption = state.SortDefinitions.First()!;
            sortField = sortOption.SortBy;
            switch (sortField)
            {
                case nameof(ManagerDto.Index.Id):
                    sortBy = SortableFields.Id;
                    break;
                case nameof(ManagerDto.Index.Name):
                    sortBy = SortableFields.Name;
                    break;
            }

            descending = sortOption.Descending;
        }
        try
        {
            var result = await ManagerService.GetIndexAsync(new UserRequest.Index
            {
                Searchterm = _searchString,
                PageSize = state.PageSize,
                Page = state.Page,
            });
            _isLoading = false;
            return new GridData<ManagerDto.Index>()

            {
                Items = result.Items.Select(
                    x => new ManagerDto.Index()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Email = x.Email
                    }).ToList(),
                TotalItems = result.TotalItems
            };
        }catch(Exception ex)
        {
            _isLoading = false;
            Snackbar.Add(ex.Message, Severity.Error);
        }
        return new GridData<ManagerDto.Index>();
    }

    private async Task HandelUserFilterChanged()
    {
        await _grid?.ReloadServerData();
    }
}