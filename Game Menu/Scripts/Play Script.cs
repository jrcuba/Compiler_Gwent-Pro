using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayScript : MonoBehaviour
{
    public void StartGame()
    {
        if (SelectDeckScript.SelectedDecks.Count == 2)
        {
            SceneManager.LoadScene("Game");
        }
        else
        {
            Debug.Log("aún no ha seleccionado ambos decks");
        }
    }
}