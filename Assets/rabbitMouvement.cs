using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rabbitMouvement : MonoBehaviour
{
    public float moveDistance = 3f; // Distance � d�placer
    public float moveTime = 0.2f; // Temps pour effectuer le d�placement
    private int horizontalPos = 0;
    private int verticalPos = 0;
    public int mapWidth;
    public int maxBackward;
    private int currentBackward = 0;
    private GameManager gameManager;
    private bool isOnLog = false;
    private GameObject currentLog;
    private float posOnLog = 0;
    private bool isAnimatingLoss = false;
    private bool isMoving = false;
    private bool dieFromWater = false;

    // R�f�rence � l'Animator
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
        // R�initialiser IsRunning � false si aucune touche n'est press�e
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
            if (animator != null) animator.SetBool("IsRunning", true);
            StartCoroutine(MovePlayer(Vector3.forward * moveDistance, Quaternion.LookRotation(Vector3.forward)));
            verticalPos += 1;
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
                Vector3 newPosition = new Vector3(horizontalPos * moveDistance, transform.position.y, transform.position.z);
                // Si le joueur est sur une b�che, nous devons utiliser une autre approche pour le d�placement
                // car nous ne pouvons pas simplement t�l�porter le joueur
                // Nous devons d�caler posOnLog pour correspondre � la nouvelle position
                if (currentLog != null)
                {
                    posOnLog = horizontalPos * moveDistance - currentLog.transform.position.x;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && !isAnimatingLoss && !isMoving)
        {
            if (animator != null) animator.SetBool("IsRunning", true);
            if (verticalPos > 0 && currentBackward < maxBackward)
            {
                verticalPos -= 1;
                currentBackward += 1;
                StartCoroutine(MovePlayer(Vector3.back * moveDistance, Quaternion.LookRotation(Vector3.back)));
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && !isAnimatingLoss && !isMoving)
        {
            if (animator != null) animator.SetBool("IsRunning", true);
            if (horizontalPos > mapWidth / 2 * (-1))
            {
                horizontalPos -= 1;
                StartCoroutine(MovePlayer(Vector3.left * moveDistance, Quaternion.LookRotation(Vector3.left)));
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && !isAnimatingLoss && !isMoving)
        {
            if (animator != null) animator.SetBool("IsRunning", true);
            if (horizontalPos < mapWidth / 2)
            {
                horizontalPos += 1;
                StartCoroutine(MovePlayer(Vector3.right * moveDistance, Quaternion.LookRotation(Vector3.right)));
            }
        }
    }

    // Coroutine pour d�placer le joueur de mani�re fluide
    IEnumerator MovePlayer(Vector3 direction, Quaternion targetRotation)
    {
        isMoving = true;
        animator.SetBool("isRunning", true);

        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;
        Vector3 targetPosition = startPosition + direction;

        // Si le joueur est sur une b�che, nous devons ajuster la position cible
        if (isOnLog && currentLog != null)
        {
            // Calculer la nouvelle position sur la b�che
            float newXPos = startPosition.x + direction.x;
            float newPosOnLog = newXPos - currentLog.transform.position.x;

            // Limiter le mouvement pour que le joueur reste sur la b�che
            BoxCollider logCollider = currentLog.GetComponent<BoxCollider>();
            if (logCollider != null)
            {
                float logLength = logCollider.size.x;
                float logX = currentLog.transform.position.x;
                float halfLogLength = logLength / 2f;

                // Limiter newXPos pour qu'il reste dans les limites de la b�che
                newXPos = Mathf.Clamp(newXPos, logX - halfLogLength, logX + halfLogLength);
                targetPosition.x = newXPos;
                posOnLog = newXPos - logX;
            }
        }

        while (elapsedTime < moveTime)
        {
            // Interpolation lin�aire pour la position
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveTime);
            // Interpolation sph�rique pour la rotation (plus fluide que Lerp pour les rotations)
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / moveTime*2);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // S'assurer que la position et la rotation finales sont exactement celles d�sir�es
        transform.position = targetPosition;
        transform.rotation = targetRotation;

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
            // Mettre � jour la position du joueur sur la b�che en mouvement
            if (currentLog != null)
            {
                float newXPos = currentLog.transform.position.x + posOnLog;
                Vector3 newPosition = new Vector3(newXPos, transform.position.y, transform.position.z);
                // D�placer le joueur avec un Lerp pour un effet plus fluide
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
        Debug.Log("Le joueur est tomb� dans l'eau !");
        isAnimatingLoss = true;
        StartCoroutine(PlayerLossAnimation());
    }

    IEnumerator PlayerLossAnimation()
    {
        yield return new WaitForSeconds(2f); // Temps par d�faut si pas d'animator
        
        isAnimatingLoss = false;
    }


    public int getAdvancement()
    {
        return verticalPos;
    }
}
