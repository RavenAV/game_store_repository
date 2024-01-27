using System.Text.Json;
using GameStore.Client.Models;

namespace GameStore.Client.Clients;

public class NotAuthenticatedClient
{
    private const int pageSize = 5;

    private readonly HttpClient httpClient;
    private readonly JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };

    public NotAuthenticatedClient(HttpClient httpClient) => this.httpClient = httpClient;

    public async Task<PagedResponse<Game>> GetGamesAsync(int pageNumber, string? filter)
    {
        var responseMessage = await httpClient.GetAsync($"games?pageNumber={pageNumber}&pageSize={pageSize}&filter={filter}");
        var content = await responseMessage.Content.ReadAsStringAsync();

        if (!responseMessage.IsSuccessStatusCode)
        {
            throw new ApplicationException(content);
        }

        responseMessage.Headers.TryGetValues("X-Pagination", out var headers);
        var paginationHeader = headers?.FirstOrDefault();

        var pagedResponse = new PagedResponse<Game>
        {
            Items = JsonSerializer.Deserialize<List<Game>>(content, options) ?? new List<Game>(),
            PagingInfo = paginationHeader is not null 
                ? JsonSerializer.Deserialize<PagingInfo>(paginationHeader, options) ?? new PagingInfo() 
                : new PagingInfo()
        };

        pagedResponse.PagingInfo.CurrentPage = pageNumber;

        return pagedResponse;
    }
}