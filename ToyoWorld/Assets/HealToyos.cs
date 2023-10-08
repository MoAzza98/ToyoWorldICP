using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealToyos : MonoBehaviour
{
    [SerializeField] GameObject healPanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    public void HealParty()
    {
        healPanel.SetActive(true);
        foreach(var member in GameController.instance.gcParty.ToyoPartyList)
        {
            member.Init();
        }
    }

    public void CloseHealPanel()
    {
        this.gameObject.SetActive(false);
    }
}
