using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class rabbitMouvement : MonoBehaviour
{
    public float moveDistance = 3f; // Distance à déplacer
    private int horizontalPos = 0;
    private int verticalPos = 0;
    public int mapWidth;
    public int maxBackward;
    private int currentBackward = 0;
    private GameManager gameManager;
    private bool isOnLog = false;
    private GameObject currentLog;
    private float posOnLog = 0;
    private bool ignoreRiverCollsision = true;

    private void Start()
    {
        gameManager = gameObject.GetComponentInParent<GameManager>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // Déplacer en profondeur (axe Z positif)
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

            if(isOnLog)
            {
                ignoreRiverCollsision = true;
                horizontalPos = Mathf.RoundToInt(transform.position.x / moveDistance);
                transform.position = new Vector3(horizontalPos*moveDistance, transform.position.y, transform.position.z);
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (verticalPos > 0 && currentBackward<maxBackward)
            {
                verticalPos -= 1;
                currentBackward += 1;
                // Déplacer vers l'arrière (axe Z négatif)
                transform.position += Vector3.back * moveDistance;
                transform.rotation = Quaternion.LookRotation(Vector3.back);
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if(horizontalPos > mapWidth/2*(-1))
            {
                horizontalPos -= 1;
                // Déplacer vers la gauche (axe X négatif)
                transform.position += Vector3.left * moveDistance;
                transform.rotation = Quaternion.LookRotation(Vector3.left);
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if(horizontalPos < mapWidth / 2) 
            {
                horizontalPos += 1;
                // Déplacer vers la droite (axe X positif)
                transform.position += Vector3.right * moveDistance;
                transform.rotation = Quaternion.LookRotation(Vector3.right);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Log"))
        {
            isOnLog = true;
            currentLog = other.gameObject;

            LogMouvements logMouvements = currentLog.GetComponent<LogMouvements>();
            BoxCollider logCollider  = currentLog.GetComponent<BoxCollider>();
            if (logMouvements != null)
            {
                
                float logLength = logCollider.size.x;
                Debug.Log(transform.position.x + " - " + currentLog.transform.position.x + " - " + logLength);
                if(transform.position.x < currentLog.transform.position.x + logLength && transform.position.x>currentLog.transform.position.x-logLength)
                {
                    posOnLog = transform.position.x - currentLog.transform.position.x;
                }
                else
                {
                    HandlePlayerLoss();
                    
                }
            }
        }

        if (other.CompareTag("River") && !isOnLog)
        {
            if(ignoreRiverCollsision)
            {
                ignoreRiverCollsision = false;
            }
            else
            {
                HandlePlayerLoss();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Log"))
        {
            isOnLog = false;
            currentLog = null;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Log"))
        {
            transform.position = new Vector3(currentLog.transform.position.x + posOnLog, transform.position.y, transform.position.z);
            if(transform.position.x > moveDistance * mapWidth / 2 || transform.position.x < -moveDistance * mapWidth / 2)
            {
                HandlePlayerLoss();
            }
        }
    }


    void HandlePlayerLoss()
    {
        Debug.Log("Le joueur est tombé dans l'eau !");
        // Ajoutez ici votre logique de fin de jeu ou de perte de vie
    }


    public int getAdvancement()
    {
        return verticalPos;
    }
}
