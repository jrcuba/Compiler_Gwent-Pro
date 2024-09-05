using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playwithia : MonoBehaviour
{
    public void StartGame()
    {
        SummonScript.IsplayinWithIa = true;
        SceneManager.LoadScene("Game");
    }
}
