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
        "En la sesión Play puedes aventurarte en el juego con los mazos ya preparados por mí anteriormente",
        "En la sesión Create Deck puedes programar tu propio deck",
        "Pulsa el botón de cada una de las sesiones según lo que quieras hacer",
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
