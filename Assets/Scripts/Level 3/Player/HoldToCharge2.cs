using System.Collections;
using System.Collections.Generic;
//using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class HoldToCharge2 : MonoBehaviour
{
    [SerializeField] private float chargeTime = 2.0f; 
    private float currentCharge = 0.0f;
    
    [SerializeField] private Slider chargeBar;

    [SerializeField] private FoodBarController FoodBar;

    //[SerializeField] private Animator anim;


    bool showChargeBar = false;
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy")) {
            ReleaseAction();
        }
    }
    void Update()
    {
        Vector3 boxSize = new(1, 1, 1);

        Collider[] colliders = Physics.OverlapBox(transform.position, boxSize);
        foreach (Collider collider in colliders)
        {
            if (collider.GetComponent<PickUpItem>())
            {
                PickUpItem food = collider.GetComponent<PickUpItem>();
                Eating(food);
            }
        }
    }

    void ReleaseAction()
    {
        //anim.SetBool("eating", false);
        chargeBar.gameObject.SetActive(false);
        currentCharge = 0.0f;
        showChargeBar = false;
    }

    void MaxCharge()
    {
        Debug.Log("Max charge!");
        //anim.SetBool("eating", false);

    }

    void Eating(PickUpItem food)
    {
        if (Input.GetKey(KeyCode.Space))
        {
            //anim.SetBool("eating", true);
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
        } else {
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
            gameObject.GetComponent<Player>().TakeDamage(3);
        }
        else if (food.itemName == "Bone")
        {
            gameObject.GetComponent<PlayerJump>().PlusBone();
        } 
        else if (food.itemName == "Egg")
        {
            GameObject.Find("Quest").GetComponent<QuestDisplay>().UpdateCurrentQuest();

            //
        }
        FoodBar.Hunger += 30f;
    }


}
