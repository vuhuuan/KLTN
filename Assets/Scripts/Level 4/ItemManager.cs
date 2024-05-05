using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public List<Item> itemList = new List<Item>(); 
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public Item GetItem(string name)
    {
        return itemList.Find(item => item.itemName == name);
    }
}
