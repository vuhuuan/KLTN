using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;

public class CutSceneController4 : MonoBehaviour
{

    [SerializeField] GameObject[] gameObjects;
    [SerializeField] PlayerMovement3 playerMovement;
    [SerializeField] NavToDes enemyMovement;


    PlayableDirector timeline;

    private void Start()
    {
        timeline = gameObject.GetComponent<PlayableDirector>() ;
    }

    private void Update()
    {

    }

    public void TurnOffSomething()
    {
        playerMovement.enabled = false;
        if (enemyMovement)
        {
            enemyMovement.enabled = false;
        }

        foreach (var gameObject in gameObjects)
        {
            gameObject.SetActive(false);
        }
    }

    public void PlayTheScene()
    {
        TurnOffSomething();
        timeline.Play();
        StartCoroutine(WaitForCutScene());
    }

    public void TurnOnAgain()
    {
        playerMovement.enabled = true;
        if (enemyMovement)
        {
            enemyMovement.enabled = true;
        }


        foreach (var gameObject in gameObjects)
        {
            gameObject.SetActive(true);
        }
    }

    IEnumerator WaitForCutScene()
    {
        yield return new WaitForSeconds((float) timeline.duration);
        TurnOnAgain();
    }
}
