using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInteractUI : MonoBehaviour
{
    [SerializeField] private GameObject container;
    [SerializeField] private PlayerInteract playerInteract;
    [SerializeField] private TextMeshProUGUI interactTextUI;

    public bool isTalking = false;

    private void Update()
    {
        if (playerInteract.GetInteractableObject() != null && !isTalking)
        {
            Show(playerInteract.GetInteractableObject());
        } else
        {
            Hide();
        }
    }
    private void Show(NPCInteractable npcInteractable)
    {
        interactTextUI.text = npcInteractable.GetInteractText();
        container.SetActive(true);
    }

    private void Hide()
    {
        container.SetActive(false);
    }
}
