using GameStore.Api.Entities;

namespace GameStore.Api.Repositories;

public interface IGamesRepository
{
    Task CreateAsync(Game game);
    Task DeleteAsync(int id);
    Task<Game?> GetAsync(int id);
    Task<IEnumerable<Game>> GetAllAsync(int pageNumber, int pageSize, string? filter);
    Task UpdateAsync(Game updatedGame);
    Task<int> CountAsync(string? filter);
}
