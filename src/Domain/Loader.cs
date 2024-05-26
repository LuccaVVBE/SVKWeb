namespace Svk.Domain;

public class Loader : User
{
    public Loader()
    {
    }

    public Loader(string email, string name, string auth0Id)
    {
        Email = email;
        Name = name;
        Auth0Id = auth0Id;
    }
}