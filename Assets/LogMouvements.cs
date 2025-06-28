using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogMouvements : MonoBehaviour
{
    public float speed; // Vitesse de la voiture
    public bool isMouvingRight = true; // Direction initiale : gauche ou droite
    private float distanceDone = 0;
    private float vehiculeLength = 0;
    public float distanceMax;
    // Start is called before the first frame update
    void Start()
    {
        Collider collider = GetComponent<Collider>();
        vehiculeLength = collider.bounds.size.z;

        // Détermine la direction initiale
        if (!isMouvingRight)
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
        if (distanceDone > distanceMax)
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
