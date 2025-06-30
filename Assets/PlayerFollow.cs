using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    private void Start()
    {
        transform.position = new Vector3(rabbit.position.x, height, -depthOffset);
    }
    public Transform rabbit; // R�f�rence au transform du lapin
    public float height = 15f; // Hauteur de la cam�ra au-dessus du lapin
    public float depthOffset = 5f; // D�calage en profondeur pour voir plus devant le lapin
    private float advancement=0;

    void LateUpdate()
    {
        if (rabbit != null)
        {
            // Position de la cam�ra : suit le lapin en X et Z, mais reste fixe en Y
            float cameraX = transform.position.x; // Ne suit pas le mouvement lat�ral
            float cameraY = height; // Hauteur fixe
            float cameraZ = rabbit.position.z + depthOffset; // Suit le lapin en profondeur avec un d�calage
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

            // La cam�ra regarde toujours vers le bas (vers le fond de la sc�ne)
            transform.rotation = Quaternion.Euler(70f, 0f, 0f);

        }
    }
}
