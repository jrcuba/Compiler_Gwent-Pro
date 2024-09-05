using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class EffectsNoCompilables : MonoBehaviour
{
    public static bool PutIncreaseBool = false;
    // Efecto de poner un efecto en una fila propia
    private int k;
    public void PutIncrease(Card card, Player player)
    {
        //reajustaré el efecto a invocar una carta de efecto de manera aleatoria
        SummonScript.count++;
        PutIncreaseBool = true;
    }
    //eliminar la carta con más poder en el campo (Propio o rival ,da igual)
    public void DeleteMostPowerfullCard(ref List<GameObject> PrefabsCards,List<Card> cards)
    {
        //busco la carta con mas poder
        GameObject aux = PrefabsCards[0];
        Card cardaux = aux.GetComponent<Card>();
        foreach (GameObject Prefab in PrefabsCards)
        {
            Card card = Prefab.GetComponent<Card>();
            if (card.Power > cardaux.Power)
            {
                cardaux.Power = cardaux.Power;
                aux = Prefab;
            }
        }
        PrefabsCards.Remove(aux);
        Destroy(aux);
    }
    //eliminar la carta con menos poder del rival
    public void DeleteMostWeekCard(ref List<GameObject> PrefabsCards,List<Card> cards)
    {
        GameObject aux = PrefabsCards[0];
        Card cardaux = aux.GetComponent<Card>();
        foreach (GameObject Prefab in PrefabsCards)
        {
            Card card = Prefab.GetComponent<Card>();
            if (card.Power < cardaux.Power)
            {
                cardaux.Power = cardaux.Power;
                aux = Prefab;
            }
        }
        cards.Remove(cardaux);
        PrefabsCards.Remove(aux);
        Destroy(aux);
    }
    //robar una carta ,ya está hecho
    //Multiplicar por n su ataque ,siendo n la cantidad de cartas iguales a ella en el campo
    public void GetScalarAtack(List<Card> Cards, ref Card card)
    {
        foreach (Card @object in Cards)
        {
            Card card1 = @object;
            if (card1.Name == card.Name && card1 != card)
            {
                card.Power += card1.Power;
            }
        }
    }

    //limpiar la fila o campo no vacía propia o del rival 
    public void DeleteFile(ref List<GameObject> Melee1,ref List<GameObject> Ranged1,ref List<GameObject> Siege1,ref List<GameObject> Melee2,ref List<GameObject> Ranged2,ref List<GameObject> Siege2)
    {
        // Crear un diccionario para almacenar las listas y sus tamaños
        Dictionary<List<GameObject>, int> listSizes = new Dictionary<List<GameObject>, int>
    {
        { Melee1, Melee1.Count },
        { Ranged1, Ranged1.Count },
        { Siege1, Siege1.Count },
        { Melee2, Melee2.Count },
        { Ranged2, Ranged2.Count },
        { Siege2, Siege2.Count }
    };

        // Encontrar la lista con el menor tamaño
        List<GameObject> smallestList = null;
        int smallestSize = int.MaxValue;

        foreach (var entry in listSizes)
        {
            if (entry.Value < smallestSize)
            {
                smallestSize = entry.Value;
                smallestList = entry.Key;
            }
        }

        // Destruir los objetos de la lista con el menor tamaño
        if (smallestList != null)
        {
            foreach (GameObject obj in smallestList)
            {
                Destroy(obj);
            }
            smallestList.Clear();
        }
    }
    //calcular el promedio de poder y darselo a todas las cartas
    public void RestartPower(List<GameObject> Field, List<Card> CardsOnField)
    {
        double power = 0;
        int count = 0;

        // Calcular el poder total y el número de cartas
        foreach (GameObject gameObject in Field)
        {
            Card card = gameObject.GetComponent<Card>();
            power += card.Power;
            count++;
        }

        // Calcular el promedio de poder
        double averagePower = power / count;

        // Asignar el promedio de poder a todas las cartas en el campo
        foreach (GameObject gameObject1 in Field)
        {
            Card card = gameObject1.GetComponent<Card>();
            card.Power = averagePower;
        }

        // Asignar el promedio de poder a todas las cartas en la lista de cartas en el campo
        foreach (Card card1 in CardsOnField)
        {
            card1.Power = averagePower;
        }
    }



    //método para invocar al efecto de la carta
    public void InvocateEffect(Card card)
    {
        if (card.PlayerAlQuePertenece == SelectDeckScript.players[0].Id) 
        {
            InvocarEfecto(card, 0);
        }
        else
        {
            InvocarEfecto(card, 1);
        }
    }
    public void InvocarEfecto(Card card,int k)
    {
        OnActivationParser onActivationParser = new OnActivationParser();
        //tengo que hacer mi lista de efectos ,por ahora la voy a dejar como new
        List<Effect> effects = new List<Effect>();
        switch (card.EffectName)
        {
            case "PutIncrease":
                PutIncrease(card, SelectDeckScript.players[k]);
                break;
            case "DeleteMostPowerfullCard":
                List<Card> InvoquedCards = SummonScript.InvoquedCardsPlayer1;
                foreach (Card cartica in SummonScript.InvoquedCardsPlayer2)
                {
                    InvoquedCards.Add(cartica);
                }
                DeleteMostPowerfullCard(ref SummonScript.InvoquedCardsObjects,InvoquedCards);
                break;
            case "DeleteMostWeekCard":
                if (k == 0)
                {
                    //Arreglar esto
                    DeleteMostWeekCard(ref SummonScript.InvoquedCardsObjects,SummonScript.InvoquedCards);
                }
                break;
            case "GetScalarAtack":
                if (k == 0)
                {
                    GetScalarAtack(SummonScript.InvoquedCardsPlayer1,ref card);
                }
                else
                {
                    GetScalarAtack(SummonScript.InvoquedCardsPlayer2, ref card);
                }
                break;
            case "DeleteFile":
                //Arreglar esto ,solo elimina lo visual ,no el backend
                DeleteFile(ref SummonScript.CardsOnRangedPlayer1Object,ref SummonScript.CardsOnRangedPlayer1Object,ref SummonScript.CardsOnSiegePlayer1Object,ref SummonScript.CardsOnMeleePlayer2Object,ref SummonScript.CardsOnRangedPlayer2Object,ref SummonScript.CardsOnSiegePlayer2Object);
                break;
            case "RestartPower":
                List<Card> InvoquedCards2 = SummonScript.InvoquedCardsPlayer1;
                foreach (Card cartica in SummonScript.InvoquedCardsPlayer2)
                {
                    InvoquedCards2.Add(cartica);
                }
                RestartPower(SummonScript.InvoquedCardsObjects,InvoquedCards2);
                break;
            default:
                onActivationParser.OnActivation(card.OnActivationTokens,Compilar.effects,card,ref Compilar.exceptions,0);
                Context context = new Context();
                if (onActivationParser.actionTokens.Count != 0)
                {
                    Program.actionParser.ActionParser_(card, onActivationParser.actionTokens, onActivationParser.Targets, context,ref onActivationParser.effect.Params, 0,ref Program.exceptions);
                }
                break;
        }
    }
}