using System.Net.Http.Json;
using GameStore.Client.Models;

namespace GameStore.Client.Clients;

public class ImagesClient
{
    private readonly HttpClient httpClient;

    public ImagesClient(HttpClient httpClient) => this.httpClient = httpClient;

    public async Task<string> UploadImageAsync(MultipartFormDataContent image)
    {
        var response = await httpClient.PostAsync("images", image);
        var imageUpload = await response.Content.ReadFromJsonAsync<ImageUploadResponse>() ?? throw new Exception("Could not upload image!");
        return imageUpload.BlobUri;
    }
}