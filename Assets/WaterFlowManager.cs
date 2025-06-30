using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFlowManager : MonoBehaviour
{
    public bool isRightDirection = true; // Direction de la route : true pour droite, false pour gauche
    public GameObject logPrefab; // Liste des préfabs de véhicules
    public float minDistanceBetweenVehicles = 1f; // Distance minimale entre les véhicules

    public float logYOffset;

    private List<GameObject> spawnedVehicles = new List<GameObject>();
    public float speed = 10f;
    private float requiredDistanceForNextCar = 0;
    public int mapWidth;
    // Start is called before the first frame update
    void Start()
    {
        SpawnLog(UnityEngine.Random.Range(mapWidth, mapWidth * 3));
        SpawnLog(UnityEngine.Random.Range(0, mapWidth));
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnedVehicles.Count == 0)
        {
            SpawnLog(null);
        }
        else
        {
            GameObject lastVehicle = spawnedVehicles[spawnedVehicles.Count - 1];
            float lastDistanceDone = lastVehicle.GetComponent<LogMouvements>().getDistanceDone();

            //Debug.Log(requiredDistanceForNextCar + " : " + lastDistanceDone);

            if (lastDistanceDone > lastVehicle.GetComponent<LogMouvements>().getLogLength() + requiredDistanceForNextCar)
            {
                SpawnLog(null);
            }
        }
    }

    void SpawnLog(float? advancement)
    {
        // Choisir un véhicule aléatoire
        GameObject vehiclePrefab = logPrefab;

        // Position du véhicule
        Vector3 spawnPosition;
        if (isRightDirection)
        {
            if (advancement == null)
            {
                spawnPosition = new Vector3(transform.position.x - (mapWidth / 2) * 3 - 5, transform.position.y - logYOffset, transform.position.z);
            }
            else
            {
                spawnPosition = new Vector3(transform.position.x - (mapWidth / 2) * 3 - 5 + advancement.Value, transform.position.y-logYOffset, transform.position.z);
            }
        }
        else
        {
            if (advancement == null)
            {
                spawnPosition = new Vector3(transform.position.x + (mapWidth / 2) * 3 + 5, transform.position.y - logYOffset, transform.position.z);
            }
            else
            {
                spawnPosition = new Vector3(transform.position.x + (mapWidth / 2) * 3 + 5 - advancement.Value, transform.position.y - logYOffset, transform.position.z);
            }
        }

        // Instancier le véhicule
        GameObject vehicle = Instantiate(vehiclePrefab, spawnPosition, Quaternion.identity);
        vehicle.transform.localScale = new Vector3(UnityEngine.Random.Range(0.5f, 2f), 4f, 4f);

        vehicle.GetComponent<LogMouvements>().speed = this.speed;
        vehicle.GetComponent<LogMouvements>().distanceMax = this.mapWidth * 3 + 10;
        vehicle.GetComponent<LogMouvements>().isMouvingRight = this.isRightDirection;



        requiredDistanceForNextCar = UnityEngine.Random.Range(3f, 12f);

        // Ajouter le véhicule à la liste des véhicules spawnés
        spawnedVehicles.Add(vehicle);
    }

    private void OnDestroy()
    {
        foreach (var vehicle in spawnedVehicles)
        {
            Destroy(vehicle.gameObject);
        }
    }
}
