using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class DialogueSystem : MonoBehaviour
{

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    public GameObject dialogueGUI;
    public Transform dialogueBoxGUI;
    public NPC character;

    public DialogueManager diaManager;

    [SerializeField] public AudioSource audioSource;


    public float letterDelay = 0.1f;
    public float letterMultiplier = 0.5f;

    public KeyCode DialogueInput = KeyCode.F;

    public string Names;

    public string[] dialogueLines;

    public bool letterIsMultiplied = false;
    public bool dialogueActive = false;
    public bool dialogueEnded = false;
    public bool outOfRange = true;

    void Start()
    {
        character = GetComponent<NPC>();
        audioSource = GetComponent<AudioSource>();
        if(dialogueText != null)
        {
            dialogueText.text = "";
        }
    }

    void Awake()
    {
        SetDialogueRefs();
    }

    void Update()
    {

    }

    void SetDialogueRefs()
    {
        diaManager = FindAnyObjectByType<DialogueManager>();
        nameText = diaManager.nameText;
        dialogueText = diaManager.panelText;
        dialogueGUI = diaManager.dialoguePanel;
        dialogueBoxGUI = diaManager.dialoguePanel.transform;
    }

    public void NPCName()
    {
        outOfRange = false;
        dialogueGUI.SetActive(true);
        nameText.text = Names;
        if (!dialogueActive)
        {
            dialogueActive = true;
            StartCoroutine(StartDialogue());
            //character.ani.SetInteger("animation", character.talkNum);
        }

    }

    private IEnumerator StartDialogue()
    {
        if (outOfRange == false)
        {
            int dialogueLength = dialogueLines.Length;
            int currentDialogueIndex = 0;

            while (currentDialogueIndex < dialogueLength || !letterIsMultiplied)
            {
                if (!letterIsMultiplied)
                {
                    letterIsMultiplied = true;
                    StartCoroutine(DisplayString(dialogueLines[currentDialogueIndex++]));

                    if (currentDialogueIndex >= dialogueLength)
                    {
                        dialogueEnded = true;
                        //Debug.Log(character.talkNum);
                        //StartCoroutine(animationReset(character.talkNum));

                    }
                }
                yield return 0;
            }

            while (true)
            {
                if (Input.GetKeyDown(DialogueInput) && dialogueEnded == false)
                {
                    break;
                }
                yield return 0;
            }
            dialogueEnded = false;
            dialogueActive = false;
            DropDialogue();
        }
    }

    private IEnumerator DisplayString(string stringToDisplay)
    {
        if (outOfRange == false)
        {
            int stringLength = stringToDisplay.Length;
            int currentCharacterIndex = 0;

            dialogueText.text = "";

            while (currentCharacterIndex < stringLength)
            {
                dialogueText.text += stringToDisplay[currentCharacterIndex];
                currentCharacterIndex++;

                if (currentCharacterIndex < stringLength)
                {
                    if (Input.GetKey(DialogueInput))
                    {
                        yield return new WaitForSeconds(letterDelay * letterMultiplier);

                        audioSource.Play();
                    }
                    else
                    {
                        yield return new WaitForSeconds(letterDelay * letterMultiplier);

                        audioSource.Play();
                    }
                }
                else
                {
                    dialogueEnded = false;
                    break;
                }
            }
            while (true)
            {
                if (Input.GetKeyDown(DialogueInput))
                {
                    break;
                }
                yield return 0;
            }
            dialogueEnded = false;
            letterIsMultiplied = false;
            dialogueText.text = "";
        }
    }

    public void DropDialogue()
    {
        dialogueGUI.SetActive(false);
        dialogueBoxGUI.gameObject.SetActive(false);
    }

    public void OutOfRange()
    {
        outOfRange = true;
        if (outOfRange == true)
        {
            letterIsMultiplied = false;
            dialogueActive = false;
            StopAllCoroutines();
            dialogueGUI.SetActive(false);
            dialogueBoxGUI.gameObject.SetActive(false);
        }
    }
}