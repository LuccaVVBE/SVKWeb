using Microsoft.AspNetCore.Components;
using MudBlazor;
using Svk.Client.Extensions;
using Svk.Shared.Controles;
using Svk.Shared.Misc;

namespace Svk.Client.Shared.Controles;

public partial class ControleTable
{
    [Inject] public IControleService ControleService { get; set; } = default!;
    private List<extendedControleDto> _controles = new();
    private SortMode _sortMode = SortMode.Single;
    private bool _filterable = false;
    private bool _hover = true;
    private bool _striped = true;
    private int _currentPage = 0;
    private bool _isLoading = false;
    private int _rowsPerPage = 5;
    private int[] _pageSizeOptions = { 5, 10, 25, 50, 100 };
    private FilterData _filterData = new();
    private MudDataGrid<extendedControleDto>? _controleGrid;
    private string _sortField = "";
    private bool descending = false;
    private bool _firstLoad = true;
    private SortDefinition<extendedControleDto>? _previousSortDefinition = null;
    private String? _previousFilterDataAsQueryString = null;
    private int _previousPage = 0;
    private int _previousPageSize = 0;

    //accept filter changes
    private async Task HandelFilterChanged(FilterData filterData)
    {
        _filterData = filterData;
        //reset page to 0 when filter changes
        if (_currentPage != 0)
        {
            _currentPage = 0;
            //reloads data automatically when page changes
        }
        else
        {
            _controleGrid?.ReloadServerData();
        }
    }


    /// <summary>
    /// Loads grid data based on the provided grid state and filter data.
    /// </summary>
    /// <param name="state">The grid state including sorting, filtering, and pagination options.</param>
    /// <returns>A task representing the asynchronous operation. The result contains the grid data.</returns>
    private async Task<GridData<extendedControleDto>> LoadGridData(GridState<extendedControleDto> state)
    {
        //Return previous data if filter and sort are the same and page is the same and page length is the same
        if (!_firstLoad && _previousSortDefinition == state.SortDefinitions.FirstOrDefault() &&
            _previousFilterDataAsQueryString == _filterData.AsQueryString() && _previousPage == state.Page &&
            _rowsPerPage == state.PageSize)
        {
            return new GridData<extendedControleDto>()
            {
                Items = _controleGrid!.Items,
                TotalItems = _controleGrid!.Items.Count()
            };
        }

        //reset page to 0 when sort changes
        if (state.Page != 0 && _previousSortDefinition != state.SortDefinitions.FirstOrDefault())
        {
            _currentPage = 0;
            _controleGrid?.NavigateTo(Page.First);
            //also set page 0 null in grid state
            state.Page = 0;
        }

        //Save previous filter and sort
        _previousSortDefinition = state.SortDefinitions.FirstOrDefault();
        _previousFilterDataAsQueryString = _filterData.AsQueryString();
        _previousPage = state.Page;


        _firstLoad = false;
        _isLoading = true;


        sortableFields? sortBy;
        bool? descending;
        (sortBy, descending) = GetSortField(state);


        //Make request
        try
        {
            var result = await ControleService.GetIndexAsync(CreateRequest(state, sortBy, descending, _filterData));





            _isLoading = false;

            //Format data to required format
            return new GridData<extendedControleDto>()
            {
                Items = (result.Items).Select(
                    x => new extendedControleDto()
                    {
                        Nummerplaat = x.Nummerplaat,
                        Transporteur = x.Transporteur,
                        Id = x.Id,
                        RouteNummersString = string.Join(", ", x.Routenummers ?? new List<string>()),
                        Datum = x.Datum,
                        Laadbonnummers = (x.Laadbonnen ?? Array.Empty<LaadBonDto.Index>())
                            .Select(laadbon => laadbon.LaadBonnummer).ToList()
                    }
                ).ToList(),
                TotalItems = result.TotalItems
            };
        }catch(Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
            _isLoading = false;
        }
        return new GridData<extendedControleDto>();
    }


    /// <summary>
    /// Creates ControlRequest.Index object based on the provided parameters.
    /// </summary>
    /// <param name="state">The grid state.</param>
    /// <param name="sortBy">The field to order by.</param>
    /// <param name="isDescending">Indicates whether to sort in descending order.</param>
    /// <param name="filterData">The filter data.</param>
    /// <param name="sortField">The field to order by.</param>
    /// <returns>A ControlRequest.Index object.</returns>
    private ControleRequest.Index CreateRequest(GridState<extendedControleDto> state,
        sortableFields? sortBy, bool? isDescending, FilterData filterData)
    {
        return new ControleRequest.Index()
        {
            Pagination = new Pagination.Index
            {
                Page = state.Page,
                PageSize = state.PageSize
            },
            PageSize = state.PageSize,
            Page = state.Page,
            Laadbonnummer = filterData.Laadbonnummer,
            Transporteur = filterData.Transporteur,
            Nummerplaat = filterData.Nummerplaat,
            Routenummer = filterData.Routenummer,
            Lader = filterData.Lader,
            StartDateRange = filterData._datumRange.Start,
            EndDateRange = filterData._datumRange.End,
            SortBy = sortBy,
            SortDescending = isDescending ?? false
        };
    }


    //TODO ask if correct
    protected void FiltersLoaded(FilterData filterData)
    {
        _filterData = filterData;
        if (_firstLoad)
        {
            _controleGrid?.ReloadServerData();
        }
    }

    /// Retrieves the sort field, sort by and descending flag from
    /// the given GridState object.
    /// @param state The GridState object containing the sort definitions
    /// .
    /// @return A tuple containing the sortField, sortBy and descending
    /// values.
    /// /
    private (sortableFields? sortBy, bool? descending) GetSortField(
        GridState<extendedControleDto> state)
    {
        string? sortField = null;
        sortableFields? sortBy = null;
        bool? descending = null;

        if (state.SortDefinitions?.Count > 0)
        {
            SortDefinition<extendedControleDto> sortOption = state.SortDefinitions.First()!;
            sortField = sortOption.SortBy;
            descending = sortOption.Descending;
            switch (sortField)
            {
                case nameof(ControleDto.Index.Laadbonnen):
                    sortBy = sortableFields.Laadbonnummer;
                    break;
                case nameof(ControleDto.Index.Transporteur):
                    sortBy = sortableFields.Transporteur;
                    break;
                case nameof(ControleDto.Index.Nummerplaat):
                    sortBy = sortableFields.Nummerplaat;
                    break;
                case nameof(ControleDto.Index.Id):
                    sortBy = sortableFields.Id;
                    break;
                case nameof(extendedControleDto.RouteNummersString):
                    sortBy = sortableFields.Routenummer;
                    break;
                case nameof(extendedControleDto.Datum):
                    sortBy = sortableFields.Datum;
                    break;
            }
        }

        return (sortBy, descending);
    }


    private EventCallback<DataGridRowClickEventArgs<extendedControleDto>> RowClicked =>
        EventCallback.Factory.Create<DataGridRowClickEventArgs<extendedControleDto>>(this, OnRowClicked);

    /// <summary>
    /// Event handler for when a row in the data grid is clicked.
    /// </summary>
    /// <param name="route">The event argument containing the clicked row information.</param>
    private void OnRowClicked(DataGridRowClickEventArgs<extendedControleDto> route)
    {
        Navigation.NavigateTo($"/controles/{route.Item.Id}");
    }
}