using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rabbitMouvement : MonoBehaviour
{
    public float moveDistance = 1f; // Distance � d�placer

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // D�placer en profondeur (axe Z positif)
            transform.position += Vector3.forward * moveDistance;
            transform.rotation = Quaternion.LookRotation(Vector3.forward);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // D�placer vers l'arri�re (axe Z n�gatif)
            transform.position += Vector3.back * moveDistance;
            transform.rotation = Quaternion.LookRotation(Vector3.back);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // D�placer vers la gauche (axe X n�gatif)
            transform.position += Vector3.left * moveDistance;
            transform.rotation = Quaternion.LookRotation(Vector3.left);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // D�placer vers la droite (axe X positif)
            transform.position += Vector3.right * moveDistance;
            transform.rotation = Quaternion.LookRotation(Vector3.right);
        }
    }
}
