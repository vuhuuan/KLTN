using System.Collections;
//using System.Collections.Generic;
//using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.SceneManagement;



public class TriggerLevel4 : MonoBehaviour
{
    // Start is called before the first frame update
    public QuestDisplay quest;
    [SerializeField] private CutSceneController3 cutSceneController;
    [SerializeField] GameObject winCutScene;
    bool isWin = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (quest.currentQuestIndex == 1)
            {
                if (!isWin)
                {
                    cutSceneController.PlayCutScene(winCutScene);
                    StartCoroutine(WaitBeforeCutScene());
                    isWin = true;
                    //GameObject.Find("Quest").GetComponent<QuestDisplay>().FinishCurrentQuest();
                }

                Debug.Log("Next Level");
            }
        }
    }
    
    IEnumerator WaitBeforeCutScene()
    {
        yield return new WaitForSeconds(15f);
        GameObject.Find("Loading Scene").GetComponent<LoadingScene>().LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
