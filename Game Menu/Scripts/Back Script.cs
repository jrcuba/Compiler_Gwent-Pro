using UnityEngine;
using UnityEngine.SceneManagement;

public class Back : MonoBehaviour
{
    public void GoBack()
    {
        SceneManager.LoadScene("Main Menu");
    }
}