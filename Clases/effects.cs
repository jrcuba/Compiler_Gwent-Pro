using System.Collections.Generic;
public class Effect
{

    public string Name { get; set; }
    public List<Params> Params = new List<Params>();
    public List<Token> tokens = new List<Token>();
    public List<object> targets = new List<object>();
    public object context = new object();
    public Effect()
    {
        Name = "";
    }
    public Effect(string Name)
    {
        this.Name = Name;
    }

    //m�todos de efectos en espec�fico para ciertas cartas
    public void Draw(Player player)
    {
        player.Hand.Add(player.DeckOfPlayer[player.DeckOfPlayer.Count - 1]);
        player.DeckOfPlayer.RemoveAt(player.DeckOfPlayer.Count - 1);
    }
}