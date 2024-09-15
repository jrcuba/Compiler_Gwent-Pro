using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Clase para cargar la escena del men� principal  
public class Play : MonoBehaviour
{
    private int count = 0;
    private float maxDistance = 5f;  // Distancia m�xima desde el centro  

    private void OnMouseEnter()
    {
        if (count <= 1)
        {
            // Mover el bot�n a una posici�n aleatoria dentro de un radio de maxDistance  
            float randomX = Random.Range(-maxDistance, maxDistance);
            float randomY = Random.Range(-maxDistance, maxDistance);
            float randomZ = Random.Range(-maxDistance, maxDistance);

            // Establecer la nueva posici�n  
            Transform transform = GetComponent<Transform>();
            transform.position = new Vector3(randomX, randomY, randomZ);
            count++;
        }
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Game Menu");
    }
}