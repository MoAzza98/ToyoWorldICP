using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PartyScreen : MonoBehaviour
{
    PartyMemberUI[] memberSlots;
    [SerializeField] TextMeshProUGUI messageText;
    [SerializeField] GameObject backButton;

    // Start is called before the first frame update
    public void Init()
    {
        memberSlots = GetComponentsInChildren<PartyMemberUI>();
    }

    public void SetPartyData(List<Toyo> toyos)
    {
        for(int i = 0; i < memberSlots.Length; i++)
        {
            if(i < toyos.Count)
            {
                memberSlots[i].SetData(toyos[i]);
            }
            else
            {
                memberSlots[i].gameObject.SetActive(false);
            }
        }
        messageText.text = "Choose a Toyo";
    }

    public void SetMessageText(string message) 
    {
        messageText.text = message;
    }

    public void CanLeaveScreen(bool canLeave)
    {
        if(canLeave)
        {
            backButton.SetActive(true);
        }
        else
        {
            backButton.SetActive(false);
        }
    }
}
