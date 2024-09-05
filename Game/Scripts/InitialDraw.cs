using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI; // Asegúrate de incluir esto

public class GameControllerScript : MonoBehaviour
{
    public GameObject CardPrefab; // Prefab para la carta
    public static List<GameObject> prefabs = new List<GameObject>();
    public Transform HandPlayer1;
    public Transform HandPlayer2;
    public static Transform HandPlayer1Copy;
    public static Transform HandPlayer2Copy;
    public Text Player1NameText;
    public Text Player2NameText;
    void Start()
    {
        Player1NameText.text = SelectDeckScript.players[0].Id;
        Player2NameText.text = SelectDeckScript.players[1].Id;
        foreach (var player in SelectDeckScript.players)
        {
            // Barajar el deck
            player.DeckOfPlayer = Shuffle(ref player.DeckOfPlayer);
            // Robar 10 cartas
            if (player.DeckOfPlayer.Count >= 10)
            {
                player.Hand = InitialDraw(ref player.DeckOfPlayer);
            }
            else
            {
                Debug.LogError("No hay suficientes cartas para robar 10 cartas.");
                continue;
            }
            // Mostrar las cartas de las respectivas manos
            if (player == SelectDeckScript.players[0])
            {

                InstanciateHand(player.Hand, HandPlayer1);
            }
            else
            {
                InstanciateHand(player.Hand, HandPlayer2);
            }
        }
        HandPlayer1Copy = HandPlayer1;
        HandPlayer2Copy = HandPlayer2;
    }

    public List<Card> Shuffle(ref List<Card> cards)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            int random = Random.Range(0, cards.Count);
            Card aux = cards[random];
            cards[random] = cards[i];
            cards[i] = aux;
        }
        return cards;
    }

    private List<Card> InitialDraw(ref List<Card> cards)
    {
        List<Card> cardsaux = new List<Card>();
        for (int i = 0; i < 10; i++)
        {
            cardsaux.Add(cards[0]);
            cards.RemoveAt(0);
        }
        return cardsaux;
    }

    private void InstanciateHand(List<Card> hand, Transform handTransform)
    {
        int i = -790;
        foreach (var card in hand)
        {
            // Crear una instancia del prefab de la carta
            GameObject cardInstance = Instantiate(CardPrefab, handTransform);

            // Ajustar la posición local de la carta
            cardInstance.transform.localPosition = new Vector3(i, 0, 0);

            // Configurar la imagen de la carta
            Image cardImage = cardInstance.GetComponent<Image>();
            if (cardImage != null)
            {
                cardImage.sprite = card.Image;
            }

            // Configurar otros componentes de la carta si es necesario
            Card cardComponent = cardInstance.GetComponent<Card>();
            if (cardComponent != null)
            {
                cardComponent.PlayerAlQuePertenece = card.PlayerAlQuePertenece;
                cardComponent.Type = card.Type;
                cardComponent.Name = card.Name;
                cardComponent.Faction = card.Faction;
                cardComponent.Power = card.Power;
                cardComponent.Range = new List<string>(card.Range);
                cardComponent.OnActivationTokens = new List<Token>(card.OnActivationTokens);
                cardComponent.player = card.player;
                cardComponent.EffectName = card.EffectName;
            }
            prefabs.Add(cardInstance);
            i += 158;
        }
    }
}
