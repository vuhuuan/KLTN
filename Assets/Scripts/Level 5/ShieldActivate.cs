using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldActivate : MonoBehaviour
{
    GameObject shield;
    Player player;
    private void Awake()
    {
        shield = transform.Find("Shield").gameObject;
        player = gameObject.GetComponent<Player>();
    }

    void Update()
    {
        if (transform.Find("Locket necklace"))
        {
            shield.SetActive(true);
            player.isProtected = true;
        } else
        {
            shield.SetActive(false);
            player.isProtected = false;
        }
    }
}
