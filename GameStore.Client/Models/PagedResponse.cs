namespace GameStore.Client.Models;

public class PagedResponse<T> where T : class
{
    public required List<T> Items { get; set; }
    public required PagingInfo PagingInfo { get; set; }
}