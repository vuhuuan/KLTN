using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputUI : MonoBehaviour
{
    TMP_InputField inputField;
    Button subtmitButton;
    GameManager2 gameManager;

    private void Awake()
    {
        inputField = transform.Find("Input Name").GetComponent<TMP_InputField>();
        inputField.characterLimit = 10;
        gameManager = GameObject.Find("Game Manager 2").GetComponent<GameManager2>();
    }

    bool isSaved = false;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && ! isSaved)
        {
            SaveGamerName();
            isSaved = true;
        }
    }

    public void SaveGamerName()
    {
        string gamerName = "";
        if (inputField.text.Trim() != "")
        {
            gamerName = inputField.text.Trim();
            //Debug.Log(inputField.text.Trim());
        } else
        {
            gamerName = "Anonymous";

            Debug.Log("Anonymous");
        }

        gameManager.sceneIndex = 0;
        gameManager.SaveGamerRecord(gamerName);

        // return to start scene
        GameObject.Find("Loading Scene").GetComponent<LoadingScene>().LoadScene(0);
        Debug.Log("Should move to Menu scene now from new Record");

    }

}
