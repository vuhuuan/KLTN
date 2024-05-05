using System.Collections;
using System.Collections.Generic;
//using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class HoldToCharge : MonoBehaviour
{
    [SerializeField] private Quest huntRabbitQuest;
    [SerializeField] private CutSceneController3 cutSceneController;


    [SerializeField] private float chargeTime = 2.0f; 
    private float currentCharge = 0.0f;
    
    [SerializeField] private Slider chargeBar;

    [SerializeField] private FoodBarController FoodBar;

    [SerializeField] private Animator player;
    [SerializeField] private Player playerHealth;



    bool showChargeBar = false;
    bool isChangeQuest = false;
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy")) {
            ReleaseAction();
        }
    }
    void Update()
    {
        Vector3 boxSize = GetComponent<BoxCollider>().size * 1.65f;

        Collider[] colliders = Physics.OverlapBox(transform.position, boxSize);
        foreach (Collider collider in colliders)
        {
            if (collider.GetComponent<Rabbit>())
            {
                Rabbit rabbit = collider.GetComponent<Rabbit>();
                
                if (rabbit.Dead)
                {
                    Eating(rabbit);
                }
            } else if (collider.GetComponent<PickUpItem>())
            {
                PickUpItem food = collider.GetComponent<PickUpItem>();
                EatingFood(food);
            }
        }

        if (huntRabbitQuest.isFinished && !isChangeQuest)
        {
            isChangeQuest = true;
            StartCoroutine(CutSceneAfterHuntRabbit(1));
        }
    }

    void ReleaseAction()
    {
        player.SetBool("eating", false);
        chargeBar.gameObject.SetActive(false);
        currentCharge = 0.0f;
        showChargeBar = false;
    }

    void MaxCharge()
    {
        Debug.Log("Max charge!");
        player.SetBool("eating", false);

    }

    void EatingFood(PickUpItem food)
    {
        if (Input.GetKey(KeyCode.Space))
        {
            player.SetBool("eating", true);
            if (!showChargeBar)
            {
                chargeBar.gameObject.SetActive(true);
                showChargeBar = true;
            }

            currentCharge += Time.deltaTime;

            chargeBar.value = currentCharge / chargeTime;

            currentCharge = Mathf.Clamp(currentCharge, 0.0f, chargeTime);

            if (currentCharge >= chargeTime)
            {
                GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("Collect Item");

                ReleaseAction();

                FoodEffect(food.item);

                MaxCharge();
                Debug.Log("Yummy");
                Destroy(food.gameObject);
            }
        }
        else
        {
            ReleaseAction();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            ReleaseAction();
        }
    }

    public void FoodEffect(Item food)
    {
        if (food.itemName == "Mushroom")
        {
            playerHealth.TakeDamage(3);
        }
        else if (food.itemName == "Bone")
        {
            // can jump;
        }
        FoodBar.Hunger += 30f;
    }
    void Eating(Rabbit rabbit)
    {
        if (Input.GetKey(KeyCode.Space) && rabbit)
        {
            player.SetBool("eating", true);
            if (!showChargeBar)
            {
                chargeBar.gameObject.SetActive(true);
                showChargeBar = true;
            }

            currentCharge += Time.deltaTime;

            chargeBar.value = currentCharge / chargeTime;

            currentCharge = Mathf.Clamp(currentCharge, 0.0f, chargeTime);

            if (currentCharge >= chargeTime)
            {
                GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("Collect Item");

                ReleaseAction();
                Destroy(rabbit.gameObject);
                MaxCharge();
                Debug.Log("Yummy");
                FoodBar.Hunger += 30f;

                huntRabbitQuest.currentCount++;
            }
        } else {
            ReleaseAction();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            ReleaseAction();
        }
    }

    IEnumerator CutSceneAfterHuntRabbit(int index)
    {
        gameObject.GetComponentInParent<Player>().BlockPlayerMovement();
        // should add overlay here
        GameObject.Find("Overlay2").GetComponent<Animation>().Play("overlay-end");
        yield return new WaitForSeconds(2f);
        //cutSceneController
        Debug.Log("Run Cut Scene 2");
        player.GetComponent<Player>().nearestSpawn = GameObject.Find("Cut scene 2.5");
        GameObject.Find("Overlay2").GetComponent<Animation>().Play("overlay-start");
        cutSceneController.PlayCutScene(index);
    }

}
