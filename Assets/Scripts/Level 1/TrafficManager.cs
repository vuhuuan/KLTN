using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class TrafficManager : MonoBehaviour
{
    [SerializeField] private bool beforeCutScene;

    // Start is called before the first frame update
    [SerializeField] private List<GameObject> cars_1;
    [SerializeField] private List<GameObject> cars_2;

    [SerializeField] private float spawnRate = 5f;

    private float removeRange1;

    void Start()
    {
        beforeCutScene = true;
        InvokeRepeating("SpawnCar", 1, spawnRate);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void SpawnCar ()
    {
        if (beforeCutScene) //before cut scene
        {
            float xSpawn = Random.Range(-321f, -326f);
            float ySpawn = 69.76f;
            float zSpawn = -74f;

            Vector3 SpawnPos = new Vector3(xSpawn, ySpawn, zSpawn);

            int carIndex = Random.Range(0, cars_1.Count);

            Instantiate(cars_1[carIndex], SpawnPos, cars_1[carIndex].transform.rotation);

        } else //after cut scene
        {
            spawnRate = 2f;
            float xSpawn = -250f;
            float ySpawn = 69.76f;
            float zSpawn = Random.Range(5.5f, 14f);

            Vector3 SpawnPos = new Vector3(xSpawn, ySpawn, zSpawn);

            int carIndex = Random.Range(0, cars_2.Count);

            Instantiate(cars_2[carIndex], SpawnPos, cars_2[carIndex].transform.rotation);
        }
    }

    public void SetBeforeCutScene(bool setValue)
    {
        beforeCutScene = setValue;
    }
}
