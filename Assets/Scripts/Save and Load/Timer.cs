using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    TextMeshProUGUI m_TextMeshProUGUI;
    public float elapsedTime;
    public bool Run;
    public GameObject player;

    private void Awake()
    {
        LoadTimer();
    }

    private void Start()
    {
        m_TextMeshProUGUI = GetComponent<TextMeshProUGUI>();
        //Run = true;
        if (player == null)
        {
            player = GameObject.Find("Mick3 Player");
        }
    }
    void Update()
    {
        if (Run)
        {
            elapsedTime += Time.deltaTime;

            int minutes = Mathf.FloorToInt(elapsedTime / 60);
            int seconds = Mathf.FloorToInt(elapsedTime % 60);
            m_TextMeshProUGUI.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        if (player == null)
        {
            player = GameObject.Find("Mick3 Player");
            if (player == null)
            {
                player = GameObject.Find("Mick3 Player(Clone)");
            }
            Run = false;
        }
        else
        {
            if (player.GetComponent<ThirdPersonMovement>()) {
                if (player.GetComponent<ThirdPersonMovement>().enabled)
                {
                    Run = true;
                } else { Run = false; }
            } else if (player.GetComponent<PlayerMovement>())
            {
                if (player.GetComponent<PlayerMovement>().enabled)
                {
                    Run = true;
                } else { Run = false; }
            } else if (player.GetComponent<PlayerMovement2>())
            {
                if (player.GetComponent<PlayerMovement2>().enabled)
                {
                    Run = true;
                }
                else { Run = false; }
            } else if (player.GetComponent<PlayerMovement3>())
            {
                if (player.GetComponent<PlayerMovement3>().canMove)
                {
                    Run = true;
                }
                else { Run = false; }
            }
        }
    }
    
    public void LoadTimer()
    {
        m_TextMeshProUGUI = GetComponent<TextMeshProUGUI>();

        // comment this for test single level / remember to enable it when you play

        // {
        elapsedTime = GameObject.Find("Game Manager 2").GetComponent<GameManager2>().scoreTime;
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        m_TextMeshProUGUI.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        // }
    }

    public void SaveTimer()
    {
        // comment this for test single level / remember to enable it when you play

        // {
        GameObject.Find("Game Manager 2").GetComponent<GameManager2>().scoreTime = elapsedTime;
        // }
    }
}
