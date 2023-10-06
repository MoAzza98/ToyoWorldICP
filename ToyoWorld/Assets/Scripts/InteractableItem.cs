using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractableItem : MonoBehaviour
{
    [SerializeField] private string interactText;
    public NPC npcTalker;
    // Start is called before the first frame update
    void Start()
    {
        npcTalker = GetComponent<NPC>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        npcTalker.Talk();
    }

    public string GetInteractText()
    {
        return interactText;
    }

}
