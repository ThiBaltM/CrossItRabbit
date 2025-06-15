using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rabbitMouvement : MonoBehaviour
{
    public float moveDistance = 1f; // Distance à déplacer

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // Déplacer en profondeur (axe Z positif)
            transform.position += Vector3.forward * moveDistance;
            transform.rotation = Quaternion.LookRotation(Vector3.forward);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // Déplacer vers l'arrière (axe Z négatif)
            transform.position += Vector3.back * moveDistance;
            transform.rotation = Quaternion.LookRotation(Vector3.back);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // Déplacer vers la gauche (axe X négatif)
            transform.position += Vector3.left * moveDistance;
            transform.rotation = Quaternion.LookRotation(Vector3.left);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // Déplacer vers la droite (axe X positif)
            transform.position += Vector3.right * moveDistance;
            transform.rotation = Quaternion.LookRotation(Vector3.right);
        }
    }
}
