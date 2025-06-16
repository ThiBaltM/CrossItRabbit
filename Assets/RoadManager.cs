using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    public bool isRightDirection = true; // Direction de la route : true pour droite, false pour gauche
    public List<GameObject> vehiclePrefabs; // Liste des préfabs de véhicules
    public float minDistanceBetweenVehicles = 1f; // Distance minimale entre les véhicules

    private List<GameObject> spawnedVehicles = new List<GameObject>();
    private float lastVehicleZPosition = 0f;

    void Update()
    {
        if (spawnedVehicles.Count == 0)
        {
            SpawnVehicle();
        }
        else
        {
            GameObject lastVehicle = spawnedVehicles[spawnedVehicles.Count - 1];
            float distanceSinceLastVehicle = Mathf.Abs(lastVehicle.transform.position.z - lastVehicleZPosition);

            // Supposons que chaque véhicule a une vitesse définie dans un script attaché
            float vehicleSpeed = lastVehicle.GetComponent<VehiculeMovement>().speed;

            // Calculer la distance nécessaire en fonction de la vitesse et du temps écoulé
            float requiredDistance = vehicleSpeed * Time.deltaTime;

            if (distanceSinceLastVehicle >= minDistanceBetweenVehicles + requiredDistance + Random.Range(0f, 4f))
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
        Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y, lastVehicleZPosition);

        // Instancier le véhicule
        GameObject vehicle = Instantiate(vehiclePrefab, spawnPosition, Quaternion.identity);

        // Ajuster la rotation en fonction de la direction de la route
        if (!isRightDirection)
        {
            vehicle.transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        // Mettre à jour la position Z du dernier véhicule
        lastVehicleZPosition = spawnPosition.z + vehicle.GetComponent<Renderer>().bounds.size.z;

        // Ajouter le véhicule à la liste des véhicules spawnés
        spawnedVehicles.Add(vehicle);
    }
}
