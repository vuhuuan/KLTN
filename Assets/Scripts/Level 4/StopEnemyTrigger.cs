using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopEnemyTrigger : MonoBehaviour
{
    public NavToDes enemyFollowPlayerScript;

    public EnemyBackHome enemyBackHomeScript;

    public GameObject chickenCoop;

    public float chaseRange = 30f;


    private void Update()
    {
        if (!CloseToTheChickenCoop())
        {
            enemyFollowPlayerScript.enabled = false;
            enemyBackHomeScript.enabled = true;
        } else
        {
            enemyFollowPlayerScript.enabled = true;
            enemyBackHomeScript.enabled = false;
        }
    }

    bool CloseToTheChickenCoop()
    {
        return (transform.position - chickenCoop.transform.position).magnitude < chaseRange;
    }
}
