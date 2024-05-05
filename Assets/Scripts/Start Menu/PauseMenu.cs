using System.Collections;
using System.Collections.Generic;
//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // Update is called once per frame
    bool isPaused = false;
    [SerializeField] private GameObject PauseScreen;

    [SerializeField] private Timer timer;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && !isPaused)
        {
            GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("Button Click");
            Pause();
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("Button Click");
            Continue();
        }

    }

    public void Pause()
    {
        isPaused = true;
        PauseScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Continue()
    {
        isPaused = false;
        PauseScreen.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Exit()
    {
        Debug.Log("Return to Menu");
        Time.timeScale = 1f;

        //// save game after exit
        //GameObject.Find("Game Manager 2").GetComponent<GameManager2>().sceneIndex = SceneManager.GetActiveScene().buildIndex;

        //// this is true but for now we need other.
        ////timer.SaveTimer();
        //GameObject.Find("Game Manager 2").GetComponent<GameManager2>().SaveGame();


        GameObject.Find("Loading Scene").GetComponent<LoadingScene>().LoadScene(0);
    }
}
