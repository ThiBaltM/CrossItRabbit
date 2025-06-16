using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    public bool isRightDirection = true; // Direction de la route : true pour droite, false pour gauche
    public List<GameObject> vehiclePrefabs; // Liste des pr�fabs de v�hicules
    public float minDistanceBetweenVehicles = 1f; // Distance minimale entre les v�hicules

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

            // Supposons que chaque v�hicule a une vitesse d�finie dans un script attach�
            float vehicleSpeed = lastVehicle.GetComponent<VehiculeMovement>().speed;

            // Calculer la distance n�cessaire en fonction de la vitesse et du temps �coul�
            float requiredDistance = vehicleSpeed * Time.deltaTime;

            if (distanceSinceLastVehicle >= minDistanceBetweenVehicles + requiredDistance + Random.Range(0f, 4f))
            {
                SpawnVehicle();
            }
        }
    }

    void SpawnVehicle()
    {
        // Choisir un v�hicule al�atoire
        GameObject vehiclePrefab = vehiclePrefabs[Random.Range(0, vehiclePrefabs.Count)];

        // Position du v�hicule
        Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y, lastVehicleZPosition);

        // Instancier le v�hicule
        GameObject vehicle = Instantiate(vehiclePrefab, spawnPosition, Quaternion.identity);

        // Ajuster la rotation en fonction de la direction de la route
        if (!isRightDirection)
        {
            vehicle.transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        // Mettre � jour la position Z du dernier v�hicule
        lastVehicleZPosition = spawnPosition.z + vehicle.GetComponent<Renderer>().bounds.size.z;

        // Ajouter le v�hicule � la liste des v�hicules spawn�s
        spawnedVehicles.Add(vehicle);
    }
}
