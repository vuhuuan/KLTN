using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    private Slider slider;
    [SerializeField] private Gradient gradient;
    [SerializeField] private Image fill;

    
    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public void setMaxHealth (int health) {  
        slider.maxValue = health;
        slider.value = health;
        fill.color = gradient.Evaluate(1f);
    }

    public void setHealth (int health)
    {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
