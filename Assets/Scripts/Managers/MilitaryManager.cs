using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilitaryManager : MonoBehaviour
{
    public static MilitaryManager Instance { get; private set; }

    public int militaryPoints = 0;
    public int maxTanks = 50;
    public Transform player;
    public GameObject tankPrefab;

    private int totalSpawnedTanks = 0;
    private int lastMilestone = 0; // Keeps track of the last milestone for spawning tanks
    private List<GameObject> activeTanks = new List<GameObject>();

    public void Awake()
    {
        if (Instance == null)
            Instance = this;

        if (Instance != this)
            Destroy(gameObject);
    }

    private void Update()
    {
        CheckSpawnConditions();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void AddMilitaryPoints(int points)
    {
        militaryPoints += points;
        Debug.Log($"Military Points: {militaryPoints}");
    }

    private void CheckSpawnConditions()
    {
        // Calculate the milestone difference since the last spawn
        int milestone = militaryPoints / 5;
        if (milestone > lastMilestone)
        {
            // Determine number of tanks to spawn
            int tanksToSpawn = milestone > 10 ? 2 : 1; // 2 tanks if militaryPoints > 50 (milestone > 10)

            // Spawn tanks
            SpawnTanks(tanksToSpawn * (milestone - lastMilestone));

            // Update the last milestone
            lastMilestone = milestone;
        }
    }

    private void SpawnTanks(int count)
    {
        Camera mainCamera = Camera.main;

        for (int i = 0; i < count && totalSpawnedTanks < maxTanks; i++)
        {
            Vector3 spawnPosition = new Vector3(
                Random.Range(mainCamera.transform.position.x + 50f, mainCamera.transform.position.x + 100),
                1.3f,
                0f
            );

            GameObject newTank = Instantiate(tankPrefab, spawnPosition, Quaternion.identity);
            activeTanks.Add(newTank);
            totalSpawnedTanks++;
        }
    }
}
