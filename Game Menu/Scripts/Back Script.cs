using UnityEngine;
using UnityEngine.SceneManagement;

//M�todo para volver al men� principal
public class Back : MonoBehaviour
{
    public void GoBack()
    {
        SceneManager.LoadScene("Main Menu");
    }
}