using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreesManager : MonoBehaviour
{
    public List<GameObject> treesPrefab = new List<GameObject>();
    private List<GameObject> trees = new List<GameObject>();
    public float density;
    public float gap;
    public int roadWidth;

    public void Initialize(float density, List<bool>? preset)
    {
        if(preset != null)
        {
            for(int i=0; i < preset.Count; i++)
            {
                if (preset[i])
                {
                    createTree(i);
                }
            }
        }
        else
        {
            this.density = density;
            customStart();
        }
    }

    void customStart()
    {
        int totalCases = roadWidth;
        int numberOfTrees = Mathf.FloorToInt(totalCases * density);

        List<int> numbersList = new List<int>();
        for (int i = 1; i <= roadWidth; i++)
        {
            numbersList.Add(i);
        }

        for(int i =0; i < numberOfTrees || i==roadWidth-1; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0,numbersList.Count);
            createTree(numbersList[randomIndex]);
            numbersList.RemoveAt(i);
        }
    }

    void createTree(int xPos)
    {
        // Choisir un arbre aléatoire parmi les prefabs
        GameObject treePrefab = treesPrefab[Random.Range(0, treesPrefab.Count)];

        // Calculer la position de l'arbre
        float xOrigine = roadWidth / 2 * gap * (-1);
        float xPosition = xOrigine+ xPos * gap;
        Vector3 treePosition = new Vector3(xPosition, transform.position.y, transform.position.z);

        // Instancier l'arbre
        GameObject tree = Instantiate(treePrefab, treePosition, Quaternion.identity);
        trees.Add(tree);
    }

    private void OnDestroy()
    {
        foreach(GameObject tree in trees)
        {
            Destroy(tree);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
