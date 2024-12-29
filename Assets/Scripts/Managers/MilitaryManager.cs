using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilitaryManager : MonoBehaviour
{
    public static MilitaryManager Instance { get; private set; }

    public int militaryPoints = 0;
    public int maxTanks = 10; 
    public Transform player; 
    public GameObject tankPrefab; 

    private int totalSpawnedTanks = 0; 
    private int[] thresholds = { 10, 15, 20, 30 }; 
    private Dictionary<int, int> tankSpawnIncrements = new Dictionary<int, int>
    {
        { 10, 1 }, { 15, 1 }, { 20, 2 }, { 30, 3 }
    };
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
        foreach (int threshold in thresholds)
        {
            if (militaryPoints >= threshold && totalSpawnedTanks < maxTanks)
            {
                int tanksToSpawn = tankSpawnIncrements[threshold];
                SpawnTanks(tanksToSpawn);
                thresholds = System.Array.FindAll(thresholds, t => t > militaryPoints);
                break;
            }
        }
    }

    private void SpawnTanks(int count)
    {
        Camera mainCamera = Camera.main;

        for (int i = 0; i < count && totalSpawnedTanks < maxTanks; i++)
        {
            Vector3 spawnPosition = new Vector3(mainCamera.transform.position.x + 20f,1.3f,0f);

            GameObject newTank = Instantiate(tankPrefab, spawnPosition, Quaternion.identity);
            activeTanks.Add(newTank);
            totalSpawnedTanks++;
        }
    }
}
