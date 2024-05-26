using System;
using JetBrains.Annotations;
using Shouldly;
using Svk.Domain;
using Xunit;

namespace Svk.Tests;

[TestSubject(typeof(Manager))]
public class ManagerTest
{
    [Fact]
    public void Be_created_when_valid()
    {
        //Arange
        const string email = "managerjef.email@email.com";
        const string name = "ManagerJef";
        const string auth0Id = "authId999";

        //Act
        var manager = new Manager(name, email, auth0Id);

        //Assert
        manager.Email.ShouldBe(email);
        manager.Name.ShouldBe(name);
        manager.Auth0Id.ShouldBe(auth0Id);
    }
    
    [Fact]
    public void Throw_when_invalid_email()
    {
        //Arange
        const string email = null;
        const string name = "ManagerJef";
        const string auth0Id = "authId999";

        //Act
        Action act = () =>
        {
            var manager = new Manager(email, name, auth0Id);
        };

        //Assert
        act.ShouldThrow<Exception>();
    }
    
    [Fact]
    public void Throw_when_invalid_name_null()
    {
        //Arange
        const string email = "managerjef.email@email.com";
        const string name = null;
        const string auth0Id = "authId999";

        //Act
        Action act = () =>
        {
            var manager = new Manager(email, name, auth0Id);
        };

        //Assert
        act.ShouldThrow<Exception>();
    }
    
    [Fact]
    public void Throw_when_invalid_name_empty()
    {
        //Arange
        const string email = "managerjef.email@email.com";
        const string name = "";
        const string auth0Id = "authId999";

        //Act
        Action act = () =>
        {
            var manager = new Manager(email, name, auth0Id);
        };

        //Assert
        act.ShouldThrow<Exception>();
    }
}