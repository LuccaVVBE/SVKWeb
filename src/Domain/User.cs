using Ardalis.GuardClauses;
using Svk.Domain.Common;

namespace Svk.Domain;

public abstract class User : Entity
{
    private string _name = default!;
    private string _email = default!;
    private string? _auth0Id = default; //TODO: fix this

    public User()
    {
    }

    public User(string name, string auth0Id, string email)
    {
        Name = name;
        Auth0Id = auth0Id;
        Email = email;
    }


    public string Name
    {
        get => _name;
        set => _name = Guard.Against.NullOrEmpty(value, nameof(Name));
    }

    public string Auth0Id
    {
        get => _auth0Id;
        set => _auth0Id = value;
        //TODO: check if guard is needed here
        //set => _auth0Id = Guard.Against.NullOrEmpty(value, nameof(Auth0Id));
    }

    public string Email
    {
        get => _email;
        set => _email = Guard.Against.NullOrEmpty(value, nameof(Email));
    }
}