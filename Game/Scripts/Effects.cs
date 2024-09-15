using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class EffectsNoCompilables : MonoBehaviour
{
    public GameObject ActionParser;
    public static bool PutIncreaseBool = false;
    // Efecto de poner un Aumento
    private int k;
    public void PutIncrease(Card card, Player player)
    {
        SummonScript.count++;
        PutIncreaseBool = true;
    }
    //eliminar la carta con más poder en el campo (Propio o rival ,da igual)
    public void DeleteMostPowerfullCard(ref List<GameObject> PrefabsCards,List<Card> cards)
    {
        //busco la carta con mas poder
        GameObject aux = PrefabsCards[0];
        foreach (GameObject Prefab in PrefabsCards)
        {
            Card card = Prefab.GetComponent<Card>();
            if (card.Power > aux.GetComponent<Card>().Power)
            {
                aux = Prefab;
            }
        }
        Card cardaux = aux.GetComponent<Card>();
        if (aux.GetComponent<Card>().PlayerAlQuePertenece == SelectDeckScript.players[0].Id)
        {
            SummonScript.PowerPointsPlayer1 -= cardaux.Power;
        }
        else
        {
            SummonScript.PowerPointsPlayer2 -= cardaux.Power;
        }
        //TODO: agregar al cementerio ,eliminar del campo
        PrefabsCards.Remove(aux);
        Destroy(aux);
    }
    //eliminar la carta con menos poder del rival
    public void DeleteMostWeekCard(ref List<GameObject> PrefabsCards,List<Card> cards)
    {
        GameObject aux = PrefabsCards[0];
        foreach (GameObject Prefab in PrefabsCards)
        {
            if (gameObject != null)
            {
                Card card = Prefab.GetComponent<Card>();
                if (card.Power < aux.GetComponent<Card>().Power)
                {
                    aux = Prefab;
                }
            }
        }
        Card cardaux = aux.GetComponent<Card>();
        if (aux.GetComponent<Card>().PlayerAlQuePertenece == SelectDeckScript.players[0].Id)
        {
            SummonScript.PowerPointsPlayer1 -= cardaux.Power;
        }
        else
        {
            SummonScript.PowerPointsPlayer2 -= cardaux.Power;
        }
        PrefabsCards.Remove(aux);
        Destroy(aux);
    }


    //Multiplicar por n su ataque ,siendo n la cantidad de cartas con poder igual a ella en el campo
    public void GetScalarAtack(List<Card> Cards, ref Card card)
    {
        int count = 1;
        foreach (Card cartica in Cards)
        {
            if (cartica.Power == card.Power)
            {
                count++;
            }
        }
        card.Power *= count;
    }

    //limpiar la fila o campo no vacía propia o del rival 
    public void DeleteFile(ref List<GameObject> Melee1, ref List<GameObject> Ranged1, ref List<GameObject> Siege1,ref List<GameObject> Melee2, ref List<GameObject> Ranged2, ref List<GameObject> Siege2)
    {
        // Crear una lista para contar los tamaños.
        List<int> aux = new List<int>();

        // Solo agregar conteos de listas que no estén vacías
        if (Melee1.Count > 0) aux.Add(Melee1.Count);
        if (Ranged1.Count > 0) aux.Add(Ranged1.Count);
        if (Siege1.Count > 0) aux.Add(Siege1.Count);
        if (Melee2.Count > 0) aux.Add(Melee2.Count);
        if (Ranged2.Count > 0) aux.Add(Ranged2.Count);
        if (Siege2.Count > 0) aux.Add(Siege2.Count);

        // Asegúrate de que hay listas que considerar
        if (aux.Count == 0)
        {
            return; // No hay nada que eliminar
        }

        // Encontrar el índice de la lista con el menor tamaño.
        int position = 0;
        for (int i = 1; i < aux.Count; i++)
        {
            // Obtener el índice correspondiente a su lista
            int currentPos = (i == 1) ? 0 : i; // Ajustar el índice para búsqueda
            if (aux[i] < aux[position]) // Comparar con el mínimo actual
            {
                position = currentPos; // Actualiza la posición del mínimo tamaño
            }
        }

        // Destruir la lista correspondiente a la posición mínima
        switch (position)
        {
            case 0:
                if (Melee1.Count > 0) destroy_(ref Melee1, "Melee1");
                break;
            case 1:
                if (Ranged1.Count > 0) destroy_(ref Ranged1, "Ranged1");
                break;
            case 2:
                if (Siege1.Count > 0) destroy_(ref Siege1, "Siege1");
                break;
            case 3:
                if (Melee2.Count > 0) destroy_(ref Melee2, "Melee2");
                break;
            case 4:
                if (Ranged2.Count > 0) destroy_(ref Ranged2, "Ranged2");
                break;
            case 5:
                if (Siege2.Count > 0) destroy_(ref Siege2, "Siege2");
                break;
        }

        // Método para destruir los objetos de una lista.
        void destroy_(ref List<GameObject> objects, string listName)
        {
            foreach (GameObject obj in objects)
            {
                if (obj != null) // Comprobar si el objeto es nulo
                {
                    //TODO: ver la talla de los puntos de poder al destruir
                    Destroy(obj);
                }
            }
            objects.Clear(); // Limpiar la lista después de destruir
        }
    }
    //calcular el promedio de poder y se lo da a todas las cartas
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
            case "DeleteMostWeakCard":
                List<Card> InvoquedCards1 = SummonScript.InvoquedCardsPlayer1;
                foreach (Card cartica in SummonScript.InvoquedCardsPlayer2)
                {
                    InvoquedCards1.Add(cartica);
                }
                DeleteMostWeekCard(ref SummonScript.InvoquedCardsObjects, InvoquedCards1);
                break;
            case "GetScalarAtack":
                if (k == 0)
                {
                    SummonScript.PowerPointsPlayer1 -= card.Power;
                    GetScalarAtack(SummonScript.InvoquedCardsPlayer1,ref card);
                    SummonScript.PowerPointsPlayer2 += card.Power;
                }
                else
                {
                    SummonScript.PowerPointsPlayer2 -= card.Power;
                    GetScalarAtack(SummonScript.InvoquedCardsPlayer2, ref card);
                    SummonScript.PowerPointsPlayer2 += card.Power;
                }
                break;
            case "DeleteFile":
                //Arreglar esto ,solo elimina lo visual ,no el backend
                DeleteFile(ref SummonScript.CardsOnMeleePlayer1Object,ref SummonScript.CardsOnRangedPlayer1Object,ref SummonScript.CardsOnSiegePlayer1Object,ref SummonScript.CardsOnMeleePlayer2Object,ref SummonScript.CardsOnRangedPlayer2Object,ref SummonScript.CardsOnSiegePlayer2Object);
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
                onActivationParser.OnActivation(card.OnActivationTokens,Compilar.effects,card,ref Program.exceptions,0);
                Context context = new Context();
                Debug.Log(onActivationParser.effect.Name);
                foreach (var item in onActivationParser.Targets)
                {
                    Debug.Log(item.Count);
                }
                //Ya puedo empezar a hacer pruebas en esta talla
                if (onActivationParser.effect.tokens.Count != 0)
                { 
                    Program.actionParser.ActionParser_(card, onActivationParser.effect.tokens, onActivationParser.Targets, context,ref onActivationParser.effect.Params, 0,ref Program.exceptions);
                    Program.actionParser.ActionParser_(card, onActivationParser.effectPostAction.tokens, onActivationParser.TargetsPostAction, context, ref onActivationParser.effect.Params, 0, ref Program.exceptions);
                }
                break;
        }
    }
}