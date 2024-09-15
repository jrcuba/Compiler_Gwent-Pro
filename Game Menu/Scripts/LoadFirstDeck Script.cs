using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Método que me carga los decks que tengo guardados en la base de datos 
public class LoadFirstDeck : MonoBehaviour
{
    public Text Text;
    public static int Count = -1;
    void Start()
    {
        foreach (string key in LoadDataBase.Mazos.Keys)
        {
            Count++;
            Text.text = key;
            break;
        }
    }
}