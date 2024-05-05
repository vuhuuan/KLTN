using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class WolfBoss : Enemy
{
    public Timer timer;

    public GameObject WinCutScene;

    public GameObject StoryTelling;

    public GameObject CreditRollScene;

    public TextMeshProUGUI CreditRoll;

    public GameObject NewRecordUI;

    public GameObject MainCamera;

    public GameObject CutsceneCamera;

    public GameObject CutsceneCamera2;



    public bool checkStoryTelling = false;

    public string winRecordTime;


    public bool isEnd = false;
    protected override void Die()
    {
        MainCamera.GetComponent<AudioSource>().enabled = false;
        
        base.Die();
        // to make player die = false;
        if (!isEnd)
        {
            isEnd = true;
            GameObject.Find("Mick3 Player").GetComponent<Player>().ResetPlayer();
            GameObject.Find("Mick3 Player").GetComponent<PlayerMovement3>().canMove = false;

            //GameObject.Find("Mick3 Player").GetComponent<Player>().nearestSpawn = WinCutScene;


            GameObject.Find("Mick3 Player").GetComponent<PlayerMovement3>().enabled = false;

            if (GameObject.Find("UI"))
            {
                GameObject.Find("UI").SetActive(false);
            }

            // wolf idle
            gameObject.GetComponent<WolfAI>().SwichState(WolfAI.State.Default);

            // save game

            // save score

        

            if (timer)
            {
                Debug.Log("You won!");
                timer.SaveTimer();
                winRecordTime = GameObject.Find("Game Manager 2").GetComponent<GameManager2>().ScoreTimeToString();
                CreditRoll.text += winRecordTime;

                // load scene and thanks for play
                StartCoroutine(LoadWinScene());

                // story telling

                // slide share vid + thanks you for playing 
            }
        }
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("Wolf Get Hit");
    }

    private void Update()
    {
        if (checkStoryTelling)
        {
            if (!StoryTelling.activeSelf) // after close storytelling, run the credit roll
            {
                CreditRollScene.GetComponent<PlayableDirector>().Play();
                GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("Ending Music");

                checkStoryTelling = false;

                StartCoroutine(EnableInpuGamertName((float) CreditRollScene.GetComponent<PlayableDirector>().duration));
            }
        }
    }
    IEnumerator LoadWinScene()
    {
        GameObject.Find("Overlay2").GetComponent<Animation>().Play("overlay-end2");
        yield return new WaitForSeconds(2f);
        MainCamera.SetActive(false);
        WinCutScene.GetComponent<PlayableDirector>().Play();

        StartCoroutine(WaitForCutSceneEnd((float) WinCutScene.GetComponent<PlayableDirector>().duration));
    }

    IEnumerator WaitForCutSceneEnd(float SceneDuration)
    {
        yield return new WaitForSeconds(SceneDuration);
        GameObject.Find("Overlay2").GetComponent<Animation>().Play("overlay-start");
        StoryTelling.SetActive(true);
        checkStoryTelling = true;
        
        MainCamera.SetActive(true);
        CutsceneCamera.SetActive(false);
        CutsceneCamera2.SetActive(false);

    }

    IEnumerator EnableInpuGamertName(float creditRollDuration)
    {
        yield return new WaitForSeconds(creditRollDuration - 1f);

        if (GameObject.Find("Game Manager 2").GetComponent<GameManager2>().IsNewRecord(winRecordTime))
        {
            NewRecordUI.SetActive(true);
        }
        else
        {
            // move to the start game menu scene.
            yield return new WaitForSeconds(2f);
            //Debug.Log("Should move to Menu scene now from wolf Booss");
            GameObject.Find("Loading Scene").GetComponent<LoadingScene>().LoadScene(0);
        }
    }

    public void AttackSound()
    {
        GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("Wolf Attack");
    }
}
