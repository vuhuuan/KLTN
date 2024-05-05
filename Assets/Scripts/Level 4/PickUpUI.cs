using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PickUpUI : MonoBehaviour
{
    [SerializeField] private GameObject container;
    [SerializeField] private Inventory inventory;

    private void Update()
    {
        if (inventory.GetPickUpItem() != null)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }
    private void Show()
    {
        container.SetActive(true);
    }

    private void Hide()
    {
        container.SetActive(false);
    }
}
