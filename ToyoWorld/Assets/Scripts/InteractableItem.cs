using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractableItem : MonoBehaviour
{
    [SerializeField] private string interactText;
    public NPC npcTalker;
    private HealToyos healer;

    public bool isNPC;
    public bool isInteractable;
    public bool isHealer;
    // Start is called before the first frame update
    void Start()
    {
        if (isNPC)
        {
            npcTalker = GetComponent<NPC>();
        }
        if(isInteractable)
        {

        }
        if (isHealer)
        {
            healer = GetComponent<HealToyos>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        if (isInteractable)
        {

        }
        if(isNPC) 
        { 
            if (npcTalker != null)
            {
                npcTalker.Talk();
            }
        }
        if(isHealer)
        {
            healer.HealParty();
        }
    }

    public string GetInteractText()
    {
        return interactText;
    }

}
