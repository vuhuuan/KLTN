using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
//using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine.Playables;

public class TriggerCutScene : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private GameObject CutScene2;
    [SerializeField] private GameObject CutScene3;

    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject TrafficManager;
    [SerializeField] private GameObject Cars;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject player2;

    [SerializeField] private GameObject timeline4;

    [SerializeField] private Image PopUpQuest1;
    [SerializeField] private Image PopUpQuest2;

    [SerializeField] private GameObject Andy;

    [SerializeField] private GameObject NextTrigger;

    [SerializeField] private TMP_Text notifyText;


    [SerializeField] private GameManager gameManager;

    public GameObject UI;

    bool isNewScene = false;


    private bool canRunAnimation = true;
    private float animationCooldown = 1.0f;

    public TextMeshProUGUI QuestNote;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            player = GameObject.Find("Mick3 Player(Clone)");
        }
        if (player2 == null)
        {
            player2 = GameObject.Find("Mick3 Player(Clone)");
        }

    }
    IEnumerator WaitBeforeCutScene()
    {
        yield return new WaitForSeconds(1.2f);
        UI.SetActive(false);

        mainCamera.SetActive(false);
        player2.SetActive(false);
        GameObject.Find("Overlay2").GetComponent<Animation>().Play("overlay-start");
        CutScene2.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.name == "TriggerCutScene2" && other.CompareTag("Player"))
        {
            if (other.transform.Find("Locket necklace") || other.transform.Find("Locket necklace(Clone)"))
            {
                other.GetComponent<ThirdPersonMovement>().enabled = false;

                mainCamera.GetComponent<AudioSource>().loop = false;
                other.GetComponent<Animator>().SetBool("running", false);
                other.GetComponent<Animator>().SetBool("walking", false);
                other.GetComponent<Animator>().SetBool("idle", true);

                //PopUpQuest1.gameObject.SetActive(false);
                //PopUpQuest2.gameObject.SetActive(false);
                if (QuestNote)
                {
                    QuestNote.text = "Back home... Mick";
                }


                GameObject.Find("Overlay2").GetComponent<Animation>().Play("overlay-end");
                StartCoroutine("WaitBeforeCutScene");



                TrafficManager.GetComponent<TrafficManager>().SetBeforeCutScene(false);
                StartCoroutine(backToGame());
                gameManager.nearestSpawnPos = new Vector3(-318.44f, 69.922f, 1.32f);
            }
            else
            {
                other.GetComponent<BoundaryOne>().setBoundZ(-42f, 0f);
                Debug.Log("Display forgot item text");  

                if (canRunAnimation)
                {
                    notifyText.GetComponent<Animation>().Play("Float text");
                    canRunAnimation = false;
                    StartCoroutine(CooldownTimer());
                }
            }
        } else if (gameObject.name == "TriggerCutScene3" && other.CompareTag("Player"))
        {
            if (other.transform.Find("Locket necklace") || other.transform.Find("Locket necklace(Clone)"))
            {
                UI.SetActive(false);
                other.GetComponent<ThirdPersonMovement>().enabled = false;
                other.GetComponent<Animator>().SetBool("running", false);
                other.GetComponent<Animator>().SetBool("walking", false);
                other.gameObject.SetActive(false);
                //PopUpQuest1.gameObject.SetActive(false);
                //PopUpQuest2.gameObject.SetActive(false);
                mainCamera.SetActive(false);
                CutScene3.SetActive(true);
                //TrafficManager.GetComponent<TrafficManager>().SetBeforeCutScene(false);
                //StartCoroutine(backToGame());
                Debug.Log("Change to new scene");

                if (!isNewScene)
                {
                    isNewScene = true;
                    StartCoroutine(newScene());
                }
                //Destroy(gameObject);
                gameObject.GetComponent<BoxCollider>().enabled = false;
            }
            else
            {
                Debug.Log("Display forgot item text");

                if (canRunAnimation)
                {
                    notifyText.GetComponent<Animation>().Play("Float text");
                    canRunAnimation = false;
                    StartCoroutine(CooldownTimer());
                }
            }
        }
    }

    IEnumerator backToGame()
    {
        yield return new WaitForSeconds(20f);
        UI.SetActive(true);
        mainCamera.SetActive(true);
        //player.GetComponent<BoundaryOne>().setBoundX();
        //player.GetComponent<BoundaryOne>().setBoundZ();

        NextTrigger.SetActive(true);
        Destroy(Cars);
        Destroy(Andy);
        Destroy(player);
        player2.SetActive(true);
        player2.GetComponent<ThirdPersonMovement>().enabled = true;


        player2.transform.position = new Vector3(-317.891f, 69.922f, 4.58f);

        Destroy(gameObject);

    }

    IEnumerator newScene()
    {

            isNewScene = true;
        yield return new WaitForSeconds(5.5f);
        //mainCamera.SetActive(true);
        GameObject.Find("Loading Scene").GetComponent<LoadingScene>().LoadScene(2);
        
    }

    private IEnumerator CooldownTimer()
    {
        yield return new WaitForSeconds(animationCooldown);

        canRunAnimation = true;
    }

}
