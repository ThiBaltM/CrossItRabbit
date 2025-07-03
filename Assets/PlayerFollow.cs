using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    public Transform rabbit; // Référence au transform du lapin
    public float height; // Hauteur de la caméra au-dessus du lapin
    public float depthOffset; // Décalage en profondeur pour voir plus devant le lapin
    private float advancement=-30;

    void LateUpdate()
    {
        if (rabbit != null)
        {
            // Position de la caméra : suit le lapin en X et Z, mais reste fixe en Y
            float cameraX = transform.position.x; // Ne suit pas le mouvement latéral
            float cameraY = height; // Hauteur fixe
            float cameraZ = rabbit.position.z + depthOffset; // Suit le lapin en profondeur avec un décalage
            if(cameraZ < advancement)
            {
                cameraZ = advancement;
            }
            else
            {
                advancement = cameraZ;
            }

            // Applique la nouvelle position
            transform.position = new Vector3(cameraX, cameraY, cameraZ);

            // La caméra regarde toujours vers le bas (vers le fond de la scène)
            transform.rotation = Quaternion.Euler(70f, 0f, 0f);

        }
    }
}
