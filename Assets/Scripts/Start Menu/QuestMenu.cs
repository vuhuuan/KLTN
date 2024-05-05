using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestMenu : MonoBehaviour
{
    // Update is called once per frame
    bool isPaused = false;
    [SerializeField] private GameObject QuestScreen;
    [SerializeField] private GameObject StoryTelling;

    private void Awake()
    {
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !isPaused && !StoryTelling.activeSelf)
        {
            Pause();
        } else if (Input.GetKeyDown(KeyCode.Q) && !StoryTelling.activeSelf)
        {
            Continue();
        }

        
    
    }

    public void Pause()
    {
        isPaused = true;
        QuestScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Continue()
    {
        isPaused = false;
        QuestScreen.SetActive(false);
        Time.timeScale = 1f;
    }
}
