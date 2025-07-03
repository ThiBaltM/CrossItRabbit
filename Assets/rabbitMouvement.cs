using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class rabbitMouvement : MonoBehaviour
{
    public float moveDistance = 3f; // Distance à déplacer
    public float moveTime = 0.2f; // Temps pour effectuer le déplacement
    private int horizontalPos = 0;
    private int verticalPos = 0;
    public int mapWidth;
    public int maxBackward;
    private int currentBackward = 0;
    private int oldBackward = 0;
    private GameManager gameManager;
    private bool isOnLog = false;
    private GameObject currentLog;
    private float posOnLog = 0;
    private bool isAnimatingLoss = false;
    private bool isMoving = false;
    private bool dieFromWater = false;
    private int[] oldPos = new int[2];
    private bool hurtTree = false;
    public bool reaxe = false;
    public GameObject sphere;

    // Référence à l'Animator
    private Animator animator;
    private AudioSource audioSource;

    private void Start()
    {
        gameManager = gameObject.GetComponentInParent<GameManager>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {

        // Réinitialiser IsRunning à false si aucune touche n'est pressée
        if (!Input.anyKey && !isAnimatingLoss && !isMoving)
        {
            if (animator != null)
            {
                animator.SetBool("IsRunning", false);
            }
        }

        // Votre logique de mouvement
        if (Input.GetKeyDown(KeyCode.UpArrow) && !isAnimatingLoss && !isMoving)
        {
            prepareMouvement();
            verticalPos += 1;
            reaxe = (gameManager.getLayers()[this.verticalPos].GetComponent<WaterFlowManager>() == null);
            StartCoroutine(MovePlayer(Vector3.forward * moveDistance, Quaternion.LookRotation(Vector3.forward)));
            //reaxer s'il sort des buches

            if (currentBackward > 0)
            {
                currentBackward -= 1;
            }
            else
            {
                gameManager.deleteLayers();
                gameManager.createLayer();
            }
            if (isOnLog)
            {
                horizontalPos = Mathf.RoundToInt(transform.position.x / moveDistance);

                if (currentLog != null)
                {
                    posOnLog = horizontalPos * moveDistance - currentLog.transform.position.x;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && !isAnimatingLoss && !isMoving)
        {
            
            
            if (verticalPos > 0 && currentBackward < maxBackward)
            {
                prepareMouvement();
                verticalPos -= 1;
                oldBackward = currentBackward;
                currentBackward += 1;
                //reaxer s'il sort des buches
                sphere.transform.position = gameManager.getLayers()[this.verticalPos].transform.position;

                reaxe = (gameManager.getLayers()[this.verticalPos].GetComponent<WaterFlowManager>() == null);
                StartCoroutine(MovePlayer(Vector3.back * moveDistance, Quaternion.LookRotation(Vector3.back)));
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && !isAnimatingLoss && !isMoving)
        {
            prepareMouvement();
            reaxe = false;
            if (horizontalPos > mapWidth / 2 * (-1))
            {
                horizontalPos -= 1;
                StartCoroutine(MovePlayer(Vector3.left * moveDistance, Quaternion.LookRotation(Vector3.left)));
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && !isAnimatingLoss && !isMoving)
        {
            prepareMouvement();
            reaxe = false;
            if (horizontalPos < mapWidth / 2)
            {
                horizontalPos += 1;
                StartCoroutine(MovePlayer(Vector3.right * moveDistance, Quaternion.LookRotation(Vector3.right)));
            }
        }
    }
    private void prepareMouvement()
    {
        oldPos[0] = horizontalPos;
        oldPos[1] = verticalPos;
        if (animator != null) animator.SetBool("IsRunning", true);

    }


    // Coroutine pour déplacer le joueur de manière fluide
    IEnumerator MovePlayer(Vector3 direction, Quaternion targetRotation)
    {
        isMoving = true;
        animator.SetBool("isRunning", true);

        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;
        Vector3 targetPosition = startPosition + direction;
        if (reaxe)
        {
            horizontalPos = Mathf.RoundToInt(transform.position.x / moveDistance);
            targetPosition.x = horizontalPos*moveDistance;
        }
        

        while (elapsedTime < moveTime)
        {
            // Interpolation linéaire pour la position
            if (hurtTree)
            {
                targetPosition = new Vector3(oldPos[0] * moveDistance, transform.position.y, oldPos[1] * moveDistance);
                horizontalPos = oldPos[0];
                verticalPos = oldPos[1];
                if(direction == Vector3.back * moveDistance)
                {
                    currentBackward = oldBackward;
                }
            }
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveTime);
            // Interpolation sphérique pour la rotation (plus fluide que Lerp pour les rotations)
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / moveTime*2);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // S'assurer que la position et la rotation finales sont exactement celles désirées
        transform.position = targetPosition;
        transform.rotation = targetRotation;

        hurtTree = false;
        isMoving = false;

        animator.SetBool("isRunning", false);
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Log"))
        {
            isOnLog = true;
            currentLog = other.gameObject;
            LogMouvements logMouvements = currentLog.GetComponent<LogMouvements>();
            BoxCollider logCollider = currentLog.GetComponent<BoxCollider>();
            if (logMouvements != null && logCollider != null)
            {
                float logLength = logCollider.size.x;
                if (transform.position.x < currentLog.transform.position.x + logLength && transform.position.x > currentLog.transform.position.x - logLength)
                {
                    posOnLog = transform.position.x - currentLog.transform.position.x;
                }
                else
                {
                    HandlePlayerLoss();
                }
            }
        }
        if (other.CompareTag("River") && !isOnLog && !isMoving)
        {
            dieFromWater = true;
            HandlePlayerLoss();
            
        }
        if (other.CompareTag("Car") && !isMoving)
        {
            HandlePlayerLoss();
        }
        if (other.CompareTag("Tree"))
        {
            hurtTree = true;
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
        if (other.CompareTag("Log") && !isAnimatingLoss && !isMoving)
        {
            // Mettre à jour la position du joueur sur la bûche en mouvement
            if (currentLog != null)
            {
                float newXPos = currentLog.transform.position.x + posOnLog;
                Vector3 newPosition = new Vector3(newXPos, transform.position.y, transform.position.z);
                // Déplacer le joueur avec un Lerp pour un effet plus fluide
                transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * 10f);

                if (transform.position.x > moveDistance * mapWidth / 2 || transform.position.x < -moveDistance * mapWidth / 2)
                {
                    HandlePlayerLoss();
                }
            }
        }

        if (other.CompareTag("River") && !isOnLog && !isMoving)
        {
            dieFromWater = true;
            HandlePlayerLoss();

        }

        if (other.CompareTag("Car") && !isMoving)
        {
            HandlePlayerLoss();
        }
    }

    void HandlePlayerLoss()
    {
        if (isAnimatingLoss) return;

        animator.SetBool("isDying",true); 
        isAnimatingLoss = true;
        StartCoroutine(PlayerLossAnimation());
    }

    IEnumerator PlayerLossAnimation()
    {
        yield return new WaitForSeconds(2f); // Temps par défaut si pas d'animator
        
        isAnimatingLoss = false;
    }


    public int getAdvancement()
    {
        return verticalPos;
    }
}
