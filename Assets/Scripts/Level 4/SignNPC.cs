using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SignNPC : NPCInteractable
{
    //public string[] scriptList;
    void Awake()
    {
        base.scriptList = this.scriptList;
    }

    // Update is called once per frame
    void Update()
    {
        base.CheckInteractInput();
    }

    public override void Interact()
    {
        base.Interact();
    }
}
