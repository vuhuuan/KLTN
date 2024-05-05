using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CutSceneController : MonoBehaviour
{
    [SerializeField] private GameObject sceneCam;

    [SerializeField] private GameObject mainCam;
    [SerializeField] private GameObject miniMap;

    [SerializeField] private GameObject Andy;
    [SerializeField] private GameObject Necklace;

    [SerializeField] private Image PopUpQuest1;

    [SerializeField] private GameObject CutScene3;

    [SerializeField] private GameObject CutScene2;
    public TextMeshProUGUI QuestNoteText;





    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("StartGameScene");
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator StartGameScene()
    {
        //PopUpQuest1.gameObject.SetActive(false);
        if (QuestNoteText)
        {
            QuestNoteText.text = "Give the Item to Andy";
        }
        GameObject.Find("Mick3 Player").GetComponent<ThirdPersonMovement>().enabled = false;
        mainCam.SetActive(false);

        CutScene3.gameObject.SetActive(false);
        CutScene2.gameObject.SetActive(false);

        //miniMap.SetActive(false);
        //sceneCam.SetActive(true);
        Necklace.GetComponent<MeshRenderer>().enabled = false;

        Necklace.transform.Find("Item_indicator").gameObject.SetActive(false);



        yield return new WaitForSeconds(6.2f);

        Necklace.GetComponent<MeshRenderer>().enabled = true;

        yield return new WaitForSeconds(4.0f);

        Necklace.transform.Find("Item_indicator").gameObject.SetActive(true);

        yield return new WaitForSeconds(5f);


        //sceneCam.SetActive(false);
        mainCam.SetActive(true);
        //miniMap.SetActive(true);
        Necklace.transform.position = new Vector3 (-318.375f, 70f, -32.868f);

        //PopUpQuest1.gameObject.SetActive(true);
        GameObject.Find("Mick3 Player").GetComponent<ThirdPersonMovement>().enabled = true;
        GameObject.Find("CutScene Camera").SetActive(false);
    }
}
