using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class ChickenCoopTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject CutScene1;
    [SerializeField] ArrowIndicatorController arrowIndicator;
    [SerializeField] StopEnemyTrigger stopEnemyScript;
    [SerializeField] GameObject enemy;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Some one is in the coop range!");
            CutScene1.GetComponent<CutSceneController4>().PlayTheScene();

            float cutSceneDuration = (float) CutScene1.GetComponent<PlayableDirector>().duration;

            other.GetComponent<PlayerMovement3>().canMove = false;
            StartCoroutine(UnlockMove(cutSceneDuration, other));


            arrowIndicator.Hide();
            stopEnemyScript.enabled = true;

            other.GetComponent<Player>().nearestSpawn = CutScene1;

            Destroy(enemy.GetComponent<NPCInteractable>());
            GetComponent<BoxCollider>().enabled = false;

        }
    }

    IEnumerator UnlockMove(float delayTime, Collider player)
    {
        yield return new WaitForSeconds(delayTime);
        player.GetComponent<PlayerMovement3>().canMove = true;

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
