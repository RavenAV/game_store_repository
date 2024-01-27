namespace GameStore.Client.Models;

public class PagingInfo
{
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }

    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;
}