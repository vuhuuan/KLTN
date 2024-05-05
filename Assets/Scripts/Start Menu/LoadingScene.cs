using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    [SerializeField] private GameObject LoadingScreen;
    [SerializeField] private Slider loadingBar;

    public Timer timer;

    public void LoadScene(int sceneId) 
    {
        if (sceneId != 0)
        {
            if (timer)
            {
                timer.SaveTimer();
            }
            if (timer)
            {
                timer.SaveTimer();
            }
            GameObject.Find("Game Manager 2").GetComponent<GameManager2>().sceneIndex = sceneId;
            GameObject.Find("Game Manager 2").GetComponent<GameManager2>().SaveGame();
        }
        StartCoroutine(LoadSceneAsync(sceneId));
    }

    IEnumerator LoadSceneAsync (int sceneId)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);

        LoadingScreen.SetActive(true);

        while(!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);

            loadingBar.value = progressValue;

            yield return null;
        }
    }
}
