using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FoodBarController : MonoBehaviour
{
    // Start is called before the first frame update

    private Slider slider;
    [SerializeField] private Gradient gradient;
    [SerializeField] private Image fill;


    [SerializeField] private PlayerMovement2 player2;
    [SerializeField] private PlayerMovement3 player3;
    [SerializeField] private ThirdPersonMovement player0;

    [SerializeField] private PlayerMovement player1;


    public float hunger = 64f;

    public float Hunger {
        get { return hunger; }
        set { hunger = value; }
    }

    void Start()
    {
        slider = GetComponent<Slider>();
        slider.maxValue = hunger;
        slider.value = hunger;
    }

    // Update is called once per frame
    void Update()
    {
        if (player1 != null)
        {
            if (player1.isRunning)
            {
                hunger -= Time.deltaTime * 2;
            } else
            {
                hunger -= Time.deltaTime;
            }
            slider.value = hunger;
        } else if (player2 != null)
        {
            if (player2.isRunning)
            {
                hunger -= Time.deltaTime * 2;
            }
            else
            {
                hunger -= Time.deltaTime;
            }
            slider.value = hunger;
        } else if (player3 != null)
        {
            if (player3.isRunning)
            {
                hunger -= Time.deltaTime * 2;
            }
            else
            {
                hunger -= Time.deltaTime;
            }
            slider.value = hunger;
        } else if (player0 != null)
        {
            if (player1.isRunning)
            {
                hunger -= Time.deltaTime * 2;
            }
            else
            {
                hunger -= Time.deltaTime;
            }
            slider.value = hunger;
        }
    }
}
