using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Dictionary<int,GameObject> layers = new Dictionary<int, GameObject>();
    public GameObject rabbitObject;
    public GameObject road;
    public GameObject treeLayer;
    public GameObject waterLayer;
    public ForestManager forestManager;
    public ScoreManager scoreManager;
    private rabbitMouvement rabbitMouvement;
    public float gap = 3;
    public int roadWidth = 13;
    private int currentLayer = 0;

    private List<List<bool>> treesRemaining = new List<List<bool>>();

    private int smallRoadPattern = 0;
    private int largeRoadPattern = 0;
    private int highwayPattern = 0;
    private int waterfallsPattern = 0;
    private bool waterfallsRight = false;
    // Start is called before the first frame update
    void Start()
    {
        //first layer is empty
        treesRemaining.Add(new List<bool>());
        for(int i = 0; i < roadWidth; i++)
        {
            treesRemaining[0].Add(false);
        }
        createLayer();
        treesRemaining = forestManager.generateForest(0.8f, 5, 2);
        for(int i=0; i<10; i++)
        {
            createLayer();
        }
        rabbitMouvement = rabbitObject.GetComponent<rabbitMouvement>();

    }

    // Update is called once per frame
    void Update()
    {
        scoreManager.scoreValue = this.rabbitMouvement.getAdvancement();
    }

    public void deleteLayers()
    {
        int seuil = rabbitMouvement.getAdvancement() - 4;
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
        if (treesRemaining.Count > 0) // forest
        {
            layers[currentLayer] = Instantiate(treeLayer, new Vector3(0, 0, gap * currentLayer), Quaternion.identity);
            layers[currentLayer].GetComponent<TreesManager>().Initialize(1, treesRemaining[0]);
            treesRemaining.RemoveAt(0);
        }
        else if (smallRoadPattern > 0) //simple road
        {
            if (smallRoadPattern == 1)
            {
                layers[currentLayer] = Instantiate(road, new Vector3(0, 0, gap * currentLayer), Quaternion.identity);
                layers[currentLayer].GetComponent<RoadManager>().isRightDirection = false;
                layers[currentLayer].GetComponent<RoadManager>().speed = UnityEngine.Random.Range(3, 8);
            }
            else
            {
                layers[currentLayer] = Instantiate(road, new Vector3(0, 0, gap * currentLayer), Quaternion.identity);
                layers[currentLayer].GetComponent<RoadManager>().speed = UnityEngine.Random.Range(3, 8);
            }
            smallRoadPattern -= 1;
        }
        else if (largeRoadPattern > 0) // large road
        {
            if (largeRoadPattern < 3)
            {
                layers[currentLayer] = Instantiate(road, new Vector3(0, 0, gap * currentLayer), Quaternion.identity);
                layers[currentLayer].GetComponent<RoadManager>().isRightDirection = false;
                layers[currentLayer].GetComponent<RoadManager>().speed = UnityEngine.Random.Range(4, 10);
            }
            else if (largeRoadPattern < 4)
            {
                layers[currentLayer] = Instantiate(treeLayer, new Vector3(0, 0, gap * currentLayer), Quaternion.identity);
                layers[currentLayer].GetComponent<TreesManager>().Initialize(UnityEngine.Random.Range(0.1f, 0.8f), null);
            }
            else
            {
                layers[currentLayer] = Instantiate(road, new Vector3(0, 0, gap * currentLayer), Quaternion.identity);
                layers[currentLayer].GetComponent<RoadManager>().speed = UnityEngine.Random.Range(4, 10);
            }
            largeRoadPattern -= 1;
        }
        else if (highwayPattern > 0) //highway
        {
            if (highwayPattern < 5)
            {
                layers[currentLayer] = Instantiate(road, new Vector3(0, 0, gap * currentLayer), Quaternion.identity);
                layers[currentLayer].GetComponent<RoadManager>().isRightDirection = false;
                layers[currentLayer].GetComponent<RoadManager>().speed = UnityEngine.Random.Range(6, 14);
            }
            else if (highwayPattern < 6)
            {
                layers[currentLayer] = Instantiate(treeLayer, new Vector3(0, 0, gap * currentLayer), Quaternion.identity);
                layers[currentLayer].GetComponent<TreesManager>().Initialize(UnityEngine.Random.Range(0.1f, 0.8f), null);
            }
            else
            {
                layers[currentLayer] = Instantiate(road, new Vector3(0, 0, gap * currentLayer), Quaternion.identity);
                layers[currentLayer].GetComponent<RoadManager>().speed = UnityEngine.Random.Range(6, 14);
            }
            highwayPattern -= 1;
        }
        else if (waterfallsPattern > 0) // rivers
        {
            layers[currentLayer] = Instantiate(waterLayer, new Vector3(0, -0.5f, gap * currentLayer), Quaternion.identity);
            layers[currentLayer].GetComponent<WaterFlowManager>().isRightDirection = waterfallsRight;
            layers[currentLayer].GetComponent<WaterFlowManager>().speed = UnityEngine.Random.Range(1, 9);
            waterfallsPattern -= 1;
        }
        else // egenrate next steps
        {
            layers[currentLayer] = Instantiate(treeLayer, new Vector3(0, 0, gap * currentLayer), Quaternion.identity);
            int choice = 6;//UnityEngine.Random.Range(0, 6);
            if (choice < 2)
            {
                treesRemaining = forestManager.generateForest(0.9f, UnityEngine.Random.Range(3, 8), UnityEngine.Random.Range(1, 3));
            }
            else if (choice == 3)
            {
                smallRoadPattern = 2;
            }
            else if (choice == 4)
            {
                largeRoadPattern = 5;
            }
            else if (choice == 5)
            {
                highwayPattern = 9;
            }
            else
            {
                waterfallsPattern = UnityEngine.Random.Range(1, 4);
                waterfallsRight = UnityEngine.Random.Range(0, 1)==1;
            }
        }
        currentLayer += 1;
    }

    public Dictionary<int,GameObject> getLayers()
    {
        return layers;
    }
}
