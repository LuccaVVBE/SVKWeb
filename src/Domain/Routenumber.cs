using Ardalis.GuardClauses;
using Svk.Domain.Common;

namespace Svk.Domain;

public class Routenumber : Entity
{
    private string _number = default!;

    public Routenumber()
    {
    }

    public Routenumber(string number)
    {
        Number = number;
    }

    public string Number
    {
        get => _number;
        set => _number = Guard.Against.NullOrEmpty(value, nameof(Number));
    }
}