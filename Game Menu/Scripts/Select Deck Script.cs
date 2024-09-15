using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//Método para seleccionar el deck 
public class SelectDeckScript : MonoBehaviour
{
    private static int count = 0;
    private bool Verificate = true;
    public Text text;
    public InputField InputField;
    public static Dictionary<string,List<Card>> SelectedDecks = new Dictionary<string, List<Card>> ();
    public static List<Player> players = new List<Player> ();
    public void Select()
    {
        if (InputField.text != "") {
            if (SelectedDecks.Count <= 2)
            {
                //verificar primero si el mazo ya fue seleccionado
                foreach (var dic in SelectedDecks)
                {
                    if (dic.Key == text.text)
                    {
                        Debug.Log("El mazo ya ha sido seleccionado");
                        Verificate = false;
                        break;
                    }
                }
                if (Verificate)
                {
                    SelectedDecks.Add(text.text, new List<Card>());
                    foreach (var dic in LoadDataBase.Mazos)
                    {
                        if (dic.Key == text.text)
                        {
                            //TODO : tengo que revisar si ya se creó un player con ese nombre
                            SelectedDecks[text.text] = LoadDataBase.Mazos[dic.Key];
                            count++;
                            players.Add(new Player(dic.Value, InputField.text));
                            foreach (Card card in dic.Value)
                            {
                                card.PlayerAlQuePertenece = players[players.Count - 1].Id;
                            }
                            InputField.text = "";
                            Debug.Log("El mazo ha sido seleccionado");
                            break;
                        }
                    }
                }
            }
            else
            {
                Debug.Log("Ya ha seleccionado 2 mazos");
            }
            Verificate = true;
        }
        else
        {
            Debug.Log("Error ,el campo de nombre de jugador no puede estar vacio");
        }
    }
}