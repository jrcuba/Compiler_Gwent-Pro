using System.Collections.Generic;
public class Context
{
    public List<Card> CartasEneltablero = new List<Card>();

    public Player TriggerPlayer(Card card)
    {
        return card.player;
    }
}