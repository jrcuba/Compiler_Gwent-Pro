using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowCardsInfoScript : MonoBehaviour
{
    public GameObject CardPrefab;
    public Text NameField;
    public Text RangeField;
    public Text TypeField;
    public Text PowerField;
    public Text FactionField;
    public Image image;
    void OnMouseEnter()
    {
        Card card = GetComponent<Card>();
        if (card != null)
        {
            TypeField.text = card.Type;
            NameField.text = card.Name;
            PowerField.text = card.Power.ToString();
            FactionField.text = card.Faction;
            RangeField.text = string.Join(", ", card.Range);
            image.sprite = gameObject.GetComponent<Image>().sprite;
            //Debug.Log("Owner: " + card.Owner());
        }
        if (!SystemMouseMover.cards.Contains(gameObject) && SelectDeckScript.players[1].Id == card.PlayerAlQuePertenece)
        {
            SystemMouseMover.cards.Add(gameObject);
        }
    }

    void OnMouseExit()
    {
        //Debug.Log("Mouse exited the card.");
        NameField.name = "";
    }
}
