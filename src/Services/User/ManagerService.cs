using Microsoft.EntityFrameworkCore;
using Svk.Domain;
using Svk.Domain.Exceptions;
using Svk.Persistence;
using Svk.Shared.Misc;
using Svk.Shared.Users;

namespace Svk.Services.User;

public class ManagerService : IManagerService
{
    private readonly SvkDbContext dbContext;

    public ManagerService(SvkDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<Result.GetItemsPaginated<ManagerDto.Index>> GetIndexAsync(UserRequest.Index request)
    {
        var query = dbContext.Managers.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Searchterm))
        {
            query = query.Where(x => x.Name.Contains(request.Searchterm));
        }

        int totalItems = await query.CountAsync();

        var items = await query
            .Skip((request.Page) * request.PageSize)
            .Take(request.PageSize)
            .OrderBy(x => x.Id)
            .Select(x => new ManagerDto.Index
            {
                Id = x.Id,
                Name = x.Name,
                Email = x.Email
            }).ToListAsync();

        var result = new Result.GetItemsPaginated<ManagerDto.Index>
        {
            Items = items,
            TotalItems = totalItems,
        };

        return result;
    }

    public async Task<ManagerDto.Detail> GetDetailAsync(int managerId)
    {
        ManagerDto.Detail? user = await dbContext.Managers.Select(x => new ManagerDto.Detail
        {
            Id = x.Id,
            Name = x.Name,
            Auth0Id = x.Auth0Id,
            Email = x.Email,
        }).SingleOrDefaultAsync(x => x.Id == managerId);

        //TODO: check EntityNotFoundException
        if (user is null)
            throw new EntityNotFoundException(nameof(Manager), nameof(Manager.Id));

        return user;
    }

    public async Task<int> CreateAsync(ManagerDto.Mutate model)
    {
        if (await dbContext.Managers.AnyAsync(x => x.Auth0Id == model.Auth0Id))
            throw new EntityAlreadyExistsException(nameof(Manager), nameof(Manager.Auth0Id), model.Auth0Id!);

        Manager manager = new Manager(model.Name!, model.Email!, model.Auth0Id!);

        dbContext.Managers.Add(manager);
        await dbContext.SaveChangesAsync();

        return manager.Id;
    }

    public async Task EditAsync(int managerId, ManagerDto.Mutate model)
    {
        Manager? manager = await dbContext.Managers.SingleOrDefaultAsync(x => x.Id == managerId);

        if (manager is null)
            throw new EntityNotFoundException(nameof(Manager), managerId);

        manager.Name = model.Name!;
        manager.Auth0Id = model.Auth0Id!;

        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int managerId)
    {
        Manager? manager = await dbContext.Managers.SingleOrDefaultAsync(x => x.Id == managerId);

        if (manager is null)
            throw new EntityNotFoundException(nameof(Manager), managerId);

        dbContext.Managers.Remove(manager);

        await dbContext.SaveChangesAsync();
    }
}