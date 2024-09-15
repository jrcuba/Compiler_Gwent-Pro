using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Clase para acceder al menú de compilación
public class Compilate : MonoBehaviour
{
    public void LoadCompilateMenu()
    {
        SceneManager.LoadScene("Compiler Menu");
    }
}
