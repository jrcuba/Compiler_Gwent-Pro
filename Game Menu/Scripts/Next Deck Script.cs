using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextDeckScript : MonoBehaviour
{
    public Text text;
    public void PassDeck()
    {
        bool Verificate = false;
        foreach (string key in LoadDataBase.Mazos.Keys)
        {
            if (Verificate == true)
            {
                text.text = key;
                break;
            }
            else if (text.text == key)
            {
                Verificate = true;
            }
        }
    }
}
