using Microsoft.EntityFrameworkCore;
using Svk.Domain;
using Svk.Domain.Exceptions;
using Svk.Persistence;
using Svk.Shared.Misc;
using Svk.Shared.Users;

public class LoaderService : ILoaderService
{
    private readonly SvkDbContext dbContext;


    public LoaderService(SvkDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<Result.GetItemsPaginated<LoaderDto.Index>> GetIndexAsync(UserRequest.Index request)
    {
        var query = dbContext.Loaders.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Searchterm))
        {
            query = query.Where(x => x.Name.Contains(request.Searchterm));
        }

        int totalItems = await query.CountAsync();

        var items = await query
            .Skip((request.Page) * request.PageSize)
            .Take(request.PageSize)
            .OrderBy(x => x.Id)
            .Select(x => new LoaderDto.Index
            {
                Id = x.Id,
                Name = x.Name,
                Email = x.Email
            }).ToListAsync();

        var result = new Result.GetItemsPaginated<LoaderDto.Index>
        {
            Items = items,
            TotalItems = totalItems,
        };

        return result;
    }

    public async Task<LoaderDto.Detail> GetDetailAsync(int loaderId)
    {
        LoaderDto.Detail? user = await dbContext.Loaders.Select(x => new LoaderDto.Detail
        {
            Id = x.Id,
            Name = x.Name,
            Auth0Id = x.Auth0Id,
            Email = x.Email,
        }).SingleOrDefaultAsync(x => x.Id == loaderId);

        //TODO: check EntityNotFoundException
        if (user is null)
            throw new EntityNotFoundException(nameof(Loader), nameof(Loader.Id));

        return user;
    }

    public async Task<LoaderDto.Detail> GetDetailAsync(string auth0Id)
    {
        LoaderDto.Detail? user = await dbContext.Loaders.Select(x => new LoaderDto.Detail
        {
            Id = x.Id,
            Name = x.Name,
            Email = x.Email,
            Auth0Id = x.Auth0Id,
        }).SingleOrDefaultAsync(x => x.Auth0Id == auth0Id);

        if (user is null)
            throw new EntityNotFoundException(nameof(Loader), nameof(Loader.Auth0Id));

        return user;
    }

    public async Task EditAsync(int loaderId, LoaderDto.Mutate model)
    {
        Loader? loader = await dbContext.Loaders.SingleOrDefaultAsync(x => x.Id == loaderId);

        if (loader is null)
            throw new EntityNotFoundException(nameof(Loader), loaderId);

        loader.Name = model.Name!;
        loader.Email = model.Email;
        loader.Auth0Id = model.Auth0Id!;

        await dbContext.SaveChangesAsync();
    }

    public async Task<int> CreateAsync(LoaderDto.Mutate model)
    {
        if (await dbContext.Loaders.AnyAsync(x => x.Name == model.Name))
            throw new EntityNotFoundException(nameof(Loader.Name), model.Name!);


        Loader loader = new Loader(model.Email!, model.Name!, model.Auth0Id!);


        dbContext.Loaders.Add(loader);
        await dbContext.SaveChangesAsync();

        return loader.Id;
    }

    public async Task DeleteAsync(int loaderId)
    {
        Loader? loader = await dbContext.Loaders.SingleOrDefaultAsync(x => x.Id == loaderId);

        if (loader is null)
            throw new EntityNotFoundException(nameof(Loader), loaderId);

        dbContext.Loaders.Remove(loader);

        await dbContext.SaveChangesAsync();
    }
}