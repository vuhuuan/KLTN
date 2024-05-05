using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using static UnityEditor.Progress;

public class PickUpItem : MonoBehaviour
{
    private Inventory inventory;
    public Item item;
    void Start()
    {
        inventory = GameObject.Find("Mick3 Player").GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void AddToInventory()
    {
        Debug.Log("Add item to inventory successfully");
        for (int i = 0; i < inventory.inventorySlots.Length; i++)
        {
            InventorySlot slot = inventory.inventorySlots[i];
            if (!slot.isFull)
            {
                Debug.Log(slot.name);
                slot.PickUp(item);
                Destroy(gameObject);
                break;
            }
        }
    }
}
