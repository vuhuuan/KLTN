using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject playerPrefab;
    public GameObject itemPrefab;



    [SerializeField] HealthBarController HealthBar;
    [SerializeField] FoodBarController FoodBar;

    public GameObject nearestSpawn;
    [SerializeField] CutSceneController3 cutSceneController;

    [SerializeField] GameObject player_cs;

    [SerializeField] Animation damageOverlay;

    [SerializeField] CinemachineFreeLook freeLookCamera;
    public Vector3 nearestSpawnPos;

    public StoryTelling storyTelling;

    public GameObject startGameScene;

    public AudioSource cameraSource;

    public ThirdPersonMovement playerMovement;




    void Start()
    {
        storyTelling.gameObject.SetActive(true);
        nearestSpawnPos = new Vector3 (-318.44f, 69.922f, -31.93f);
        cameraSource.enabled = false;
        playerMovement.enabled = false;
    }

    bool startGame = false;
    // Update is called once per frame
    void Update()
    {
        if (storyTelling.tellingIsDone && !startGame)
        {
            startGame = true;
            startGameScene.SetActive(true);
            cameraSource.enabled = true;
        }
    }
    public void TestSpawnPlayer(GameObject player)
    {
        Debug.Log("destroy and spawn");
        GameObject newPlayer = Instantiate(playerPrefab, nearestSpawnPos, Quaternion.identity);

        if (player.transform.Find("Locket necklace") || player.transform.Find("Locket necklace(Clone)")) 
        {
            Instantiate(itemPrefab, nearestSpawnPos + new Vector3(0, 0, -1), Quaternion.identity);
        }

        TestBackUpPlayerInform1(newPlayer);
        Destroy(player);

    }

    public void TestBackUpPlayerInform1(GameObject player)
    {
        player.GetComponent<Player>().HealthBar = this.HealthBar;
        player.GetComponent<Player>().damageOverlay = this.damageOverlay;
        player.GetComponent<Player>().nearestSpawn = this.nearestSpawn;

        freeLookCamera.Follow = player.transform;
        freeLookCamera.LookAt = player.transform;

        Transform mick3CamView = player.transform.Find("Mick3 Cam View");

        player.transform.Find("Mick3 Cam View");
        if (mick3CamView != null)
        {
            freeLookCamera.Follow = mick3CamView; 
            freeLookCamera.LookAt = mick3CamView; 
        }
        else
        {
            Debug.LogError("Mick3 Cam View not found!");
        }

    }
}
