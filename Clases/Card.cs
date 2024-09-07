using System.Collections.Generic;
using UnityEngine;
public class Card : MonoBehaviour
{
    public Sprite Image;
    public string PlayerAlQuePertenece { get; set; }
    public string SpecialType { get;set; }//Gold o Silver
    public string Type { get; set; }//Effect o Monster o Líder
    public string CardName { get; set; }
    public string Faction { get; set; }
    public double Power { get; set; }
    public List<string> Range {  get; set; }
    public List<Token> OnActivationTokens { get; set; }
    public string EffectName { get; set; }
    public Player player { get; set; }
    public static Card CreateCard(GameObject parent, string name, double power, string type, string faction, List<string> range, Player player,string EffectName)
    {
        Card newCard = parent.AddComponent<Card>();
        newCard.CardName = name;
        newCard.Power = power;
        newCard.Type = type;
        newCard.Faction = faction;
        newCard.Range = range;
        newCard.player = player;
        newCard.EffectName = EffectName;
        newCard.OnActivationTokens = new List<Token>();
        return newCard;
    }

    public void Inicializate()
    {
        EffectName = "";
        SpecialType = "";
        Type = "";
        CardName = "";
        Faction = "";
        PlayerAlQuePertenece = "";
        Power = 0;
        Range = new List<string>();
        player = new Player(new List<Card>(), "");
        player.Id = PlayerAlQuePertenece;
        OnActivationTokens = new List<Token>();
    }
    public string Owner()
    {
        return player.Id;
    }
}