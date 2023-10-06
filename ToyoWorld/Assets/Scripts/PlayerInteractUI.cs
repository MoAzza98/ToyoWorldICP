using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInteractUI : MonoBehaviour
{
    [SerializeField] private GameObject containerGameObject;
    [SerializeField] private PlayerInteract playerInteract;
    [SerializeField] private TextMeshProUGUI interactText;

    private void Update()
    {
        if(playerInteract == null)
        {
            playerInteract = GameController.instance.player.GetComponent<PlayerInteract>();
        }
        else
        {
            if(playerInteract.GetInteractableObject() != null)
            {
                Show(playerInteract.GetInteractableObject());
            }
            else
            {
                Hide();
            }
        }
    }

    private void Show(InteractableItem item)
    {
        containerGameObject.SetActive(true);
        interactText.text = item.GetInteractText();
    }

    private void Hide()
    {
        containerGameObject.SetActive(false);
    }

}
