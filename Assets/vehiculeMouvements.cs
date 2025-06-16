using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vehiculeMouvements : MonoBehaviour
{
    public float speed = 10f; // Vitesse de la voiture
    public bool moveLeft = false; // Direction initiale : gauche ou droite

    void Start()
    {
        // D�termine la direction initiale
        if (moveLeft)
        {
            // Tourne la voiture vers la gauche au d�but
            transform.rotation = Quaternion.Euler(0, 270, 0);
        }
        else
        {
            // Tourne la voiture vers la droite au d�but
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
    }

    void Update()
    {
        // D�place la voiture vers l'avant selon sa rotation initiale
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
