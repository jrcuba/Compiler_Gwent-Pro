using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Método para jugar una vez seleccionados los mazos 
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