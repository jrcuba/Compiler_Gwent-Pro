using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Clase para cargar la escena del menú principal  
public class Play : MonoBehaviour
{
    private int count = 0;
    private float maxDistance = 5f;  // Distancia máxima desde el centro  

    private void OnMouseEnter()
    {
        if (count <= 1)
        {
            // Mover el botón a una posición aleatoria dentro de un radio de maxDistance  
            float randomX = Random.Range(-maxDistance, maxDistance);
            float randomY = Random.Range(-maxDistance, maxDistance);
            float randomZ = Random.Range(-maxDistance, maxDistance);

            // Establecer la nueva posición  
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