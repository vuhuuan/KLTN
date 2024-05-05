using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPCInteractable : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] string interactText;
    [SerializeField] PlayerInteractUI playerInteractUI;
    [SerializeField] PlayerMovement3 playerMovement;

    [SerializeField] GameObject normalCamera;
    [SerializeField] GameObject topDownCamera;

    public string[] scriptList;

    protected bool isTalking = false;

    [SerializeField] TextMeshProUGUI talkingTextUI;

    public float timeBetweenScripts = 1f;

    private int currentScriptIndex = 0;
    protected bool canPressSpace = true;


    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        CheckInteractInput();
    }

    protected virtual void CheckInteractInput()
    {
        if (isTalking && canPressSpace && Input.GetKeyDown(KeyCode.Space))
        {
            //Debug.Log("I was clicked");
            GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("Interact");
            StartCoroutine(NextScriptWithDelay());
        }
        else if (isTalking && Input.GetKeyDown(KeyCode.Escape))
        {
            FinishInteract();
        }
    }

    public virtual void Interact()
    {
        Debug.Log(gameObject.name);
        playerInteractUI.isTalking = true;
        isTalking = true;

        //player stop when interact
        playerMovement.enabled = false;

        //change camera when interact
        normalCamera.SetActive(false);
        topDownCamera.SetActive(true);

        // open talking text ui
        talkingTextUI.text = scriptList[0];
        talkingTextUI.gameObject.SetActive(true);
    }

    public string GetInteractText()
    {
        return interactText;
    }

    public virtual void FinishInteract ()
    {
        playerInteractUI.isTalking = false;
        isTalking = false;

        //player active after interact
        playerMovement.enabled = true;

        //change camera after interact
        normalCamera.SetActive(true);
        topDownCamera.SetActive(false);

        // reset talking script
        currentScriptIndex = 0;
        canPressSpace = false;
        talkingTextUI.gameObject.SetActive(false);
    }

    IEnumerator NextScriptWithDelay()
    {
        canPressSpace = false;
        currentScriptIndex += 1;

        if (currentScriptIndex < scriptList.Length)
        {
            talkingTextUI.text = scriptList[currentScriptIndex];
            talkingTextUI.gameObject.GetComponent<Animation>().Play();
        }
        else
        {
            FinishInteract();
        }

        yield return new WaitForSeconds(timeBetweenScripts);

        canPressSpace = true;
    }

}
