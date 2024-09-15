using System.Collections.Generic;
using UnityEngine;

//Una base de datos b�sica para probar mi juego 
public class LoadDataBase : MonoBehaviour
{
    public static Dictionary<string, List<Card>> Mazos = new Dictionary<string, List<Card>>();
    private static int count = 0;
    private static int count2 = 0;
    void Start()
    {
        if (count == 0)
        {
            Mazos = LoadDataBase_();
            count++;
        }
    }

    public Dictionary<string, List<Card>> LoadDataBase_()
    {
        GameObject cardParent = new GameObject("CardParent");

        List<Card> deck1Cards = new List<Card>();
        List<Card> deck2Cards = new List<Card>();

        // Definir tipos de cartas para Humanos
        string[] humanTypes = { "Guerrero", "Mago", "Arquero", "Caballero", "Hechicero", "Lancero", "Esp�a", "Asesino", "Mercenario", "Sacerdote", "Berserker", "Alquimista", "Explorador", "L�der" };

        // Crear cartas de Humanos
        foreach (string type in humanTypes)
        {
            int instances = GetInstanceCount(type, "Humanos");
            for (int i = 0; i < instances; i++)
            {
                if (count2 % 4 != 0)
                {
                    Card newCard = Card.CreateCard(cardParent, type, Random.Range(1, 10), "Monster", "Humans", new List<string> { "Melee","Ranged","Siege" }, new Player(new List<Card>(), type + i), "");
                    newCard.Image = LoadCardImage(type);
                    AssignEffectName(newCard);
                    deck1Cards.Add(newCard);
                }
                else
                {
                    Card newCard = Card.CreateCard(cardParent, type, 5, "Effect", "Humans", new List<string> { "Melee", "Ranged", "Siege" }, new Player(new List<Card>(), type + i), "");
                    newCard.Image = LoadCardImage(type);
                    AssignEffectName(newCard);
                    deck1Cards.Add(newCard);
                }
                count2++;
            }
        }

        // Definir tipos de cartas para Orcos
        string[] orcTypes = { "Orco", "Orco Cham�n", "Orco Guerrero", "Orco Berserker", "Orco Cazador", "Orco Jinete", "Orco Brujo", "Orco Asesino", "Orco Esp�a", "Orco Mercenario", "Orco Sacerdote", "Orco Alquimista", "Orco Explorador", "Orco L�der" };

        // Crear cartas de Orcos
        foreach (string type in orcTypes)
        {
            int instances = GetInstanceCount(type, "Orcos");
            for (int i = 0; i < instances; i++)
            {
                if (count2 % 4 != 0)
                {
                    Card newCard = Card.CreateCard(cardParent, type, Random.Range(1, 10), "Monster","Orcos", new List<string> { "Melee","Ranged","Siege" }, new Player(new List<Card>(), type + i), "");
                    newCard.Image = LoadCardImage(type);
                    AssignEffectName(newCard);
                    deck2Cards.Add(newCard);
                }
                else
                {
                    Card newCard = Card.CreateCard(cardParent, type, 5, "Effect", "Orcos", new List<string> { "Melee", "Ranged", "Siege" }, new Player(new List<Card>(), type + i), "");
                    newCard.Image = LoadCardImage(type);
                    AssignEffectName(newCard);
                    deck2Cards.Add(newCard);
                }
                count2++;
            }
        }

        Mazos.Add("Mazo de Humanos", deck1Cards);
        Mazos.Add("Mazo de Orcos", deck2Cards);

        return Mazos;
    }

    private int GetInstanceCount(string type, string faction)
    {
        if (faction == "Humanos" && (type == "Guerrero" || type == "Mago" || type == "Berserker"))
        {
            return 1;
        }
        if (faction == "Orcos" && (type == "Orco Berserker" || type == "Orco Cham�n" || type == "Orco Guerrero"))
        {
            return 1;
        }
        return 3;
    }

