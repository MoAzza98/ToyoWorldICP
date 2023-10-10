using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TitleUnlockManager : MonoBehaviour
{
    [SerializeField] GameObject title1;
    [SerializeField] GameObject title2;
    [SerializeField] GameObject title3;
    [SerializeField] GameObject title4;
    [SerializeField] GameObject mysteryUnlock;

    [SerializeField] GameObject name1;
    [SerializeField] GameObject name2;
    [SerializeField] GameObject name3;
    [SerializeField] GameObject name4;
    [SerializeField] GameObject newbie;

    public List<TextMeshProUGUI> counterText;

    // Start is called before the first frame update
    void Start()
    {
        CheckButtons();
        foreach (var t in counterText)
        {
            t.text = $"{GameController.instance.toyosDefeated}{t.text}";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckButtons()
    {
        if(GameController.instance.toyosDefeated > 10)
        {
            title1.SetActive(true);
        }
        if (GameController.instance.toyosDefeated > 20)
        {
            title2.SetActive(true);
        }
        if (GameController.instance.toyosDefeated > 30)
        {
            title3.SetActive(true);
        }
        if (GameController.instance.toyosDefeated >= 50)
        {
            title4.SetActive(true);
            mysteryUnlock.SetActive(true);
        }
    }

    public void EquipTitleNewbie()
    {
        newbie.SetActive(true);
        name1.SetActive(false);
        name2.SetActive(false);
        name3.SetActive(false);
        name4.SetActive(false);
    }

    public void EquipTitleOne()
    {
        name1.SetActive(true);
        name2.SetActive(false);
        name3.SetActive(false);
        name4.SetActive(false);
        newbie.SetActive(false);
    }

    public void EquipTitleTwo()
    {
        name1.SetActive(false);
        name2.SetActive(true);
        name3.SetActive(false);
        name4.SetActive(false);
        newbie.SetActive(false);
    }

    public void EquipTitleThree()
    {
        name1.SetActive(false);
        name2.SetActive(false);
        name3.SetActive(true);
        name4.SetActive(false);
        newbie.SetActive(false);
    }

    public void EquipTitleFour()
    {
        name1.SetActive(false);
        name2.SetActive(false);
        name3.SetActive(false);
        name4.SetActive(true);
        newbie.SetActive(false);
    }
}
