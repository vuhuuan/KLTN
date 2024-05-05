using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BonePowerUp : MonoBehaviour
{
    public ParticleSystem particleEffect;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Instantiate(particleEffect, other.transform.position, Quaternion.identity);
            GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("Collect Item");


            if (other.GetComponent<PlayerMovement2>().speed < 5f)
            {
                other.GetComponent<PlayerMovement2>().speed += 0.6f;
            } else
            {
                GameObject.Find("Notify Text").GetComponent<Animation>().Play("Float text");

            }
            
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
