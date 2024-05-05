using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static GameManager2;

[System.Serializable]
public class GameData
{
    //scene
    public int sceneIndex;
    public int cutSceneIndex;

    //score
    public float scoreTime;


    public string GamerRecordNo1_name;
    public string GamerRecordNo2_name;
    public string GamerRecordNo3_name;

    public string GamerRecordNo1_recordTime;
    public string GamerRecordNo2_recordTime;
    public string GamerRecordNo3_recordTime;

    public string GamerRecordNo1_recordDate;
    public string GamerRecordNo2_recordDate;
    public string GamerRecordNo3_recordDate;


    //player
    public int playerLevel;
    public int playerHealth;

    public float[] playerPosition;

    //enemy

    public int enemyLevel;
    public int enemyHealth;

    public float[]  enemyPosition;

 
    public GameData(GameManager2 gameManager)
    {
        this.sceneIndex = gameManager.sceneIndex;
        this.scoreTime = gameManager.scoreTime;

        List<GamerRecord> gamerRecords = gameManager.gamerRecords;

        this.GamerRecordNo1_name = gamerRecords[0].gamerName;
        this.GamerRecordNo2_name = gamerRecords[1].gamerName;
        this.GamerRecordNo3_name = gamerRecords[2].gamerName;

        this.GamerRecordNo1_recordTime = gamerRecords[0].recordTime;
        this.GamerRecordNo2_recordTime = gamerRecords[1].recordTime;
        this.GamerRecordNo3_recordTime = gamerRecords[2].recordTime;

        this.GamerRecordNo1_recordDate = gamerRecords[0].recordDate;
        this.GamerRecordNo2_recordDate = gamerRecords[1].recordDate;
        this.GamerRecordNo3_recordDate = gamerRecords[2].recordDate;

    }

    public List<GamerRecord> GameDataGamerRecord()
    {
        List<GamerRecord> records = new List<GamerRecord>
        {
            new GamerRecord(GamerRecordNo1_name, GamerRecordNo1_recordTime, GamerRecordNo1_recordDate),
            new GamerRecord(GamerRecordNo2_name, GamerRecordNo2_recordTime, GamerRecordNo2_recordDate),
            new GamerRecord(GamerRecordNo3_name, GamerRecordNo3_recordTime, GamerRecordNo3_recordDate)
        };

        return records;
    }

}
