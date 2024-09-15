using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Método para jugar con la Ia una vez seleccionados los mazos
public class playwithia : MonoBehaviour
{
    public void StartGame()
    {
        SummonScript.IsplayinWithIa = true;
        SceneManager.LoadScene("Game");
    }
}
