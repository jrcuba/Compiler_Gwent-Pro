using UnityEngine;
using UnityEngine.SceneManagement;

//Método para volver al menú principal
public class Back : MonoBehaviour
{
    public void GoBack()
    {
        SceneManager.LoadScene("Main Menu");
    }
}