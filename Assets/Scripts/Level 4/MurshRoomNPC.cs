using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MurshRoomNPC : NPCInteractable
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        base.CheckInteractInput();
    }

    public override void FinishInteract()
    {
        base.FinishInteract();
        GameObject.Find("Arrow Indicator").GetComponent<LookAtTarget>().target = GameObject.Find("Chicken Coop").transform;
    }
}
