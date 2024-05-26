using Microsoft.AspNetCore.Components;
using MudBlazor;
using Svk.Shared.Users;

namespace Svk.Client.Shared.Users;

public partial class LoadersTable
{
    [Inject] public ILoaderService LoaderService { get; set; } = default!;
    [Inject] public IAuthService AuthService { get; set; } = default!;
    private List<LoaderDto.Index> _loaders = new();
    private SortMode _sortMode = SortMode.Multiple;
    private bool _filterable = false;
    private bool _hover = true;
    private bool _striped = true;
    private int _currentPage = 0;
    private bool _isLoading = false;
    private int _rowsPerPage = 10;
    private string _searchString = "";
    private bool _MultiSelection = true;
    private MudDataGrid<LoaderDto.Index>? _grid;

    private Func<LoaderDto.Index, bool> _quickFilter => x =>
    {
        if (string.IsNullOrWhiteSpace(_searchString))
            return true;

        if (x.Name.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
            return true;


        return false;
    };

    private async Task<GridData<LoaderDto.Index>> LoadGridData(GridState<LoaderDto.Index> state)
    {
        _isLoading = true;
        string? sortField = null;
        SortableFields? sortBy = null;
        bool? descending = null;
        //column sorting
        if (state.SortDefinitions?.Count > 0)
        {
            SortDefinition<LoaderDto.Index> sortOption = state.SortDefinitions.First()!;
            sortField = sortOption.SortBy;
            switch (sortField)
            {
                case nameof(LoaderDto.Index.Id):
                    sortBy = SortableFields.Id;
                    break;
                case nameof(LoaderDto.Index.Name):
                    sortBy = SortableFields.Name;
                    break;
                case nameof(LoaderDto.Index.Email):
                    sortBy = SortableFields.Email;
                    break;
            }

            descending = sortOption.Descending;
        }
        try
        {
            var result = await LoaderService.GetIndexAsync(new UserRequest.Index
            {
                Searchterm = _searchString,
                PageSize = state.PageSize,
                Page = state.Page,
            });
            _isLoading = false;
            return new GridData<LoaderDto.Index>()

            {
                Items = result.Items.Select(
                    x => new LoaderDto.Index()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Email = x.Email
                    }).ToList(),
                TotalItems = result.TotalItems
            };
        }catch(Exception ex)
        {
            Snackbar.Add(ex.Message,Severity.Error);
            _isLoading = false;
        }
        return new GridData<LoaderDto.Index>();
    }

    private async Task HandelUserFilterChanged()
    {
        await _grid?.ReloadServerData();
    }

    private void StartedEditingItem(LoaderDto.Index item)
    {
    }

    private void CanceledEditingItem(LoaderDto.Index item)
    {
    }

    private async Task StartDeleteItemAsync(LoaderDto.Index item)
    {
        bool? result = await DialogService.ShowMessageBox(
            "Bevestig",
            $"Ben je zeker dat je {item.Name} wilt verwijderen?",
            yesText: "Verwijder", cancelText: "Annuleer");

        if (result == true)
        {
            try
            {
                var loader = await LoaderService.GetDetailAsync(item.Id);
                await LoaderService.DeleteAsync(item.Id);
                await AuthService.DeleteUserAsync("auth0|" + loader.Auth0Id!);
                Snackbar.Add($"Lader {loader.Name} is verwijderd.", Severity.Success);

                await _grid.ReloadServerData();
            }catch(Exception ex)
            {
                Snackbar.Add(ex.Message, Severity.Error);
            }
        }
    }

    private async Task CommittedItemChanges(LoaderDto.Index item)
    {
        try
        {
            var loader = await LoaderService.GetDetailAsync(item.Id);
            if (loader is null)
                throw new Exception("Lader niet gevonden!");

            var loaderMutate = new LoaderDto.Mutate()
            {
                Email = loader.Email,
                Name = item.Name,
                Auth0Id = loader.Auth0Id,
            };
        
         await LoaderService.EditAsync(item.Id, loaderMutate);
         Snackbar.Add($"Lader {loader.Name} werd gewijzigd.", Severity.Success );
    
         await _grid.ReloadServerData();
            
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
    }
}