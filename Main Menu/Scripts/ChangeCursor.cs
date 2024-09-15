using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCursor : MonoBehaviour
{
    public Texture2D cursorTexture; // Arrastra aquí tu textura desde el Inspector  
    public Vector2 hotspot = Vector2.zero; // Ajusta esto si es necesario  

    void Start()
    {
        // Cambiar el cursor al iniciar  
        Cursor.SetCursor(cursorTexture, hotspot, CursorMode.Auto);
    }

    void OnDisable()
    {
        // Restaurar el cursor por defecto al desactivar el script  
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}