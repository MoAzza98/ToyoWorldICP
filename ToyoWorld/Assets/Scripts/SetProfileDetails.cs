using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SetProfileDetails : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI hpText;
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI playerName;
    [SerializeField] Slider healthSlider;
    private bool detailsSet = false;

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            levelText.text = GameController.instance.gcParty.GetHealthyToyo().Level.ToString();
            hpText.text = $"{GameController.instance.gcParty.GetHealthyToyo().HP}/{GameController.instance.gcParty.GetHealthyToyo().MaxHP}";
            detailsSet = false;
        } catch(Exception e) {
            Debug.Log(e);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!detailsSet)
        {
            levelText.text = GameController.instance.gcParty.GetHealthyToyo().Level.ToString();
            hpText.text = $"{GameController.instance.gcParty.GetHealthyToyo().HP}/{GameController.instance.gcParty.GetHealthyToyo().MaxHP}";
            healthSlider.value = ((float)GameController.instance.gcParty.GetHealthyToyo().HP/GameController.instance.gcParty.GetHealthyToyo().MaxHP);
            Debug.Log(healthSlider.value);
            detailsSet = true;
        }
    }

    public void UpdateProfileDetails()
    {
        levelText.text = GameController.instance.gcParty.GetHealthyToyo().Level.ToString();
        hpText.text = $"{GameController.instance.gcParty.GetHealthyToyo().HP}/{GameController.instance.gcParty.GetHealthyToyo().MaxHP}";
        healthSlider.value = ((float)GameController.instance.gcParty.GetHealthyToyo().HP / GameController.instance.gcParty.GetHealthyToyo().MaxHP);
        Debug.Log(healthSlider.value);
    }
}
