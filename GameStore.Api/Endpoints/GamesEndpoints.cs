using GameStore.Api.Authorization;
using GameStore.Api.Dtos;
using GameStore.Api.Entities;
using GameStore.Api.Repositories;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
    const string GetGameV1EndpointName = "GetGameV1";
    const string GetGameV2EndpointName = "GetGameV2";

    public static RouteGroupBuilder MapGamesEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.NewVersionedApi()
                .MapGroup("/games")
                .HasApiVersion(1.0)
                .HasApiVersion(2.0)
                .WithParameterValidation();

        // V1 GET ENDPOINTS
        group.MapGet("/", async (IGamesRepository repository, ILoggerFactory loggerFactory) =>
        {
            return Results.Ok((await repository.GetAllAsync()).Select(game => game.AsDtoV1()));
        })
        .MapToApiVersion(1.0);

        group.MapGet("/{id}", async (IGamesRepository repository, int id) =>
        {
            Game? game = await repository.GetAsync(id);
            return game is not null ? Results.Ok(game.AsDtoV1()) : Results.NotFound();
        })
        .WithName(GetGameV1EndpointName)
        .RequireAuthorization(Policies.ReadAccess)
        .MapToApiVersion(1.0);

        // V2 GET ENDPOINTS
        group.MapGet("/", async (IGamesRepository repository, ILoggerFactory loggerFactory) =>
        {
            return Results.Ok((await repository.GetAllAsync()).Select(game => game.AsDtoV2()));
        })
        .MapToApiVersion(2.0);

        group.MapGet("/{id}", async (IGamesRepository repository, int id) =>
        {
            Game? game = await repository.GetAsync(id);
            return game is not null ? Results.Ok(game.AsDtoV2()) : Results.NotFound();
        })
        .WithName(GetGameV2EndpointName)
        .RequireAuthorization(Policies.ReadAccess)
        .MapToApiVersion(2.0);

        //-----------------------------------------------------------------------------------------------------

        group.MapPost("/", async (IGamesRepository repository, CreateGameDto gameDto) =>
        {
            Game game = new()
            {
                Name = gameDto.Name,
                Genre = gameDto.Genre,
                Price = gameDto.Price,
                ReleaseDate = gameDto.ReleaseDate,
                ImageUri = gameDto.ImageUri
            };

            await repository.CreateAsync(game);
            return Results.CreatedAtRoute(GetGameV1EndpointName, new { id = game.Id }, game);
        })
        .RequireAuthorization(Policies.WriteAccess)
        .MapToApiVersion(1.0);

        group.MapPut("/{id}", async (IGamesRepository repository, int id, UpdateGameDto updateGameDto) =>
        {
            Game? existingGame = await repository.GetAsync(id);

            if (existingGame is null)
            {
                return Results.NotFound();
            }

            existingGame.Name = updateGameDto.Name;
            existingGame.Genre = updateGameDto.Genre;
            existingGame.Price = updateGameDto.Price;
            existingGame.ReleaseDate = updateGameDto.ReleaseDate;
            existingGame.ImageUri = updateGameDto.ImageUri;

            await repository.UpdateAsync(existingGame);

            return Results.NoContent();
        })
        .RequireAuthorization(Policies.WriteAccess)
        .MapToApiVersion(1.0);

        group.MapDelete("/{id}", async (IGamesRepository repository, int id) =>
        {
            Game? game = await repository.GetAsync(id);

            if (game is not null)
            {
                await repository.DeleteAsync(game.Id);
            }

            return Results.NoContent();
        })
        .RequireAuthorization(Policies.WriteAccess)
        .MapToApiVersion(1.0);

        return group;
    }
}