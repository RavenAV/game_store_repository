using System.Net.Http.Json;
using GameStore.Client.Models;

namespace GameStore.Client.Clients;

public class AuthenticatedClient
{
    private readonly HttpClient httpClient;

    public AuthenticatedClient(HttpClient httpClient) => this.httpClient = httpClient;

    public async Task AddGameAsync(Game game) => await httpClient.PostAsJsonAsync("games", game);

    public async Task<Game> GetGameAsync(int id) => await httpClient.GetFromJsonAsync<Game>($"games/{id}")
                                                    ?? throw new Exception("Could not find game!");

    public async Task UpdateGameAsync(Game updatedGame) => await httpClient.PutAsJsonAsync($"games/{updatedGame.Id}", updatedGame);

    public async Task DeleteGameAsync(int id) => await httpClient.DeleteAsync($"games/{id}");
}