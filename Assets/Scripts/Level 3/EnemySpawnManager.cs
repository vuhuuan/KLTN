using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    public GameObject rabbitPrefab;

    public GameObject bonePrefab;

    public int minRabbits = 5;
    public int maxRabbits = 10;
    public float x1Range = 0f;
    public float x2Range = 10f;
    public float z1Range = 0f;
    public float z2Range = 10f;

    void Start()
    {
        if (GameObject.Find("UI"))
        {
            SpawnRabbits();
            SpawnBones();
        }
    }

    void SpawnRabbits()
    {
        int rabbitCount = Random.Range(minRabbits, maxRabbits + 1);
        GameObject rabbitsContainer = new GameObject("Rabbits");

        for (int i = 0; i < rabbitCount; i++)
        {
            float randomX = Random.Range(x1Range, x2Range);
            float randomZ = Random.Range(z1Range, z2Range);

            Vector3 spawnPosition = new Vector3(randomX, 10f, randomZ);

            GameObject rabbit = Instantiate(rabbitPrefab, spawnPosition, Quaternion.identity);

            rabbit.transform.parent = rabbitsContainer.transform;
        }
    }
    public void SpawnBones()
    {
        int bonesCount = Random.Range(5, 15);
        GameObject bonesContainer = new GameObject("Bones");

        for (int i = 0; i < bonesCount; i++)
        {
            float randomX = Random.Range(100f, 280f);
            float randomZ = Random.Range(110f, 150f);

            Vector3 spawnPosition = new Vector3(randomX, 10f, randomZ);

            GameObject bone = Instantiate(bonePrefab, spawnPosition, Quaternion.identity);

            bone.transform.parent = bonesContainer.transform;
        }
    }
}

