using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SummonScript : MonoBehaviour
{
    //Objetos del Juego y del Backend
    public static int CantCardsInHandPlayer1 = 10;
    public static int CantCardsInHandPlayer2 = 10;

    public GameObject Ia;
    public static bool IsplayinWithIa = false;
    public EffectsNoCompilables effectsNoCompilables;
    public GameObject CardPrefab;
    public static GameObject CardPrefab_Copy;
    public static Transform HandPlayer1;
    public static Transform HandPlayer2;
    public static int RoundsWinsPlayer1 = 0;
    public static int RoundsWinsPlayer2 = 0;
    public Text PowerPointsTextPlayer1;
    public Text PowerPointsTextPlayer2;
    public static double PowerPointsPlayer1 = 0;
    public static double PowerPointsPlayer2 = 0;
    public static bool InvocatedBool = false;
    public static int RoundCount = 1;
    public static int count = 4;
    private static int CountDraw2Cards = 0;
    private string Player1Id;
    private string Player2Id;
    public static List<Card> CardsSpecialOnMeleePlayer1 = new List<Card>();
    public static List<Card> CardsSpecialOnRangedPlayer1 = new List<Card>();
    public static List<Card> CardsSpecialOnSiegePlayer1 = new List<Card>();
    public static List<Card> CardsSpecialOnMeleePlayer2 = new List<Card>();
    public static List<Card> CardsSpecialOnRangedPlayer2 = new List<Card>();
    public static List<Card> CardsSpecialOnSiegePlayer2 = new List<Card>();
    public static List<Card> CardsOnMeleePlayer1 = new List<Card>();
    public static List<Card> CardsOnMeleePlayer2 = new List<Card>();
    public static List<Card> CardsOnRangedPlayer1 = new List<Card>();
    public static List<Card> CardsOnRangedPlayer2 = new List<Card>();
    public static List<Card> CardsOnSiegePlayer1 = new List<Card>();
    public static List<Card> CardsOnSiegePlayer2 = new List<Card>();
    public static List<Card> CementeryPlayer1 = new List<Card>();
    public static List<Card> CementeryPlayer2 = new List<Card>();
    public static List<Card> InvoquedCards = new List<Card>();
    public static List<Card> InvoquedCardsPlayer1 = new List<Card>();
    public static List<Card> InvoquedCardsPlayer2 = new List<Card>();
    public static List<GameObject> InvoquedCardsObjects = new List<GameObject>();
    public static List<GameObject> CardsOnMeleePlayer1Object = new List<GameObject>();
    public static List<GameObject> CardsOnMeleePlayer2Object = new List<GameObject>();
    public static List<GameObject> CardsOnRangedPlayer1Object = new List<GameObject>();
    public static List<GameObject> CardsOnRangedPlayer2Object = new List<GameObject>();
    public static List<GameObject> CardsOnSiegePlayer1Object = new List<GameObject>();
    public static List<GameObject> CardsOnSiegePlayer2Object = new List<GameObject>();
    public static List<GameObject> CementeryPlayer1Object = new List<GameObject>();
    public static List<GameObject> CementeryPlayer2Object = new List<GameObject>();
    private Vector3 InitialPosition;

    //Inicialización de objetos base
    private void Start()
    {
        InitialPosition = transform.position;
        Player1Id = SelectDeckScript.players[0].Id;
        Player2Id = SelectDeckScript.players[1].Id;
        //cada jugador debe cambiar 2 cartas al principio
        HandPlayer1 = GameControllerScript.HandPlayer1Copy;
        HandPlayer2 = GameControllerScript.HandPlayer2Copy;
        CardPrefab_Copy = CardPrefab;
    }
    //Mostrar los puntos totales en todo momento
    private void Update()
    {
        PowerPointsTextPlayer1.text = PowerPointsPlayer1.ToString();
        PowerPointsTextPlayer2.text = PowerPointsPlayer2.ToString();
    }
    //Controlador del juego
    public void Summon()
    {
        Card card = GetComponent<Card>();
        if ((CountDraw2Cards == 0 || CountDraw2Cards == 1) && card.PlayerAlQuePertenece == SelectDeckScript.players[0].Id)
        {
            CementeryPlayer1.Add(card);
            GameControllerScript.prefabs.Remove(gameObject);
            if (SystemMouseMover.cards.Contains(gameObject))
            {
                SystemMouseMover.cards.Remove(gameObject);
            }
            Destroy(gameObject);
            SelectDeckScript.players[0].Hand.Remove(card);
            CountDraw2Cards++;
            if (CountDraw2Cards == 2)
            {
                Change2Cards(0, card,HandPlayer1);
            }
        }
        //Primera parte con ia lista
        else if ((CountDraw2Cards == 2 || CountDraw2Cards == 3) && card.PlayerAlQuePertenece == SelectDeckScript.players[1].Id) 
        {
            CementeryPlayer2.Add(card);
            GameControllerScript.prefabs.Remove(gameObject);
            if (SystemMouseMover.cards.Contains(gameObject))
            {
                SystemMouseMover.cards.Remove(gameObject);
            }
            Destroy(gameObject);
            SelectDeckScript.players[1].Hand.Remove(card);
            CountDraw2Cards++;
            if (CountDraw2Cards == 4)
            {
                Change2Cards(1, card, HandPlayer2);
            }
            InvocatedBool = true;
        }



        if (card.PlayerAlQuePertenece == Player1Id && count % 2 == 0 && InvocatedBool == true)
        {
            if (card.Type == "Monster" && !EffectsNoCompilables.PutIncreaseBool)
            {
                if (card.Range.Contains("Melee") || card.Range.Contains("Ranged") || card.Range.Contains("Siege")) {
                    //remover la carta de la mano
                    if (card.Range.Contains("Melee") && CardsOnMeleePlayer1.Count < 2) 
                    {
                        //me queda eliminar de la mano 
                        CardsOnMeleePlayer1.Add(card);
                        CardsOnMeleePlayer1Object.Add(gameObject);
                        transform.position = new Vector3(-2.5f, -0.5f,90);
                        DoTHinksPlayer1(card);
                        
                    }
                    else if (card.Range.Contains("Ranged") && CardsOnRangedPlayer1.Count < 2)
                    {
                        CardsOnRangedPlayer1.Add(card);
                        CardsOnRangedPlayer1Object.Add(gameObject);
                        transform.position = new Vector3(-2.5f, -1.33f, 90);
                        DoTHinksPlayer1(card);
                    }
                    else if (card.Range.Contains("Siege") && CardsOnSiegePlayer1.Count < 2)
                    {
                        CardsOnSiegePlayer1.Add(card);
                        CardsOnSiegePlayer1Object.Add(gameObject);
                        transform.position = new Vector3(-2.5f, -2.1f, 90);
                        DoTHinksPlayer1(card);
                    }

                }
            }
            else if (card.Type == "Effect")
            {
                if (card.Range.Contains("Melee") || card.Range.Contains("Ranged") || card.Range.Contains("Siege"))
                {
                    if (card.Range.Contains("Melee") && CardsSpecialOnMeleePlayer1.Count < 1)
                    {
                        CardsSpecialOnMeleePlayer1.Add(card);
                        CardsOnMeleePlayer1Object.Add(gameObject);
                        transform.position = new Vector3(-0.5f, -0.5f, 90);
                        DoEffectCardThinksPlayer1(card);
                    }
                    else if (card.Range.Contains("Ranged") && CardsSpecialOnRangedPlayer1.Count < 1)
                    {
                        CardsSpecialOnRangedPlayer1.Add(card);
                        CardsOnRangedPlayer1Object.Add(gameObject);
                        transform.position = new Vector3(-0.5f, -1.33f, 90);
                        DoEffectCardThinksPlayer1(card);
                    }
                    else if (card.Range.Contains("Siege") && CardsSpecialOnSiegePlayer1.Count < 1)
                    {
                        CardsSpecialOnSiegePlayer1.Add(card);
                        CardsOnSiegePlayer1Object.Add(gameObject);
                        transform.position = new Vector3(-0.5f, -2.1f, 90);
                        DoEffectCardThinksPlayer1(card);
                    }
                }
            }
        }
        else if (card.PlayerAlQuePertenece == Player2Id && count % 2 == 1 && InvocatedBool == true)
        {
            if (card.Type == "Monster" && !EffectsNoCompilables.PutIncreaseBool) 
            { 
                if (card.Range.Contains("Melee") || card.Range.Contains("Ranged") || card.Range.Contains("Siege")) {
                    if (card.Range.Contains("Melee") && CardsOnMeleePlayer2.Count < 2) 
                    {
                        CardsOnMeleePlayer2.Add(card);
                        CardsOnMeleePlayer2Object.Add(gameObject);
                        transform.position = new Vector3(-2.5f, 0.5f, 90);
                        DoThinksPlayer2(card);
                    }
                    else if (card.Range.Contains("Ranged") && CardsOnRangedPlayer2.Count < 2)
                    {
                        CardsOnRangedPlayer2.Add(card);
                        CardsOnRangedPlayer2Object.Add(gameObject);
                        transform.position = new Vector3(-2.5f, 1.25f, 90);
                        DoThinksPlayer2(card);
                    }
                    else if (card.Range.Contains("Siege") && CardsOnSiegePlayer2.Count < 2)
                    {
                        CardsOnSiegePlayer2.Add(card);
                        CardsOnSiegePlayer2Object.Add(gameObject);
                        transform.position = new Vector3(-2.5f, 2f, 90);
                        DoThinksPlayer2(card);
                    }
                }
            }
            else if (card.Type == "Effect")
            {
                if (card.Range.Contains("Melee") || card.Range.Contains("Ranged") || card.Range.Contains("Siege"))
                {
                    if (card.Range.Contains("Melee") && CardsSpecialOnMeleePlayer2.Count < 1)
                    {
                        CardsSpecialOnMeleePlayer2.Add(card);
                        CardsOnMeleePlayer1Object.Add(gameObject);
                        SelectDeckScript.players[1].Hand.Remove(card);
                        transform.position = new Vector3(-0.5f, 0.5f, 90);
                        DoEffectThinksPlayer2(card);
                    }
                    else if (card.Range.Contains("Ranged") && CardsSpecialOnRangedPlayer2.Count < 1)
                    {
                         CardsSpecialOnRangedPlayer2.Add(card);
                         CardsOnMeleePlayer1Object.Add(gameObject);
                        transform.position = new Vector3(-0.5f, 1.25f, 90);
                        DoEffectThinksPlayer2(card);

                    }
                    else if (card.Range.Contains("Siege") && CardsSpecialOnSiegePlayer2.Count < 1)
                    {
                        CardsSpecialOnSiegePlayer2.Add(card);
                        CardsOnMeleePlayer1Object.Add(gameObject);
                        transform.position = new Vector3(-0.5f, 2f, 90);
                        DoEffectThinksPlayer2(card);
                    }
                }
            }
        }
    }

    //Método para robar carta
    public void DrawCard(Card card,Transform transform,Player player,GameObject Prefab,int PlayerPosition)
    {
        GameObject CardInstance = Instantiate(Prefab, transform);
        //le asigno el padre
        //ajusto la posición local
        CardInstance.transform.localPosition = new Vector3(0, 0, 0);
        // Configurar la imagen de la carta
        Image cardImage = CardInstance.GetComponent<Image>();
        if (cardImage != null)
        {
            cardImage.sprite = card.Image;
        }
        //asigno resto de componentes
        Card cardComponent = CardInstance.GetComponent<Card>();
        if (cardComponent != null)
        {
            cardComponent.PlayerAlQuePertenece = card.PlayerAlQuePertenece;
            cardComponent.Type = card.Type;
            cardComponent.CardName = card.CardName;
            cardComponent.Faction = card.Faction;
            cardComponent.Power = card.Power;
            cardComponent.Range = new List<string>(card.Range);
            cardComponent.OnActivationTokens = new List<Token>(card.OnActivationTokens);
            cardComponent.player = card.player;
            cardComponent.EffectName = card.EffectName;
        }
        GameControllerScript.prefabs.Add(CardInstance);
        CardInstance.GetComponent<Button>().enabled = true;
        CardInstance.GetComponent<Image>().enabled = true;
        CardInstance.GetComponent<BoxCollider2D>().enabled = true;
        CardInstance.GetComponent<SummonScript>().enabled = true;
        CardInstance.GetComponentInChildren<TextMeshPro>().enabled = true;
        player.Hand.Add(card);
        SelectDeckScript.players[PlayerPosition].DeckOfPlayer.Remove(card);
    }
    //Método para verificar si está jugando con el Bot
    public static void IsPlayingWithIaMethod()
    {
        if (IsplayinWithIa)
        {
            SystemMouseMover.FirstMovement = false;
            SystemMouseMover.SecondMovement = false;
            SystemMouseMover.SextoMovimiento = true;
            SystemMouseMover.SectimoMovimiento = true;
            SystemMouseMover.OctavoMovimiento = false;
            SystemMouseMover.NovenoMovimiento = false;
            SystemMouseMover.isMoving = true;
            
        }
    }
    //Grupo de métodos para no repetir código de los jugadores
    private void DoTHinksPlayer1(Card card)
    {
        InvoquedCardsObjects.Add(gameObject);
        InvoquedCardsPlayer1.Add(card);
        IsPlayingWithIaMethod();
        PowerPointsPlayer1 += double.Parse(card.Power.ToString());
        SelectDeckScript.players[0].Hand.Remove(card);
        InvoquedCards.Add(card);
        count++;
        gameObject.GetComponent<SummonScript>().enabled = false;
        PassTurnScript.RoundPassCount = 0;
        CantCardsInHandPlayer1--;
        RectTransform rect = gameObject.GetComponent<RectTransform>();
        rect.localScale = new Vector3(0.35f, 0.35f);

        effectsNoCompilables.InvocateEffect(gameObject.GetComponent<Card>());
    }
    private void DoEffectCardThinksPlayer1(Card card)
    {

        InvoquedCardsObjects.Add(gameObject);
        InvoquedCardsPlayer1.Add(card);
        InvoquedCards.Add(card);
        SelectDeckScript.players[0].Hand.Remove(card);
        IsPlayingWithIaMethod();
        count++;
        gameObject.GetComponent<SummonScript>().enabled = false;
        PassTurnScript.RoundPassCount = 0;
        CantCardsInHandPlayer1--;
        RectTransform rect = gameObject.GetComponent<RectTransform>();
        rect.localScale = new Vector3(0.35f, 0.35f);
        if (!EffectsNoCompilables.PutIncreaseBool)
        {
            effectsNoCompilables.InvocateEffect(gameObject.GetComponent<Card>());
        }

        EffectsNoCompilables.PutIncreaseBool = false;
    }
    private void DoThinksPlayer2(Card card)
    {
        InvoquedCardsObjects.Add(gameObject);
        InvoquedCardsPlayer2.Add(card);
        InvoquedCards.Add(card);
        SelectDeckScript.players[1].Hand.Remove(card);
        PowerPointsPlayer2 += double.Parse(card.Power.ToString());
        count--;
        gameObject.GetComponent<SummonScript>().enabled = false;
        PassTurnScript.RoundPassCount = 0;
        CantCardsInHandPlayer2--;
        RectTransform rect = gameObject.GetComponent<RectTransform>();
        rect.localScale = new Vector3(0.35f, 0.35f);
        effectsNoCompilables.InvocateEffect(gameObject.GetComponent<Card>());
    }
    private void DoEffectThinksPlayer2(Card card)
    {
        InvoquedCardsObjects.Add(gameObject);
        InvoquedCardsPlayer1.Add(card);
        InvoquedCards.Add(card);
        SelectDeckScript.players[1].Hand.Remove(card);
        count--;
        gameObject.GetComponent<SummonScript>().enabled = false;
        PassTurnScript.RoundPassCount = 0;
        CantCardsInHandPlayer2--;
        RectTransform rect = gameObject.GetComponent<RectTransform>();
        rect.localScale = new Vector3(0.35f, 0.35f);

        if (!EffectsNoCompilables.PutIncreaseBool)
        {
            effectsNoCompilables.InvocateEffect(gameObject.GetComponent<Card>());
        }
        EffectsNoCompilables.PutIncreaseBool = false;
    }
    public void InstanciateCardLikeEffect(Card card,Player player,int PlayerPosition,GameObject Prefab)
    {
        if (PlayerPosition == 0)
            DrawCard(card,HandPlayer1, player, Prefab, PlayerPosition);
        else
        {
            DrawCard(card, HandPlayer2, player, Prefab, PlayerPosition);
        }
    }
    public void Change2Cards(int k,Card card,Transform HandPlayer)
    {
        SelectDeckScript.players[k].Hand.Add(SelectDeckScript.players[k].DeckOfPlayer[SelectDeckScript.players[k].DeckOfPlayer.Count - 1]);
        DrawCard(SelectDeckScript.players[k].DeckOfPlayer[SelectDeckScript.players[k].DeckOfPlayer.Count - 1], HandPlayer, SelectDeckScript.players[k], CardPrefab, k);
        SelectDeckScript.players[k].Hand.Add(SelectDeckScript.players[k].DeckOfPlayer[SelectDeckScript.players[k].DeckOfPlayer.Count - 1]);
        DrawCard(SelectDeckScript.players[k].DeckOfPlayer[SelectDeckScript.players[k].DeckOfPlayer.Count - 1], HandPlayer, SelectDeckScript.players[k], CardPrefab, k);
        SelectDeckScript.players[k].Hand.Add(SelectDeckScript.players[k].DeckOfPlayer[SelectDeckScript.players[k].DeckOfPlayer.Count - 1]);
        SystemMouseMover.isMoving = true;
    }
}