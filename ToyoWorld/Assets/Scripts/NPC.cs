using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[System.Serializable]
public class NPC : MonoBehaviour
{
    private DialogueSystem dialogueSystem;

    public string Name;
    public bool isObject;

    public Animator ani { get; set; }
    private Transform playerPos;
    //public Transform npcPos;

    [TextArea(5, 10)]
    public string[] sentences;

    void Start()
    {
        ani = GetComponent<Animator>();
        playerPos = FindObjectOfType<ThirdPersonMovement>().transform;
        dialogueSystem = FindObjectOfType<DialogueSystem>();
    }

    void Update()
    {
        if (!isObject)
        {
            if(playerPos == null)
            {
                playerPos = FindObjectOfType<ThirdPersonMovement>().transform;
            }
            Vector3 npcPos = gameObject.transform.position;
            Vector3 delta = new Vector3(playerPos.position.x - npcPos.x, 0.0f, playerPos.position.z - npcPos.z);
            Quaternion rotation = Quaternion.LookRotation(delta);
            gameObject.transform.rotation = rotation;
        }
    }

    public void OnTriggerStay(Collider other)
    {
        this.gameObject.GetComponent<NPC>().enabled = true;
        /*
        if ((other.gameObject.tag == "Player") && Input.GetKeyDown(KeyCode.F))
        {
            ani.SetTrigger("Talk");
            this.gameObject.GetComponent<NPC>().enabled = true;
            dialogueSystem.Names = Name;
            dialogueSystem.dialogueLines = sentences;
            dialogueSystem.NPCName();
        }
        */
    }

    public void Talk()
    {
        if (!isObject)
        {
            ani.SetTrigger("Talk");
        }
        this.gameObject.GetComponent<NPC>().enabled = true;
        dialogueSystem.Names = Name;
        dialogueSystem.dialogueLines = sentences;
        dialogueSystem.NPCName();
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out DialogueSystem dialogueSystem))
        {
            dialogueSystem.OutOfRange();
        }
        this.gameObject.GetComponent<NPC>().enabled = false;
    }
}