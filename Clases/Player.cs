using System.Collections.Generic;
public class Player
{
    public string Id;
    public List<Card> Hand = new List<Card>();
    public List<Card> CardsOnTable = new List<Card>();
    public List<Card> GraveyardOfPlayer = new List<Card>();
    public List<Card> DeckOfPlayer;
    public List<Card> CardsOnField;
    public Player(List<Card> DeckOfPlayer, string Id)
    {
        this.DeckOfPlayer = DeckOfPlayer;
        this.Id = Id;
    }
}


//TODO: hacer luego el context.hand y el predicate 