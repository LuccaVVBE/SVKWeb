using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Minio;
using Serilog;
using Svk.Persistence;
using Svk.Server.Middleware;
using Svk.Services;
using Svk.Shared.Controles;
using Svk.Shared.Users;
using static Svk.Shared.Misc.Result;

//Logger
Log.Logger = new LoggerConfiguration()
    // TODO
    .WriteTo.Console()
    .CreateBootstrapLogger();


// Add services to the container.

try
{
    var builder = WebApplication.CreateBuilder(args);

    //Logging
    builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
        loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration));

    builder.Services.AddControllersWithViews();


    builder.Services.AddMinio(configureClient => configureClient
        .WithEndpoint(builder.Configuration["MinioConnection:Endpoint"])
        .WithCredentials(builder.Configuration["MinioConnection:AccessKey"],
            builder.Configuration["MinioConnection:SecretKey"]));


    builder.Services.addSvkServices();


    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Auth0:Authority"];
        options.Audience = builder.Configuration["Auth0:ApiIdentifier"];
    });

    builder.Services.AddRazorPages();


    builder.Services.AddMinio(configureClient => configureClient
        .WithEndpoint(builder.Configuration["MinioConnection:Endpoint"])
        .WithCredentials(builder.Configuration["MinioConnection:AccessKey"],
            builder.Configuration["MinioConnection:SecretKey"]));


    builder.Services.AddDbContext<SvkDbContext>(options =>
    {
        if (builder.Environment.IsDevelopment())
        {
            options.UseSqlServer
            (
                builder.Configuration.GetConnectionString("SqlServer")
            );
        }
        else
        {
            var serverVersion = new MariaDbServerVersion(new Version(11, 5, 22));
            options.UseMySql
            (
                builder.Configuration.GetConnectionString("MySqlServer"), serverVersion
            );
        }
    });

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        
        options.SwaggerDoc("v1", new()
        {
            Title = "Svk Api", 
            Version = "v1",
            Description = "This is the api for the svk application. It is used by the frontend to communicate with the backend and the android app.",
        });
        // Since we subclass our dto's we need a more unique id.
        options.CustomSchemaIds(type =>
            type == typeof(GetItemsPaginated<LoaderDto.Index>) ? "PaginatedLoader.Index" :
            type == typeof(GetItemsPaginated<ControleDto.Index>) ? "PaginatedControle.Index" :
            type.DeclaringType is null ? $"{type.Name}" : $"{type.DeclaringType?.Name}.{type.Name}");
        options.EnableAnnotations();
    });
    builder.Services.AddValidatorsFromAssemblyContaining<ControleDto.Create.Validator>()
        .AddFluentValidationAutoValidation();
    //builder.Services.AddFluentValidationRulesToSwagger();

    var app = builder.Build();


    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseWebAssemblyDebugging();
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    else
    {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseBlazorFrameworkFiles();

    //addLogger middleware after blazor to avoid logging blaz file requests
    app.UseSerilogRequestLogging();


    app.UseStaticFiles();

    app.UseMiddleware<ExceptionMiddleware>();
    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    using (var scope = app.Services.CreateScope())
    {
        // Require a DbContext from the service provider and seed the database if in development.
        var dbContext = scope.ServiceProvider.GetRequiredService<SvkDbContext>();
        if (app.Environment.IsDevelopment() || builder.Configuration["seeding"] == "true")
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
            //TODO CHANGE TO SEEED
            FakeSeeder seeder = new(dbContext);
            seeder.Seed();
        }
        else if (app.Environment.IsProduction())
        {
            dbContext.Database.EnsureCreated();
        }
    }

    app.MapRazorPages();
    app.MapControllers();
    app.MapFallbackToFile("index.html");

    app.Run();
}
catch (Exception e)
{
    Log.Fatal(e, "Fatal Exception");
}
finally
{
    Log.CloseAndFlush();
}