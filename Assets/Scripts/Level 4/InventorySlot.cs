using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using static UnityEditor.Progress;


public class InventorySlot : MonoBehaviour
{
    public bool isFull;
    public GameObject selectedUI;

    public Item itemStored;

    public Image itemUI;

    public bool isSelected;

    public Transform dropPosition;

    void Start()
    {
        isFull = false;
        isSelected = false;
    }

    void Update()
    {
        if (isSelected)
        {
            selectedUI.SetActive(true);
        } else
        {
            selectedUI.SetActive(false);
        }
    }

    public GameObject Drop()
    {
        //itemStored = item;
        GameObject dropPrefab = null;
        if (itemStored)
        {
            isFull = false;
            itemUI.sprite = null;
            dropPrefab = Instantiate(itemStored.itemPrefab, dropPosition.position, Quaternion.identity);
            itemStored = null;
            GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("Item Drop");
        }
        return dropPrefab;
    }

    public void PickUp(Item item)
    {
        itemStored = item;
        isFull = true;
        if (itemStored)
        {
            itemUI.sprite = itemStored.itemSprite;
        }
    }

}
