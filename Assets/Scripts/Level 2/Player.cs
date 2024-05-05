using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using System.Linq.Expressions;
//using static UnityEditor.Progress;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    private int maxHealth = 5;
    private int currentHealth;
    public Timer timer;

    public GameObject TakeDameEffect;

    public int Health
    {
        get { return currentHealth; }
        set { currentHealth = value; }
    }

    private Animator animator;

    [SerializeField] public HealthBarController HealthBar;
    [SerializeField] public FoodBarController FoodBar;

    public GameObject nearestSpawn;

    [SerializeField] public CutSceneController3 cutSceneController;

    [SerializeField] public GameObject player_cs;

    [SerializeField] public Animation damageOverlay;



    public bool isDead = false;
    private void Awake()
    {
        animator = GetComponent<Animator>();


    }

    void Start()
    {
        currentHealth = maxHealth;
        HealthBar.setHealth(currentHealth);

        timer = GameObject.Find("Timer text").GetComponent<Timer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            Die();
        }

        if (transform.position.y < - 20f)
        {
            Die();
        }


        if (FoodBar)
        {
            if (FoodBar.Hunger <= 0)
            {
                Die();
                //nearestSpawn = GameObject.Find("Cut scene 1.5");
            }
        }
    }

    public void Die()
    {
        if (!isDead)
        {
            if (timer)
            {
                timer.SaveTimer();
            }
            GameObject.Find("Overlay2").GetComponent<Animation>().Play("overlay-end");

            isDead = true;

            //if (gameObject.GetComponent<ThirdPersonMovement>())
            //{
            //    gameObject.GetComponent<ThirdPersonMovement>().enabled = false;
            //}
            //else if (gameObject.GetComponent<PlayerMovement>())
            //{
            //    gameObject.GetComponent<PlayerMovement>().enabled = false;

            //}
            //else if (gameObject.GetComponent<PlayerMovement2>())
            //{
            //    gameObject.GetComponent<PlayerMovement2>().enabled = false;

            //}
            //else if (gameObject.GetComponent<PlayerMovement3>())
            //{
            //    gameObject.GetComponent<PlayerMovement3>().canMove = false;
            //}

            BlockPlayerMovement();

            if (animator)
            {
                animator.SetBool("walking", false);
                animator.SetBool("running", false);

                animator.Play("Mick|Mick_Dead");
                animator.ResetTrigger("isDead");
                animator.SetTrigger("isDead");
            }
            GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("Lose");

            if (!nearestSpawn)
            {
                StartCoroutine(GameOverReload());
            } else
            {
                StartCoroutine(GameOverReloadCutScene());
            }
        }
    }

    public bool isProtected = false;

    public void BlockPlayerMovement()
    {
        if (gameObject.GetComponent<ThirdPersonMovement>())
        {
            gameObject.GetComponent<ThirdPersonMovement>().enabled = false;
        }
        else if (gameObject.GetComponent<PlayerMovement>())
        {
            gameObject.GetComponent<PlayerMovement>().enabled = false;

        }
        else if (gameObject.GetComponent<PlayerMovement2>())
        {
            gameObject.GetComponent<PlayerMovement2>().enabled = false;

        }
        else if (gameObject.GetComponent<PlayerMovement3>())
        {
            gameObject.GetComponent<PlayerMovement3>().canMove = false;
        }
    }

    public void UnBlockPlayerMovement()
    {
        if (gameObject.GetComponent<ThirdPersonMovement>())
        {
            gameObject.GetComponent<ThirdPersonMovement>().enabled = true;
        }
        else if (gameObject.GetComponent<PlayerMovement>())
        {
            gameObject.GetComponent<PlayerMovement>().enabled = true;

        }
        else if (gameObject.GetComponent<PlayerMovement2>())
        {
            gameObject.GetComponent<PlayerMovement2>().enabled = true;

        }
        else if (gameObject.GetComponent<PlayerMovement3>())
        {
            gameObject.GetComponent<PlayerMovement3>().canMove = true;
        }
    }


    public void TakeDamage(int dame)
    {
        if (isProtected)
        {
            isProtected = false;
            transform.Find("Pickup Range").GetComponent<PickUpDrop2>().Drop();
            return;
        }

        if (isDead)
        {
            return;
        }
        
        GetKnockBack();

        if (dame == 5)
        {
            //currentHealth <= 0
            timer.SaveTimer();
        }
        currentHealth -= dame;
        HealthBar.setHealth(currentHealth);
        damageOverlay.Play("damage overlay");
        if (currentHealth <= 0)
        {
            //isDead = true;
            return;
        }

        if (animator)
        {
            animator.ResetTrigger("hurt");
            animator.SetTrigger("hurt");
        }

        GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("Dog Get Hurt");
    }

    public void GetKnockBack()
    {
        if (TakeDameEffect)
        {
            TakeDameEffect.SetActive(true);
        }
        if (gameObject.activeSelf)
        {
            BlockPlayerMovement();

            if (currentHealth > 0)
            {
                StartCoroutine("CanMoveCoolDown");
            }
        }
    }
    
    IEnumerator CanMoveCoolDown()
    {
        float cooldown = 0.6f;
        yield return new WaitForSeconds(cooldown);
        if (currentHealth > 0)
        {
            UnBlockPlayerMovement();
        }
        if (TakeDameEffect)
        {
            TakeDameEffect.SetActive(false);
        }
    }

    public void ResetPlayer()
    {
        gameObject.GetComponent<PickUpDrop>();
        //transform.position = mickPos;
        //transform.rotation = player_cs.transform.rotation;
        //transform.localScale = player_cs.transform.localScale;
        animator.ResetTrigger("isDead");

        // stop other animation and transfer to idle
        animator.SetBool("idle", true);
        animator.SetBool("running", false);
        animator.SetBool("walking", false);
        animator.SetBool("sneaky", false);
        animator.ResetTrigger("attack");
        animator.ResetTrigger("roll");
        animator.Play("Mick|Mick_Idle");

        currentHealth = maxHealth;
        HealthBar.setHealth(maxHealth);
        if (FoodBar)
        {
            FoodBar.hunger = 80f;
        }
        isDead = false;


        //animator.Play("Mick|Mick_Idle");

    }

    IEnumerator GameOverReload()
    {
        Debug.Log("Reload after dead");
        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

    IEnumerator GameOverReloadCutScene()
    {
        int level = GameObject.Find("Game Manager 2").GetComponent<GameManager2>().sceneIndex;

        // BEFORE CUTSCENE
        BlockPlayerMovement();
        // for map 5
        if (level == 5)
        {
            GameObject enemy = GameObject.Find("PrefabManager").GetComponent<PrefabManager>().currentEnemy;

            enemy.GetComponent<WolfAI>().SwichState(WolfAI.State.Idle);

            enemy.GetComponent<WolfBoss>().enabled = false;
            enemy.GetComponent<WolfAI>().enabled = false;

            enemy.GetComponent<WolfBoss>().SetUpHealthBar();
        }

        yield return new WaitForSeconds(2.4f); // need time to deal with interupt animation.
        BlockPlayerMovement();
        // AFTER CUTSCENE
        // set the player back
        ResetPlayer();
        Debug.Log("Current level:" + level);

        GameObject.Find("Overlay2").GetComponent<Animation>().Play("overlay-start");

        if (cutSceneController)
        {
            Debug.Log("Hello cut scene controller");
            cutSceneController.PlayCutScene(nearestSpawn);
        }
        else
        {
            nearestSpawn.GetComponent<PlayableDirector>().Play();
            Debug.Log("CutScene spawn played");

            // for map 1

            if (level == 1)
            {
                Debug.Log("Respawn Level 1");
                GameObject.Find("Game Manager").GetComponent<GameManager>().TestSpawnPlayer(this.gameObject);
            }

            // for map 4
            if (level == 4)
            {
                GameObject.Find("Game Manager 2").GetComponent<GameManager2>().Respawn();
            }

            // for map 5
            if (level == 5)
            {
                GameObject enemy = GameObject.Find("PrefabManager").GetComponent<PrefabManager>().currentEnemy;

                enemy.GetComponent<WolfBoss>().enabled = true;
                enemy.GetComponent<WolfAI>().enabled = true;
            }
        }

        yield return new WaitForSeconds(0.6f);
        UnBlockPlayerMovement();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cut Scene Trigger"))
        {
            StartCoroutine(WaitForCutSceneEnd(other));
        }
    }

    IEnumerator WaitForCutSceneEnd(Collider cutScene)
    {
        // if cut scene in level 5

        GameObject item = null;
        GameObject enemy = null;

        int level = GameObject.Find("Game Manager 2").GetComponent<GameManager2>().sceneIndex;

        if (level == 5)
        {
            item = GameObject.Find("PrefabManager").GetComponent<PrefabManager>().LocketNecklace;
            enemy = GameObject.Find("PrefabManager").GetComponent<PrefabManager>().currentEnemy;

            item.SetActive(false);
            enemy.GetComponent<WolfBoss>().enabled = false;
            enemy.GetComponent<WolfAI>().enabled = false;
        }


        //camera 
        GameObject camera = GameObject.Find("Main Camera");

        //block player movement to deal with interupting animation
        transform.GetComponent<PlayerMovement3>().canMove = false;

        animator.SetBool("idle", true);

        animator.SetBool("running", false);
        animator.SetBool("walking", false);
        animator.SetBool("sneaky", false);
        animator.ResetTrigger("attack");
        animator.ResetTrigger("roll");
        animator.Play("Mick|Mick_Idle");



        if (GameObject.Find("Overlay2"))
        {
            Debug.Log("Hello");
            GameObject.Find("Overlay2").GetComponent<Animation>().Play("overlay-end");
        }
        yield return new WaitForSeconds(1f);

        //turn off UI
        GameObject ui = GameObject.Find("UI");
        if (ui)
        {
            ui.SetActive(false);
        }

        // turn off main camera
        if (camera)
        {
            camera.SetActive(false);
        }

        cutScene.GetComponent<PlayableDirector>().Play();


        if (GameObject.Find("Overlay2"))
        {
            GameObject.Find("Overlay2").GetComponent<Animation>().Play("overlay-start");
        }

        yield return new WaitForSeconds((float) cutScene.GetComponent<PlayableDirector>().duration);

        if (ui)
        {
            ui.SetActive(true);
        }

        if (GameObject.Find("Overlay2"))
        {
            Debug.Log("Hello");
            GameObject.Find("Overlay2").GetComponent<Animation>().Play("overlay-start");
        }

        if (camera)
        {
            camera.SetActive(true);
        }

        //yield return new WaitForSeconds(0.5f);
        //if (GameObject.Find("Overlay2"))
        //{
        //    GameObject.Find("Overlay2").GetComponent<Animation>().Play("overlay-start");
        //}

        transform.GetComponent<PlayerMovement3>().canMove = true;

        cutScene.gameObject.SetActive(false);

        if (level == 5)
        {
            item.SetActive(true);
            enemy.GetComponent<WolfBoss>().enabled = true;
            enemy.GetComponent<WolfAI>().enabled = true;

        }

    }

    //public void LoadPlayer()
    //{
    //    PlayerData data = SaveSystem.LoadPlayer();

    //    currentHealth = data.health;

    //    Vector3 position;
    //    position.x = data.position[0];
    //    position.y = data.position[1];
    //    position.z = data.position[2];

    //    transform.position = position;
    //}
}
    