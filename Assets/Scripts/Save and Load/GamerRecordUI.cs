using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GamerRecordUI : MonoBehaviour
{
    public TextMeshProUGUI No_1_name;
    public TextMeshProUGUI No_1_record;
    public TextMeshProUGUI No_1_date;

    public TextMeshProUGUI No_2_name;
    public TextMeshProUGUI No_2_record;
    public TextMeshProUGUI No_2_date;

    public TextMeshProUGUI No_3_name;
    public TextMeshProUGUI No_3_record;
    public TextMeshProUGUI No_3_date;


    public GameManager2 gameManager;


    private void Awake()
    {
        gameManager = GameObject.Find("Game Manager 2").GetComponent<GameManager2>();
    }


    public void UpdateRecord()
    {
        if (gameManager == null)
        {
            gameManager = GameObject.Find("Game Manager 2").GetComponent<GameManager2>();
        }
        gameManager.LoadGame();

        No_1_name.text = gameManager.gamerRecords[0].gamerName;
        No_1_record.text = gameManager.gamerRecords[0].recordTime;
        No_1_date.text = gameManager.gamerRecords[0].recordDate;

        No_2_name.text = gameManager.gamerRecords[1].gamerName;
        No_2_record.text = gameManager.gamerRecords[1].recordTime;
        No_2_date.text = gameManager.gamerRecords[1].recordDate;

        No_3_name.text = gameManager.gamerRecords[2].gamerName;
        No_3_record.text = gameManager.gamerRecords[2].recordTime;
        No_3_date.text = gameManager.gamerRecords[2].recordDate;
    }
}
