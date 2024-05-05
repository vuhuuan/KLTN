using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;


public class CutScene1 : MonoBehaviour
{

    [SerializeField] private Camera mainCam;
    //[SerializeField] private Camera thirdCam;
    [SerializeField] private Camera sceneCam;

    [SerializeField] private Image quest_1;

    [SerializeField] private Transform motobiker;

    [SerializeField] private Transform guideArrow;

    [SerializeField] private Image overlay;

    [SerializeField] private PlayerMovement playerMovement;


    void Start()
    {
        guideArrow.gameObject.SetActive(false);

        mainCam.gameObject.SetActive(false);
        GameObject.Find("Crate").GetComponent<ShakeToBreak>().enabled = false;

        StartCoroutine("WaitCutSceneEnd");
        playerMovement.enabled = false;
        //thirdCam.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Skip()
    {
        playerMovement.enabled = true;

        guideArrow.gameObject.SetActive(true);
        overlay.gameObject.SetActive(false);
        motobiker.position = new Vector3(motobiker.position.x, motobiker.position.y, 290.29f);
        StopCoroutine("WaitCutSceneEnd");
        //quest_1.gameObject.SetActive(false);

        mainCam.gameObject.SetActive(true);
        sceneCam.GetComponent<AudioListener>().enabled = false;
        GameObject.Find("Crate").GetComponent<ShakeToBreak>().enabled = true;

        gameObject.SetActive(false);
    }
    IEnumerator WaitCutSceneEnd()
    {
        yield return new WaitForSeconds(7.2f);

        //quest_1.gameObject.SetActive(true);
        
        guideArrow.gameObject.SetActive(true);

        yield return new WaitForSeconds(1.7f);
        mainCam.gameObject.SetActive(true);

        GameObject.Find("Crate").GetComponent<ShakeToBreak>().enabled = true;
        playerMovement.enabled = true;
          
    }
}