    private Sprite LoadCardImage(string cardName)
    {
        switch (cardName)
        {
            case "Guerrero":
                return Resources.Load<Sprite>($"Images/{"guerrero"}");
            case "Mago":
                return Resources.Load<Sprite>($"Images/{"Mago"}");
            case "Arquero":
                return Resources.Load<Sprite>($"Images/{"Archer"}");
            case "Caballero":
                return Resources.Load<Sprite>($"Images/{"Caballero"}");
            case "Hechicero":
                return Resources.Load<Sprite>($"Images/{"hechicero"}");
            case "Lancero":
                return Resources.Load<Sprite>($"Images/{"Lancero"}");
            case "Esp�a":
                return Resources.Load<Sprite>($"Images/{"Esp�a"}");
            case "Asesino":
                return Resources.Load<Sprite>($"Images/{"asesino"}");
            case "Mercenario":
                return Resources.Load<Sprite>($"Images/{"mercenario"}");
            case "Sacerdote":
                return Resources.Load<Sprite>($"Images/{"sacerdote"}");
            case "Berserker":
                return Resources.Load<Sprite>($"Images/{"berserker"}");
            case "Alquimista":
                return Resources.Load<Sprite>($"Images/{"alquimista"}");
            case "Explorador":
                return Resources.Load<Sprite>($"Images/{"explorador"}");
            case "L�der":
                return Resources.Load<Sprite>($"Images/{"Lider Humano"}");
            case "Orco":
                return Resources.Load<Sprite>($"Images/{"orco"}");
            case "Orco Cham�n":
                return Resources.Load<Sprite>($"Images/{"orco_chaman"}");
            case "Orco Guerrero":
                return Resources.Load<Sprite>($"Images/{"orco_guerrero"}");
            case "Orco Berserker":
                return Resources.Load<Sprite>($"Images/{"orco_berserker"}");
            case "Orco Cazador":
                return Resources.Load<Sprite>($"Images/{"orco_cazador"}");
            case "Orco Jinete":
                return Resources.Load<Sprite>($"Images/{"orco_jinete"}");
            case "Orco Brujo":
                return Resources.Load<Sprite>($"Images/{"orco_brujo"}");
            case "Orco Asesino":
                return Resources.Load<Sprite>($"Images/{"orco_asesino"}");
            case "Orco Esp�a":
                return Resources.Load<Sprite>($"Images/{"orco_espia"}");
            case "Orco Mercenario":
                return Resources.Load<Sprite>($"Images/{"orco_mercenario"}");
            case "Orco Sacerdote":
                return Resources.Load<Sprite>($"Images/{"orco_sacerdote"}");
            case "Orco Alquimista":
                return Resources.Load<Sprite>($"Images/{"orco_alquimista"}");
            case "Orco Explorador":
                return Resources.Load<Sprite>($"Images/{"orco_explorador"}");
            case "Orco L�der":
                return Resources.Load<Sprite>($"Images/{"orco_lider"}");
            default:
                return Resources.Load<Sprite>($"Images/{"default"}");
        }
    }

    private void AssignEffectName(Card card)
    {
        List<string> effects = new List<string>
    {
        "PutIncrease",
        "DeleteMostPowerfullCard",
        "DeleteMostWeakCard",
        "GetScalarAttack",
        "DeleteFile",
        "RestartPower"
    };
        switch (card.CardName)
        {
            case "Mago":
                card.EffectName = effects[UnityEngine.Random.Range(0, effects.Count)];
                break;
            case "Guerrero":
                card.EffectName = effects[UnityEngine.Random.Range(0, effects.Count)];
                break;
            case "Arquero":
                card.EffectName = effects[UnityEngine.Random.Range(0, effects.Count)];
                break;
            case "Caballero":
                card.EffectName = effects[UnityEngine.Random.Range(0, effects.Count)];
                break;
            case "Hechicero":
                card.EffectName = effects[UnityEngine.Random.Range(0, effects.Count)];
                break;
            case "Lancero":
                card.EffectName = effects[UnityEngine.Random.Range(0, effects.Count)];
                break;
            case "Esp�a":
                card.EffectName = effects[UnityEngine.Random.Range(0, effects.Count)];
                break;
            case "Asesino":
                card.EffectName = effects[UnityEngine.Random.Range(0, effects.Count)];
                break;
            case "Mercenario":
                card.EffectName = effects[UnityEngine.Random.Range(0, effects.Count)];
                break;
            case "Sacerdote":
                card.EffectName = effects[UnityEngine.Random.Range(0, effects.Count)];
                break;
            case "Berserker":
                card.EffectName = effects[UnityEngine.Random.Range(0, effects.Count)];
                break;
            case "Alquimista":
                card.EffectName = effects[UnityEngine.Random.Range(0, effects.Count)];
                break;
            case "Explorador":
                card.EffectName = effects[UnityEngine.Random.Range(0, effects.Count)];
                break;
            case "L�der":
                card.EffectName = effects[UnityEngine.Random.Range(0, effects.Count)];
                break;
            case "Orco":
                card.EffectName = effects[UnityEngine.Random.Range(0, effects.Count)];
                break;
            case "Orco Cham�n":
                card.EffectName = effects[UnityEngine.Random.Range(0, effects.Count)];
                break;
            case "Orco Guerrero":
                card.EffectName = effects[UnityEngine.Random.Range(0, effects.Count)];
                break;
            case "Orco Berserker":
                card.EffectName = effects[UnityEngine.Random.Range(0, effects.Count)];
                break;
            case "Orco Cazador":
                card.EffectName = effects[UnityEngine.Random.Range(0, effects.Count)];
                break;
            case "Orco Jinete":
                card.EffectName = effects[UnityEngine.Random.Range(0, effects.Count)];
                break;
            case "Orco Brujo":
                card.EffectName = effects[UnityEngine.Random.Range(0, effects.Count)];
                break;
            case "Orco Asesino":
                card.EffectName = effects[UnityEngine.Random.Range(0, effects.Count)];
                break;
            case "Orco Esp�a":
                card.EffectName = effects[UnityEngine.Random.Range(0, effects.Count)];
                break;
            case "Orco Mercenario":
                card.EffectName = effects[UnityEngine.Random.Range(0, effects.Count)];
                break;
            case "Orco Sacerdote":
                card.EffectName = effects[UnityEngine.Random.Range(0, effects.Count)];
                break;
            case "Orco Alquimista":
                card.EffectName = effects[UnityEngine.Random.Range(0, effects.Count)];
                break;
            case "Orco Explorador":
                card.EffectName = effects[UnityEngine.Random.Range(0, effects.Count)];
                break;
            case "Orco L�der":
                card.EffectName = effects[UnityEngine.Random.Range(0, effects.Count)];
                break;
            default:
                card.EffectName = "Candela";
                break;
        }
    }
}
