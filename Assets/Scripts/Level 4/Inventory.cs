using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Inventory;

public class Inventory : MonoBehaviour
{
    // Start is called before the first frame update

    public InventorySlot[] inventorySlots;
    private int selectedIndex = 0;

    void Start()
    {
        selectedIndex = 0;
        inventorySlots[selectedIndex].isSelected = true;
    }

    // Update is called once per frame
    void Update()
    {
        PickUp();

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedIndex = 0;
            UpdateSelectedItem();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedIndex = 1;
            UpdateSelectedItem();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectedIndex = 2;
            UpdateSelectedItem();
        }

        CheckDrop();

    }

    void PickUp()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {

            float pickupRange = 1f;

            Collider[] colliderArray = Physics.OverlapSphere(transform.position, pickupRange);

            foreach (Collider collider in colliderArray)
            {
                if (collider.TryGetComponent<PickUpItem>(out PickUpItem item))
                {
                    item.AddToInventory();
                    GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("Item Pick");

                    break;
                }
            }
        }
    }

    public PickUpItem GetPickUpItem()
    {

        List<PickUpItem> itemList = new List<PickUpItem>();
        float pickupRange = 1f;

        Collider[] colliderArray = Physics.OverlapSphere(transform.position, pickupRange);

        foreach (Collider collider in colliderArray)
        {
            if (collider.TryGetComponent<PickUpItem>(out PickUpItem item))
            {
                itemList.Add(item);
            }
        }

        PickUpItem closestItem = null;

        foreach (PickUpItem item in itemList)
        {
            if (closestItem == null)
            {
                closestItem = item;
            }
            else
            {
                if (Vector3.Distance(transform.position, item.transform.position) < Vector3.Distance(transform.position, closestItem.transform.position))
                {
                    closestItem = item;
                }
            }
        }
        return closestItem;
    }

    public void CheckDrop()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            InventorySlot selectedSlot = inventorySlots[selectedIndex];
            selectedSlot.Drop();
        }
    }

    public void UpdateSelectedItem()
    {
        foreach (InventorySlot inventorySlot in inventorySlots)
        {
            inventorySlot.isSelected = false;
        }
        inventorySlots[selectedIndex].isSelected = true;
    }

    public void ClearInventory()
    {
        foreach (InventorySlot inventorySlot in inventorySlots)
        {
            GameObject preFab = inventorySlot.Drop();
            if (preFab != null)
            {
                Destroy(preFab);
            }
        }
    }

    public void UnlockJumpingSkill()
    {
        int boneCount = 0;
        foreach (InventorySlot inventorySlot in inventorySlots)
        {
            if (inventorySlot.itemStored)
            {
                if (inventorySlot.itemStored.itemName == "Bone")
                {
                    boneCount++;
                }
            }
        }

        if (gameObject.GetComponent<PlayerJump>())
        {
            if (boneCount >= 2)
            {
                // player.canJump = true
                gameObject.GetComponent<PlayerJump>().canJump = true;
            } else
            {
                gameObject.GetComponent<PlayerJump>().canJump = false;
            }
        }

    }

}
