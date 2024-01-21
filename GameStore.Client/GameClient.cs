using System.Net.Sockets;
using GameStore.Client.Models;

namespace GameStore.Client;

public class GameClient
{
    private static readonly List<Game> games = new()
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
    };

    public static Game[] GetGames() => games.ToArray();

    public static void AddGame(Game game)
    {
        game.Id = games.Max(g => g.Id) + 1;
        games.Add(game);
    }

    public static Game GetGame(int id) => games.Find(g => g.Id == id) ?? throw new Exception("Could not find game!");

    public static void UpdateGame(Game updatedGame)
    {
        Game existingGame = GetGame(updatedGame.Id);

        existingGame.Name = updatedGame.Name;
        existingGame.Genre = updatedGame.Genre;
        existingGame.Price = updatedGame.Price;
        existingGame.ReleaseDate = updatedGame.ReleaseDate;
    }
}