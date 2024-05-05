using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CutSceneController2 : MonoBehaviour
{
    [SerializeField] GameObject cutScene_1;
    [SerializeField] GameObject cutScene_2;


    private bool skipped = false;

    void Start()
    {
        cutScene_2.SetActive(false);
        cutScene_1.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !skipped && !GameObject.Find("Pause Screen") && !cutScene_2.activeSelf)
        {
            skipped = true;
            cutScene_1.GetComponent<CutScene1>().Skip();
        }
    }

}
