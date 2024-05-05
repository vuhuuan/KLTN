using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public GameManager2 gameManager;
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); 
    }
    
    public void QuitGame()
    {
        Debug.Log("Quit game activated");
        Application.Quit();
    }

    public void ContinueGame()
    {
        gameManager.LoadGame();
        GameObject.Find("Loading Scene").GetComponent<LoadingScene>().LoadScene(gameManager.sceneIndex);
    }
}
