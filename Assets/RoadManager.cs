using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    public bool isRightDirection = true; // Direction de la route : true pour droite, false pour gauche
    public List<GameObject> vehiclePrefabs; // Liste des pr�fabs de v�hicules
    public float minDistanceBetweenVehicles = 1f; // Distance minimale entre les v�hicules

    private List<GameObject> spawnedVehicles = new List<GameObject>();
    public float speed = 10f;
    private float requiredDistanceForNextCar = 0;
    public int mapWidth;

    void Update()
    {
        if (spawnedVehicles.Count == 0)
        {
            SpawnVehicle();
        }
        else
        {
            GameObject lastVehicle = spawnedVehicles[spawnedVehicles.Count - 1];
            float lastDistanceDone = lastVehicle.GetComponent<vehiculeMovements>().getDistanceDone();

            //Debug.Log(requiredDistanceForNextCar + " : " + lastDistanceDone);

            if (lastDistanceDone > lastVehicle.GetComponent<vehiculeMovements>().getVehiculeLength() + requiredDistanceForNextCar)
            {
                SpawnVehicle();
            }
        }
    }

    void SpawnVehicle()
    {
        // Choisir un v�hicule al�atoire
        GameObject vehiclePrefab = vehiclePrefabs[UnityEngine.Random.Range(0, vehiclePrefabs.Count)];

        // Position du v�hicule
        Vector3 spawnPosition;
        if(isRightDirection)
        {
            spawnPosition = new Vector3(transform.position.x- (mapWidth / 2)*3-5, transform.position.y, transform.position.z);
        }
        else
        {
            spawnPosition = new Vector3(transform.position.x+(mapWidth/2)*3+5, transform.position.y, transform.position.z);
        }

        Debug.Log(transform.position.z);

        // Instancier le v�hicule
        GameObject vehicle = Instantiate(vehiclePrefab, spawnPosition, Quaternion.identity);
        vehicle.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

        vehicle.GetComponent<vehiculeMovements>().speed = this.speed;
        vehicle.GetComponent<vehiculeMovements>().distanceMax = this.mapWidth*3+10;
        vehicle.GetComponent<vehiculeMovements>().isMouvingRight = this.isRightDirection;



        requiredDistanceForNextCar = UnityEngine.Random.Range(3f, 12f);

        // Ajouter le v�hicule � la liste des v�hicules spawn�s
        spawnedVehicles.Add(vehicle);
    }
}
