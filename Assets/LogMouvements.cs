using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogMouvements : MonoBehaviour
{
    public float speed; // Vitesse de la voiture
    public bool isMouvingRight = true; // Direction initiale : gauche ou droite
    private float distanceDone = 0;
    private float logLength = 0;
    public float distanceMax;
    // Start is called before the first frame update
    void Start()
    {
        Collider collider = GetComponent<Collider>();
        logLength = collider.bounds.size.z;
    }

    void Update()
    {
        // Déplace la voiture vers l'avant selon sa rotation initiale
        float distance = speed * Time.deltaTime;
        if(isMouvingRight )
        {
            transform.Translate(Vector3.right * distance);
        }
        else
        {
            transform.Translate(Vector3.left * distance);
        }
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

    public float getLogLength()
    {
        return logLength;
    }
}
