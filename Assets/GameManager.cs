using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Dictionary<int,GameObject> layers = new Dictionary<int, GameObject>();
    public GameObject rabbitObject;
    public GameObject road;
    public GameObject treeLayer;
    public ForestManager forestManager;
    private rabbitMouvement rabbitMouvement;
    public float gap = 3;
    private int currentLayer = 0;
    private List<List<bool>> treesRemaining = new List<List<bool>>();
    // Start is called before the first frame update
    void Start()
    {
        treesRemaining = forestManager.generateForest(0.8f, 10, 1);
        for(int i=0; i<10; i++)
        {
            createLayer();
        }
        rabbitMouvement = rabbitObject.GetComponent<rabbitMouvement>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void deleteLayers()
    {
        int seuil = rabbitMouvement.getAdvancement() - 2;
        List<int> keysToRemove = new List<int>();

        foreach (int a in layers.Keys)
        {
            if (a < seuil)
            {
                keysToRemove.Add(a);
            }
        }

        foreach (int key in keysToRemove)
        {
            Destroy(layers[key]); // Détruit le GameObject associé
            layers.Remove(key); // Supprime l'entrée du dictionnaire
        }
    }

    public void createLayer()
    {
        if(treesRemaining.Count > 0)
        {
            layers[currentLayer] = Instantiate(treeLayer, new Vector3(0, 0, gap * currentLayer), Quaternion.identity);
            layers[currentLayer].GetComponent<TreesManager>().Initialize(1, treesRemaining[0]);
            treesRemaining.RemoveAt(0);
        }
        /*
        if(currentLayer%6< 2)
        {
            layers[currentLayer] = Instantiate(road, new Vector3(0, 0, gap * currentLayer), Quaternion.identity);
            layers[currentLayer].GetComponent<RoadManager>().speed = UnityEngine.Random.Range(4, 12);
        }else if(currentLayer%6 < 4)
        {
            layers[currentLayer] = Instantiate(road, new Vector3(0, 0, gap * currentLayer), Quaternion.identity);
            layers[currentLayer].GetComponent<RoadManager>().speed = UnityEngine.Random.Range(4, 12);
            layers[currentLayer].GetComponent<RoadManager>().isRightDirection = false;
        }
        else
        {
            layers[currentLayer] = Instantiate(treeLayer, new Vector3(0, 0, gap * currentLayer), Quaternion.identity);
        }
        */
        currentLayer += 1;
    }
}
