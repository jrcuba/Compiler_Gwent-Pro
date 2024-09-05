using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreviousDeckScript : MonoBehaviour
{
    public Text text;
    private string keytext;
    public void GoToPreviousDeck()
    {
        foreach (string key in LoadDataBase.Mazos.Keys)
        {
            if (text.text == key)
            {
                text.text = keytext;
                break;
            }
            else
            {
                keytext = key;
            }
        }
    }
}
