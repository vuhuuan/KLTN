using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager2 : MonoBehaviour
{
    //scene
    public int sceneIndex;
    public int cutSceneIndex;

    //score
    public float scoreTime;

    //player
    public GameObject currentPlayer;
    public GameObject currentEnemy;

    public int playerLevel;
    public int playerHealth;

    public float[] playerPosition;

    //enemy

    public int enemyLevel;
    public int enemyHealth;

    public float[] enemyPosition;

    //singleton

    public static GameManager2 instance;

    // prefab
    public PrefabManager prefabManager;

    public GameObject ShieldPrefab;
    public GameObject PlayerPrefab;
    public GameObject WolfPrefab;
    public GameObject FarmerPrefab;
    // depend on cutscene to active trigger again

    public List<GamerRecord> gamerRecords = new List<GamerRecord>();


    // gamer record

    public class GamerRecord
    {
        public string gamerName;
        public string recordTime;
        public string recordDate;

        public GamerRecord(string name, string time, string date)
        {
            this.gamerName = name;
            this.recordTime = time;
            this.recordDate = date; 
        }
    }

    public string ScoreTimeToString()
    {
        int minutes = Mathf.FloorToInt(scoreTime / 60);
        int seconds = Mathf.FloorToInt(scoreTime % 60);
        string formattedTime = string.Format("{0:00}:{1:00}", minutes, seconds);

        return formattedTime;
    }

    public bool IsNewRecord(string newRecordTime)
    {
        int newSeconds = TimeToSeconds(newRecordTime);

        foreach (GamerRecord record in gamerRecords)
        {
            if (record.recordTime == "") return true;
            int recordSeconds = TimeToSeconds(record.recordTime);

            if (newSeconds < recordSeconds)
            {
                return true; 
            }
        }

        return false;
    }

    public void SaveGamerRecord(string name)
    {
        DateTime currentDate = DateTime.Now;
        string formattedDate = currentDate.ToString("MM-dd-yyyy");

        int minutes = Mathf.FloorToInt(scoreTime / 60);
        int seconds = Mathf.FloorToInt(scoreTime % 60);
        string formattedTime = string.Format("{0:00}:{1:00}", minutes, seconds);

        GamerRecord gamerRecord = new GamerRecord(name, formattedTime, formattedDate);

        // add
        gamerRecords.Add(gamerRecord);

        // sort

        gamerRecords.Sort((a, b) =>
        {
            if (a.recordTime == "" && b.recordTime != "")
                return 1;
            if (a.recordTime != "" && b.recordTime == "")
                return -1;
            if (a.recordTime == "" && b.recordTime == "")
                return 1;
            return TimeToSeconds(a.recordTime).CompareTo(TimeToSeconds(b.recordTime));
        });

        // delete last
        gamerRecords.RemoveAt(gamerRecords.Count - 1);

        SaveSystem.SaveGame(this);
    }
    int TimeToSeconds(string time)
    {
        string[] parts = time.Split(':');
        if (parts.Length != 2)
        {
            Debug.LogError("Invalid time format: " + time);
            return 0;
        }

        int minutes = int.Parse(parts[0]);
        int seconds = int.Parse(parts[1]);

        return minutes * 60 + seconds;
    }

    private void Awake()
    {
        switch (sceneIndex)
        {
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                PrefabManager prefabManager = GameObject.Find("PrefabManager").GetComponent<PrefabManager>();
                PlayerPrefab = prefabManager.PlayerPrefab;
                FarmerPrefab = prefabManager.FarmerPrefab;

                currentPlayer = prefabManager.currentPlayer;
                currentEnemy = prefabManager.currentEnemy;

                break;
        }
        if (instance == null)
        {

            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);

    }

    private void Start()
    {
        //SaveGame();
        gamerRecords = new List<GamerRecord>()
        {
            new GamerRecord("", "", ""),
            new GamerRecord("", "", ""),
            new GamerRecord("", "", "")
        };
    }

    bool setup = true;
    private void Update()
    {
        if (!setup)
        {
            switch (sceneIndex)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    

                    if (GameObject.Find("PrefabManager")) // current build index scene != 0
                    {
                        prefabManager = GameObject.Find("PrefabManager").GetComponent<PrefabManager>();
                        PlayerPrefab = prefabManager.PlayerPrefab;
                        FarmerPrefab = prefabManager.FarmerPrefab;

                        currentPlayer = prefabManager.currentPlayer;
                        currentEnemy = prefabManager.currentEnemy;
                        setup = true;

                        SpawnPrefab(prefabManager.EggInNestPrefab, 169f, 190f, -471f, -453f, 4, 8);
                        SpawnPrefab(prefabManager.BonePrefab, 150f, 200f, -480, -440, 2, 4);
                    }
                    break;
            }
        }
        //if (currentPlayer == null)
        //{
        //    currentPlayer = GameObject.Find("Mick3 Player");
        //    if (currentPlayer == null) 
        //    {
        //        currentPlayer = GameObject.Find("Mick3 Player(Clone)");
        //    }
        //}
    }

    public void LoadGame()
    {
        GameData data = SaveSystem.LoadGame();

        //GameObject.Find("Loading Scene").GetComponent<LoadingScene>().LoadScene(data.sceneIndex);
        this.scoreTime = data.scoreTime;

        this.sceneIndex = data.sceneIndex;

        this.gamerRecords = data.GameDataGamerRecord();

        setup = false;
    }

    IEnumerator WaitAfterUpdateSceneIndex()
    {
        GameData data = SaveSystem.LoadGame();

        GameObject.Find("Loading Scene").GetComponent<LoadingScene>().LoadScene(data.sceneIndex);

        yield return new WaitForSeconds(3f);

        this.sceneIndex = data.sceneIndex;
        setup = false;

    }

    public void SaveGame()
    {
        SaveSystem.SaveGame(this);
        Debug.Log("Save Level Index: " + sceneIndex);
    }

    public void SpawnNewPlayer(Vector3 SpawnPostion)
    {

    }

    public void SpawnNewFarmer(Vector3 SpawnPostion)
    {

    }

    public void Respawn()
    {
        switch (sceneIndex)
        {
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:

                // spawn eggs
                GameObject EggPrefab = prefabManager.EggInNestPrefab;
                GameObject BonePrefab = prefabManager.BonePrefab;

                SpawnPrefab(EggPrefab, 169f, 190f, -471f, -453f, 4, 8);
                SpawnPrefab(BonePrefab, 150f, 200f, -480, -440, 2, 4);

                // spawn bones

                StartCoroutine(SpawnDelay());
                break;
        }

    }

    IEnumerator SpawnDelay()
    {
        Debug.Log("Spawn Level 4");

        currentPlayer = GameObject.Find("Mick3 Player");
        currentPlayer.GetComponent<Inventory>().ClearInventory();

        yield return new WaitForSeconds(2f);
        currentPlayer.transform.position = new Vector3(180f, 13f, -450f);


        yield return new WaitForSeconds(6.8f);

        currentPlayer.GetComponent<PlayerMovement3>().canMove = true;
    }

    public void SpawnPrefab(GameObject prefab, float x1, float x2, float z1, float z2, int min, int max)
    {
        int prefabCount = UnityEngine.Random.Range(min, max);
        GameObject prefabContainer = new GameObject("Prefab container");

        for (int i = 0; i < prefabCount; i++)
        {
            float randomX = UnityEngine.Random.Range(x1, x2);
            float randomZ = UnityEngine.Random.Range(z1, z2);

            Vector3 spawnPosition = new Vector3(randomX, 20f, randomZ);

            GameObject spawnedPrefab = Instantiate(prefab, spawnPosition, prefab.transform.rotation);

            spawnedPrefab.transform.parent = prefabContainer.transform;
        }
    }
}
