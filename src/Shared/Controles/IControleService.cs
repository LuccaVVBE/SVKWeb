using Svk.Shared.Misc;

namespace Svk.Shared.Controles;

public interface IControleService
{
    Task<Result.GetItemsPaginated<ControleDto.Index>> GetIndexAsync(ControleRequest.Index req);
    Task<ControleDto.Detail> GetDetailAsync(int routenummer);
    Task<ControleResult.Create> CreateAsync(ControleDto.Create model);
    Task<ControleResult.Edit> EditAsync(int controleId, ControleDto.Edit model);
}