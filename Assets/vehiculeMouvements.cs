using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vehiculeMovements : MonoBehaviour
{
    public float speed = 1f; // Vitesse de la voiture
    public bool moveLeft = false; // Direction initiale : gauche ou droite
    private float distanceDone = 0;
    private float vehiculeLength = 0;

    void Start()
    {
        Collider collider = GetComponent<Collider>();
        vehiculeLength = collider.bounds.size.z;

        // Détermine la direction initiale
        if (moveLeft)
        {
            // Tourne la voiture vers la gauche au début
            transform.rotation = Quaternion.Euler(0, 270, 0);
        }
        else
        {
            // Tourne la voiture vers la droite au début
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
    }

    void Update()
    {
        // Déplace la voiture vers l'avant selon sa rotation initiale
        float distance = speed * Time.deltaTime;
        transform.Translate(Vector3.forward * distance);
        distanceDone += distance;
        if(distanceDone > 16) 
        { 
            Destroy(gameObject);
        }
    }

    public float getDistanceDone()
    {
        return distanceDone;
    }

    public float getVehiculeLength()
    {
        return vehiculeLength;
    }
}
