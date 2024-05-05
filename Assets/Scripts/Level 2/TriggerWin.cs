using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TriggerWin : MonoBehaviour
{
    [SerializeField] private GameObject cutSceneWin;

    [SerializeField] private Transform guideArrow;

    [SerializeField] private Camera mainCamera;

    [SerializeField] private GameObject sceneCamera;


    [SerializeField] private TMP_Text notifyText;

    [SerializeField] private Image questStatus;
    [SerializeField] private Sprite questStatusSprite;

    private bool canRunAnimation = true;
    private float animationCooldown = 1.0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.transform.Find("LocketNecklace"))
            {
                other.transform.Find("LocketNecklace").gameObject.SetActive(false);
                other.GetComponent<PlayerMovement>().enabled = false;
                sceneCamera.GetComponent<AudioListener>().enabled = true;
                mainCamera.gameObject.SetActive(false);
                Debug.Log("You made it, boy");
                guideArrow.gameObject.SetActive(true);
                cutSceneWin.SetActive(true);
                StartCoroutine(WaitTillNextScene());

                questStatus.sprite = questStatusSprite;
                GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("Mission Completed");
            }
            else
            {
                if (canRunAnimation)
                {
                    notifyText.GetComponent<Animation>().Play("Float text");
                    canRunAnimation = false;
                    StartCoroutine(CooldownTimer());
                }
            }
        } else
        {
            
        }
    }

    IEnumerator WaitTillNextScene()
    {
        if (GameObject.Find("Motobiker"))
        {
            GameObject.Find("Motobiker").SetActive(false);
        }

        yield return new WaitForSeconds(17f);
        GameObject.Find("Loading Scene").GetComponent<LoadingScene>().LoadScene(3);
    }
    private IEnumerator CooldownTimer()
    {
        yield return new WaitForSeconds(animationCooldown);

        canRunAnimation = true;
    }
}
