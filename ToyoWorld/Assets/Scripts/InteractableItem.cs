using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractableItem : MonoBehaviour
{
    [SerializeField] private string interactText;
    public NPC npcTalker;
    private HealToyos healer;
    [SerializeField] public NPCLines npcLines;
    [SerializeField] private Toyo toyo;
    private MapArea area;


    public bool isNPC;
    public bool isInteractable;
    public bool isTrainer;
    public bool isHealer;
    // Start is called before the first frame update
    void Start()
    {
        area = GetComponent<MapArea>();
        npcLines = GetComponent<NPCLines>();

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
        if (isNPC)
        {
            npcLines.SetLines();
        }
        if(isHealer)
        {
            healer.HealParty();
        }
        if (isTrainer)
        {
            GameController.instance.CallBossBattleStartMethod(area.GetRandomWildToyo());
        }
    }

    public string GetInteractText()
    {
        return interactText;
    }

}
