using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawnManager : MonoBehaviour
{
    public List<GameObject> allNPCs = new List<GameObject>();
    public GameObject npcPrefab;
    public int maxNPCs = 10;
    public float spawnRate = 1.0f;
    public List<Transform> spawnPoints = new List<Transform>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnPoints.Clear();
        GameObject[] spawnPointObjects = GameObject.FindGameObjectsWithTag("NPCSpawnPoint");
        foreach (GameObject obj in spawnPointObjects)
        {
            spawnPoints.Add(obj.transform);
        }
        SpawnInitialNPCs();
        StartCoroutine(SpawnNPCs());
    }

    IEnumerator SpawnNPCs()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnRate);
            if (allNPCs.Count < maxNPCs && spawnPoints.Count > 0)
            {
                Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
                Collider[] hit = Physics.OverlapSphere(randomSpawnPoint.position, 8.0f);
                bool canSpawn = true;
                foreach (Collider col in hit)
                {
                    if (col.CompareTag("AI") || col.CompareTag("Mom"))
                    {
                        canSpawn = false;
                        // Debug.Log("Spawn point occupied, skipping spawn.");
                        yield return null;
                    }
                }

                if (canSpawn)
                {
                    SpawnNPC(randomSpawnPoint);
                }
                
            }
            allNPCs.RemoveAll(obj => obj == null);

        }
    }

    public void SpawnNPC(Transform spawnpoint)
    {
        if (allNPCs.Count >= maxNPCs)
        {
            Debug.Log("Maximum NPC limit reached. Cannot spawn more NPCs.");
            return;
        }
        GameObject newNPC = Instantiate(npcPrefab, spawnpoint.position, spawnpoint.rotation);
        allNPCs.Add(newNPC);
    }

    public void SpawnInitialNPCs()
    {
        for (int i = 0; i < spawnPoints.Count; i++)
        {
            if (spawnPoints.Count == 0) break;
            SpawnNPC(spawnPoints[i]);
        }
    }
}
