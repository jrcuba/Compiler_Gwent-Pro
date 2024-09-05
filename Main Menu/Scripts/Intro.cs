using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    static int Started = 0;
    public Text text;
    private string[] messages = {
        "Hola aventurero",
        "Bienvenido a Compiler_Gwent-Pro",
        "En la sesi�n Play puedes aventurarte en el juego con los mazos ya preparados por m� anteriormente",
        "En la sesi�n Create Deck puedes programar tu propio deck",
        "Pulsa el bot�n de cada una de las sesiones seg�n lo que quieras hacer",
        "Gracias por Esperar :)",
        ""
    };

    void Start()
    {
        if (Started == 0)
        {
            StartCoroutine(DisplayMessages());
            Started = 1;
        }
    }
    IEnumerator DisplayMessages()
    {
        yield return new WaitForSeconds(1f);
        foreach (string message in messages)
        {
            yield return StartCoroutine(TypeText(message));
            yield return new WaitForSeconds(2f);
        }
    }

    IEnumerator TypeText(string message)
    {
        text.text = "";
        foreach (char letter in message.ToCharArray())
        {
            text.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
