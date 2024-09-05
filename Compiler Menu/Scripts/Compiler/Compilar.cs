using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Compilar : MonoBehaviour
{
    private static int count = 3;
    public static List<Effect> effects = new List<Effect>();
    public static List<Exceptions> exceptions = new List<Exceptions>();
    public Image Sprite;
    public Image Sprite2;
    public Image Sprite3;
    public InputField inputField;
    public void ShowInitialInfo()
    {
        //y compilar también
        effects = Program.effects;
        exceptions = Program.exceptions;
        LoadDataBase.Mazos.Add("Mazo " + count,Program.cards);
        count++;
        inputField.text = "";
        StartCoroutine(SwapSprites());
    }

    private IEnumerator SwapSprites()
    {
        var aux = Sprite2.sprite;
        
        if (Program.exceptions.Count == 0)
        {
            Sprite2.sprite = Sprite.sprite;

            // Esperar 2 segundos
            yield return new WaitForSeconds(2);
            Sprite2.sprite = aux;
        }
    }
}