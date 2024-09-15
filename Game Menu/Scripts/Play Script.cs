using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//M�todo para jugar una vez seleccionados los mazos 
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
            Debug.Log("a�n no ha seleccionado ambos decks");
        }
    }
}