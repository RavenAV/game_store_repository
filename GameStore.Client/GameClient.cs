using System.Net.Sockets;
using GameStore.Client.Models;
using System.Net.Http.Json;

namespace GameStore.Client;

public class GameClient
{
    private readonly HttpClient httpClient;

    /*private readonly List<Game> games = new()
    {
        new Game()
        {
            Id = 1,
            Name = "Street Fighter II",
            Genre = "Fighting",
            Price = 19.99M,
            ReleaseDate = new DateTime(1991, 2, 1)
        },
        new Game()
        {
            Id = 2,
            Name = "Final Fantasy XVI",
            Genre = "Roleplaying",
            Price = 59.99M,
            ReleaseDate = new DateTime(2010, 9, 30)
        },
        new Game()
        {
            Id = 3,
            Name = "FIFA 23",
            Genre = "Sports",
            Price = 69.99M,
            ReleaseDate = new DateTime(2022, 9, 27)
        }
    };*/

    public GameClient(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<Game[]?> GetGamesAsync() => await httpClient.GetFromJsonAsync<Game[]>("games");

    public async Task AddGameAsync(Game game) => await httpClient.PostAsJsonAsync("games", game);

    public async Task<Game> GetGameAsync(int id) => await httpClient.GetFromJsonAsync<Game>($"games/{id}")
                                              ?? throw new Exception("Could not find game!");

    public async Task UpdateGameAsync(Game updatedGame) => await httpClient.PutAsJsonAsync($"games/{updatedGame.Id}", updatedGame);

    public async Task DeleteGameAsync(int id) => await httpClient.DeleteAsync($"games/{id}");
}