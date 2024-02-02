using GameStore.Api.Data;
using GameStore.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Repositories;

public class EntityFrameworkGamesRepository : IGamesRepository
{
    private readonly GameStoreContext dbContext;
    private readonly ILogger<EntityFrameworkGamesRepository> logger;

    public EntityFrameworkGamesRepository(
        GameStoreContext dbContext,
        ILogger<EntityFrameworkGamesRepository> logger)
    {
        this.dbContext = dbContext;
        this.logger = logger;
    }

    public async Task<IEnumerable<Game>> GetAllAsync(int pageNumber, int pageSize, string? filter)
    {
        var skipCount = (pageNumber - 1) * pageSize;

        return await FilterGames(filter)
                        .OrderBy(game => game.Id)
                        .Skip(skipCount)
                        .Take(pageSize)
                        .AsNoTracking().ToListAsync();
    }

    public async Task<Game?> GetAsync(int id)
    {
        return await dbContext.Games.FindAsync(id);
    }

    public async Task CreateAsync(Game game)
    {
        dbContext.Games.Add(game);
        await dbContext.SaveChangesAsync();

        logger.LogInformation("Created game {Name} with price {Price}.", game.Name, game.Price);
    }

    public async Task UpdateAsync(Game updatedGame)
    {
        dbContext.Update(updatedGame);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await dbContext.Games.Where(game => game.Id == id)
            .ExecuteDeleteAsync();
    }

    public async Task<int> CountAsync(string? filter)
    {
        return await FilterGames(filter).CountAsync();
    }

    private IQueryable<Game> FilterGames(string? filter)
    {
        if (string.IsNullOrEmpty(filter))
        {
            return dbContext.Games;
        }

        return dbContext.Games
                        .Where(game => game.Name.Contains(filter) || game.Genre.Contains(filter));
    }
}