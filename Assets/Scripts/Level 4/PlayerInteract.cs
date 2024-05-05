using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E)) 
        {
            float interactRange = 2f; 

            Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);

            foreach (Collider collider in colliderArray)
            {
                if (collider.TryGetComponent<NPCInteractable>(out NPCInteractable npc)) {
                    Debug.Log("Iteracting with: " + npc.gameObject);
                    GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("Interact");

                    npc.Interact();
                }
            }
        }
    }

    public NPCInteractable GetInteractableObject()
    {
        List<NPCInteractable> npcList = new List<NPCInteractable>();
        float interactRange = 2f;

        Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);

        foreach (Collider collider in colliderArray)
        {
            if (collider.TryGetComponent<NPCInteractable>(out NPCInteractable npc))
            {
                npcList.Add(npc);
            }
        }

        NPCInteractable closestNPC = null;

        foreach (NPCInteractable npc in npcList)
        {
            if (closestNPC == null)
            {
                closestNPC = npc;
            } else
            {
                if (Vector3.Distance(transform.position, npc.transform.position) < Vector3.Distance(transform.position, closestNPC.transform.position))
                {
                    closestNPC = npc;
                }
            }
        }
        return closestNPC;
    }
}
