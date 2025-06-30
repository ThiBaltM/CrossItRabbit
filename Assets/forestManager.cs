using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ForestManager : MonoBehaviour
{
    public List<GameObject> treePrefabs;
    public List<GameObject> trees;
    public int forestWidth = 13;
    public int forestHeight = 5;
    public float gap = 3;
    public GameObject groundPlane;

    private List<List<bool>> forestMatrix; // true = arbre, false = chemin

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
            forestMatrix[currentY][currentX] = false;

        }
    }

    public List<List<bool>> generateForest(float density, int length,int numberOfPaths)
    {
        forestMatrix = new List<List<bool>>();
        this.forestHeight = length;

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

        for(int i = 0; i < numberOfPaths; i++)
        {
            GenerateRandomPath();
        }

        int totaltrees = length * this.forestWidth;
        int finalTrees = Mathf.RoundToInt(totaltrees* density);

        for(int i = 0;i<(totaltrees-finalTrees);i++)
        {
            forestMatrix[Random.Range(0, length)][Random.Range(0, forestWidth)] = false;
        }

        return this.forestMatrix;
    }
}
