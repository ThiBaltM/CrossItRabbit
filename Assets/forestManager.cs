using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class forestManager : MonoBehaviour
{
    public List<GameObject> treePrefabs;
    public List<GameObject> trees;
    public int forestWidth = 13;
    public int forestHeight = 5;
    public float gap = 3;
    public GameObject groundPlane;

    private List<List<bool>> forestMatrix; // true = arbre, false = chemin

    // Start is called before the first frame update
    void Start()
    {
        groundPlane.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, 0.3f*forestHeight);
        forestMatrix = new List<List<bool>>();

        // Initialiser la matrice avec des arbres partout
        for (int y = 0; y < forestHeight; y++)
        {
            List<bool> row = new List<bool>();
            for (int x = 0; x < forestWidth; x++)
            {
                row.Add(true); // Par défaut, chaque case contient un arbre
            }
            forestMatrix.Add(row);
        }

        // Générer un chemin aléatoire
        GenerateRandomPath();

        createTrees();
    }

    void GenerateRandomPath()
    {
        // Choisir une position aléatoire en haut de la matrice
        int startX = Random.Range(0, forestWidth);
        int currentX = startX;
        int currentY = 0;

        // Marquer la position de départ comme chemin
        forestMatrix[currentY][currentX] = false;

        // Générer le chemin
        while(currentY< forestHeight-1)
        {
            int direction = Random.Range(-1, 2); // -1, 0, ou 1
            if(direction == 0)
            {
                currentY++;
            }
            else
            {
                currentX += direction;
                currentX = Mathf.Clamp(currentX, 0, forestWidth - 1);
            }
            Debug.Log(currentX + ":" + currentY);
            forestMatrix[currentY][currentX] = false;

        }
    }


    void createTrees()
    {
        float verticalOrigine = forestHeight * gap / 2 * (-1) + transform.position.z + gap/2;
        float horizontalOrigine = forestWidth * gap / 2 * (-1)+ gap/2;
        for(int ligne=0;ligne< forestMatrix.Count; ligne++)
        {
            float verticalPos = verticalOrigine + ligne * gap;
            for(int col = 0; col < forestMatrix[0].Count; col++)
            {
                if (forestMatrix[ligne][col])
                {
                    float horizontalPos = horizontalOrigine + col * gap;

                    GameObject treePrefab = treePrefabs[Random.Range(0, treePrefabs.Count)];
                    Vector3 treePosition = new Vector3(horizontalPos, transform.position.y, verticalPos);
                    GameObject tree = Instantiate(treePrefab, treePosition, Quaternion.identity);
                    trees.Add(tree);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
