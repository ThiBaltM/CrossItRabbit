using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Dictionary<int,GameObject> layers = new Dictionary<int, GameObject>();
    public GameObject rabbitObject;
    public GameObject road;
    private rabbitMouvement rabbitMouvement;
    public float gap = 3;
    private int currentLayer = 0;
    // Start is called before the first frame update
    void Start()
    {
        for(int i=0; i<12; i++)
        {
            layers[i] = Instantiate(road);
            layers[i].transform.position = new Vector3(0,0,gap*i);
            currentLayer += 1;
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
        layers[currentLayer] = Instantiate (road);
        layers[currentLayer].transform.position = new Vector3(0, 0, gap * currentLayer);
        currentLayer += 1;
    }
}
