namespace Svk.Domain;

public class Manager : User
{
    public Manager()
    {
    }

    public Manager(string name, string email, string auth0Id)
    {
        Name = name;
        Email = email;
        Auth0Id = auth0Id;
    }
}