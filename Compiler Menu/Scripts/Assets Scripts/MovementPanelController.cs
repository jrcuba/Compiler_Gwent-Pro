using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementPanelController : MonoBehaviour
{
    // Amplitude of the movement in meters (0.005 meters = 0.5 centimeters)
    public float amplitude = 0.005f;
    // Speed of the movement
    public float speed = 2.0f;
    // Initial position of the image
    private Vector3 initialPosition;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the new position
        float newY = initialPosition.y + Mathf.Sin(Time.time * speed) * amplitude;
        transform.position = new Vector3(initialPosition.x, newY, initialPosition.z);
    }
}