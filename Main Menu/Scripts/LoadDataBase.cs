using System.Collections.Generic;
using UnityEngine;

public class LoadDataBase : MonoBehaviour
{
    public static Dictionary<string, List<Card>> Mazos = new Dictionary<string, List<Card>>();
    private static int count = 0;

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
        string[] humanTypes = { "Guerrero", "Mago", "Arquero", "Caballero", "Hechicero", "Lancero", "Espía", "Asesino", "Mercenario", "Sacerdote", "Berserker", "Alquimista", "Explorador", "Líder" };

        // Crear cartas de Humanos
        foreach (string type in humanTypes)
        {
            int instances = GetInstanceCount(type, "Humanos");
            for (int i = 0; i < instances; i++)
            {
                if ( (i + 1) % 2 == 0)
                {
                    Card newCard = Card.CreateCard(cardParent, "Silver", Random.Range(1, 10), "Monster", type, new List<string> { "Melee","Ranged","Siege" }, new Player(new List<Card>(), type + i), "");
                    newCard.Image = LoadCardImage(type);
                    AssignEffectName(newCard);
                    deck1Cards.Add(newCard);
                }
                else
                {
                    Card newCard = Card.CreateCard(cardParent, "Silver", Random.Range(1, 10), "Effect", type, new List<string> { "Melee", "Ranged", "Siege" }, new Player(new List<Card>(), type + i), "");
                    newCard.Image = LoadCardImage(type);
                    AssignEffectName(newCard);
                    deck1Cards.Add(newCard);
                }
            }
        }

        // Definir tipos de cartas para Orcos
        string[] orcTypes = { "Orco", "Orco Chamán", "Orco Guerrero", "Orco Berserker", "Orco Cazador", "Orco Jinete", "Orco Brujo", "Orco Asesino", "Orco Espía", "Orco Mercenario", "Orco Sacerdote", "Orco Alquimista", "Orco Explorador", "Orco Líder" };

        // Crear cartas de Orcos
        foreach (string type in orcTypes)
        {
            int instances = GetInstanceCount(type, "Orcos");
            for (int i = 0; i < instances; i++)
            {
                if ( (i + 1) % 2 == 0)
                {
                    Card newCard = Card.CreateCard(cardParent, "Silver", Random.Range(1, 10), "Monster", type, new List<string> { "Melee","Ranged","Siege" }, new Player(new List<Card>(), type + i), "");
                    newCard.Image = LoadCardImage(type);
                    AssignEffectName(newCard);
                    deck2Cards.Add(newCard);
                }
                else
                {
                    Card newCard = Card.CreateCard(cardParent, "Silver", Random.Range(1, 10), "Effect", type, new List<string> { "Melee", "Ranged", "Siege" }, new Player(new List<Card>(), type + i), "");
                    newCard.Image = LoadCardImage(type);
                    AssignEffectName(newCard);
                    deck2Cards.Add(newCard);
                }
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
        if (faction == "Orcos" && (type == "Orco Berserker" || type == "Orco Chamán" || type == "Orco Guerrero"))
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
            case "Espía":
                return Resources.Load<Sprite>($"Images/{"Espía"}");
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
            case "Líder":
                return Resources.Load<Sprite>($"Images/{"Lider Humano"}");
            case "Orco":
                return Resources.Load<Sprite>($"Images/{"orco"}");
            case "Orco Chamán":
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
            case "Orco Espía":
                return Resources.Load<Sprite>($"Images/{"orco_espia"}");
            case "Orco Mercenario":
                return Resources.Load<Sprite>($"Images/{"orco_mercenario"}");
            case "Orco Sacerdote":
                return Resources.Load<Sprite>($"Images/{"orco_sacerdote"}");
            case "Orco Alquimista":
                return Resources.Load<Sprite>($"Images/{"orco_alquimista"}");
            case "Orco Explorador":
                return Resources.Load<Sprite>($"Images/{"orco_explorador"}");
            case "Orco Líder":
                return Resources.Load<Sprite>($"Images/{"orco_lider"}");
            default:
                return Resources.Load<Sprite>($"Images/{"default"}");
        }
    }

    private void AssignEffectName(Card card)
    {
        //aquí le voy a dar los poderes a las cartas
        switch (card.Type)
        {
            case "Mago":
                card.EffectName = "";
                break;
            case "Guerrero":
                card.EffectName = "";
                break;
            case "Arquero":
                card.EffectName = "";
                break;
            case "Caballero":
                card.EffectName = "";
                break;
            // Agrega más casos según sea necesario
            default:
                card.EffectName = "";
                break;
        }
    }
}
