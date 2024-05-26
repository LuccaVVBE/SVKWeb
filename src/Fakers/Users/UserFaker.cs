namespace Svk.Fakers.Common;

public class UserFaker<TEntity> : EntityFaker<TEntity> where TEntity : User
{
    protected UserFaker(string locale = "nl") : base(locale)
    {
        int id = 1;
        RuleFor(x => x.Auth0Id, f => "0");
    }
}