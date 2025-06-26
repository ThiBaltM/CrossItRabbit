using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    public bool isRightDirection = true; // Direction de la route : true pour droite, false pour gauche
    public List<GameObject> vehiclePrefabs; // Liste des préfabs de véhicules
    public float minDistanceBetweenVehicles = 1f; // Distance minimale entre les véhicules

    private List<GameObject> spawnedVehicles = new List<GameObject>();
    public float speed = 10f;
    private float requiredDistanceForNextCar = 0;

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

            // Calculer la distance nécessaire en fonction de la vitesse et du temps écoulé
            requiredDistanceForNextCar = lastVehicle.GetComponent<vehiculeMovements>().getVehiculeLength() + Random.Range(1f, 4f);

            if (lastDistanceDone > requiredDistanceForNextCar)
            {
                SpawnVehicle();
            }
        }
    }

    void SpawnVehicle()
    {
        // Choisir un véhicule aléatoire
        GameObject vehiclePrefab = vehiclePrefabs[Random.Range(0, vehiclePrefabs.Count)];

        // Position du véhicule
        Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        // Instancier le véhicule
        GameObject vehicle = Instantiate(vehiclePrefab, spawnPosition, Quaternion.identity);
        vehicle.GetComponent<vehiculeMovements>().speed = this.speed;

        requiredDistanceForNextCar = vehicle.GetComponent<vehiculeMovements>().getVehiculeLength() + Random.Range(1f, 4f);

        // Ajouter le véhicule à la liste des véhicules spawnés
        spawnedVehicles.Add(vehicle);
    }
}
