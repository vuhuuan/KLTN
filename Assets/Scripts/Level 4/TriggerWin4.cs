using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerWin4 : MonoBehaviour
{
    // Start is called before the first frame update
    public QuestDisplay questDisplay;
    public GameObject storyTelling;
    public GameObject overlay2;

    public LoadingScene loadingScene;

    public PlayerMovement3 playerMovement;

    bool nextScene;

    void Start()
    {
        nextScene = false;
    }


    // Update is called once per frame
    void Update()
    {
       CheckEggToUpdateQuest();

       if (storyTelling != null)
        {
            if (storyTelling.GetComponent<StoryTelling>().tellingIsDone && !nextScene) 
            {
                nextScene = true;
                loadingScene.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }

    bool triggerWin = true;
    public void CheckEggToUpdateQuest()
    {
        if (questDisplay)
        {
            Quest quest = questDisplay.questList[questDisplay.currentQuestIndex];
            if (quest.name == "Steal Egg")
            {
                if (quest.isFinished && triggerWin)
                {
                    playerMovement.enabled = false;
                    triggerWin = false;
                    Debug.Log("You won this level!");

                    // win here
                    Animation anim = overlay2.GetComponent<Animation>();

                    //anim["overlay-end"].speed = 0.2f;
                    anim.Play("overlay-end");
                    StartCoroutine(WaitForNextScene());
                }
            }
        }
    }

    IEnumerator WaitForNextScene()
    {
        yield return new WaitForSeconds(2f);
        storyTelling.SetActive(true);
    }
}
