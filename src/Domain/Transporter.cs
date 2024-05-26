using Ardalis.GuardClauses;
using Svk.Domain.Common;

namespace Svk.Domain;

public class Transporter : Entity
{
    public Transporter(string name)
    {
        Name = Guard.Against.NullOrEmpty(name, nameof(name));
    }

    public string Name { get; set; }
}