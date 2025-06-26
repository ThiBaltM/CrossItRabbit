using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rabbitMouvement : MonoBehaviour
{
    public float moveDistance = 3f; // Distance � d�placer
    private int horizontalPos = 0;
    private int verticalPos = 0;
    public int mapWidth;
    public int maxBackward;
    private int currentBackward = 0;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = gameObject.GetComponentInParent<GameManager>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // D�placer en profondeur (axe Z positif)
            transform.position += Vector3.forward * moveDistance;
            transform.rotation = Quaternion.LookRotation(Vector3.forward);
            verticalPos += 1;
            if (currentBackward > 0)
            {
                currentBackward-=1;
            }
            else
            {
                gameManager.deleteLayers();
                gameManager.createLayer();
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (verticalPos > 0 && currentBackward<maxBackward)
            {
                verticalPos -= 1;
                currentBackward += 1;
                // D�placer vers l'arri�re (axe Z n�gatif)
                transform.position += Vector3.back * moveDistance;
                transform.rotation = Quaternion.LookRotation(Vector3.back);
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if(horizontalPos > mapWidth/2*(-1))
            {
                horizontalPos -= 1;
                // D�placer vers la gauche (axe X n�gatif)
                transform.position += Vector3.left * moveDistance;
                transform.rotation = Quaternion.LookRotation(Vector3.left);
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if(horizontalPos < mapWidth / 2) 
            {
                horizontalPos += 1;
                // D�placer vers la droite (axe X positif)
                transform.position += Vector3.right * moveDistance;
                transform.rotation = Quaternion.LookRotation(Vector3.right);
            }
        }
    }

    public int getAdvancement()
    {
        return verticalPos;
    }
}
