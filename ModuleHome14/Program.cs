using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class Karta
{
    public string Mast { get; }
    public string Tip { get; }

    public Karta(string mast, string tip)
    {
        Mast = mast;
        Tip = tip;
    }
}

class Player
{
    public string Name { get; }
    public List<Karta> Cards { get; } = new List<Karta>();

    public Player(string name)
    {
        Name = name;
    }

    public void DisplayCards()
    {
        Console.WriteLine($"{Name}'s cards: {string.Join(", ", Cards.Select(karta => $"{karta.Tip} {karta.Mast}"))}");
    }
}

class Game
{
    private List<Player> players = new List<Player>();
    private List<Karta> deck = new List<Karta>();

    public Game(List<Player> players)
    {
        this.players = players;
        InitializeDeck();
        ShuffleDeck();
        DealCards();
    }

    private void InitializeDeck()
    {
        string[] masts = { "Hearts", "Diamonds", "Clubs", "Spades" };
        string[] tips = { "6", "7", "8", "9", "10", "Jack", "Queen", "King", "Ace" };

        deck = (from mast in masts
                from tip in tips
                select new Karta(mast, tip)).ToList();
    }

    private void ShuffleDeck()
    {
        Random random = new Random();
        deck = deck.OrderBy(x => random.Next()).ToList();
    }

    private void DealCards()
    {
        int cardsPerPlayer = deck.Count / players.Count;
        foreach (var player in players)
        {
            player.Cards.AddRange(deck.Take(cardsPerPlayer));
            deck.RemoveRange(0, cardsPerPlayer);
        }
    }

    private void PlayRound()
    {
        List<Karta> roundCards = players.Select(player => player.Cards[0]).ToList();
        Karta maxCard = roundCards.OrderByDescending(karta => Array.IndexOf(new[] { "6", "7", "8", "9", "10", "Jack", "Queen", "King", "Ace" }, karta.Tip)).ThenBy(karta => Array.IndexOf(new[] { "Hearts", "Diamonds", "Clubs", "Spades" }, karta.Mast)).First();

        Console.WriteLine($"{maxCard.Tip} {maxCard.Mast} wins the round!");
        players.First(player => player.Cards.Contains(maxCard)).Cards.AddRange(roundCards);
        roundCards.ForEach(card => players.ForEach(player => player.Cards.Remove(card)));
    }

    public void PlayGame()
    {
        while (players.All(player => player.Cards.Any()))
        {
            PlayRound();
        }

        Player winner = players.OrderByDescending(player => player.Cards.Count).First();
        Console.WriteLine($"{winner.Name} wins the game!");
    }
}

class Program
{
    static void Main()
    {
        Player player1 = new Player("Player 1");
        Player player2 = new Player("Player 2");

        Game game = new Game(new List<Player> { player1, player2 });
        game.PlayGame();
    }
}
