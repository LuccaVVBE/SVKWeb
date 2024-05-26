using Microsoft.AspNetCore.Components;
using MudBlazor;
using Svk.Client.Extensions;

namespace Svk.Client.Shared.Controles;

public class FilterData
{
    public MudDateRangePicker _datePicker = new();
    public DateRange? _datumRange = new DateRange(DateTime.Now.Subtract(TimeSpan.FromDays(14)).Date, DateTime.Now.Date);

    public DateTime? StartDateRange
    {
        get => _datumRange?.Start!;
        set
        {
            _datumRange = new DateRange(value, _datumRange?.End);
            _datePicker.DateRange = _datumRange;
        }
    }

    public DateTime? EndDateRange
    {
        get => _datumRange?.End;
        set
        {
            _datumRange = new DateRange(_datumRange.Start, value);
            _datePicker.DateRange = _datumRange;
        }
    }

    public int? Laadbonnummer { get; set; }
    public string? Transporteur { get; set; }
    public string? Lader { get; set; }
    public int? Routenummer { get; set; }
    public string? Nummerplaat { get; set; }

    protected bool Equals(FilterData other)
    {
        return _datePicker.Equals(other._datePicker) && Equals(_datumRange, other._datumRange) &&
               Laadbonnummer == other.Laadbonnummer && Transporteur == other.Transporteur && Lader == other.Lader &&
               Routenummer == other.Routenummer && Nummerplaat == other.Nummerplaat;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((FilterData)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_datePicker, _datumRange, Laadbonnummer, Transporteur, Lader, Routenummer, Nummerplaat);
    }
}

public partial class Filter
{
    private readonly FilterData _filterData = new FilterData();

    //Parameter to subscribe to filter changes
    [Parameter] public EventCallback<FilterData> FilterChanged { get; set; }

    [Parameter] public EventCallback<FilterData> FiltersLoaded { get; set; }

    public FilterData FilterData => _filterData;


    protected async override Task OnInitializedAsync()
    {
        var uri = new Uri(Navigation.Uri);
        var query = uri.Query;
        _filterData.FromQueryString(query);
        await FiltersLoaded.InvokeAsync(_filterData);
    }


    private async Task HandelFilterChanged()
    {
        Navigation.NavigateTo($"{Navigation.BaseUri}?{_filterData.AsQueryString()}");
        if (FilterChanged.HasDelegate)
        {
            await FilterChanged.InvokeAsync(_filterData);
        }
    }
}