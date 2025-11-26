using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapArea : MonoBehaviour
{
    [SerializeField] string areaName;
    [SerializeField] AudioClip backgroundMusic;
    [SerializeField] List<ToyoEncounterRecord> wildToyos;

    [HideInInspector]
    [SerializeField] int totalChance = 0;

    public static MapArea i { get; private set; }
    private void Awake()
    {
        i = this;
        CalculateChancePercentage();
    }

    private void Start()
    {
        if (!string.IsNullOrEmpty(areaName))
            StartCoroutine(ShowAreaName());

        if (backgroundMusic != null)
            AudioManager.i?.PlayMusic(backgroundMusic, fade: true);
    }

    void CalculateChancePercentage()
    {
        totalChance = -1;

        if (wildToyos.Count > 0)
        {
            totalChance = 0;
            foreach (var record in wildToyos)
            {
                record.chanceLower = totalChance;
                record.chanceUpper = totalChance + record.chancePercentage;

                totalChance = totalChance + record.chancePercentage;
            }
        }
    }

    public Toyo GetRandomWildToyo()
    {
        var toyoList = wildToyos;

        int randVal = Random.Range(1, 101);
        var toyoRecord = toyoList.First(p => randVal >= p.chanceLower && randVal <= p.chanceUpper);

        var levelRange = toyoRecord.levelRange;
        int level = levelRange.y == 0 ? levelRange.x : Random.Range(levelRange.x, levelRange.y + 1);

        var wildToyo = new Toyo(toyoRecord.toyo, level);
        //wildToyo.Init();
        return wildToyo;
    }

    IEnumerator ShowAreaName()
    {
        yield return new WaitForSeconds(1f);
        MessageUI.i?.ShowMessage(areaName);
    }
}

[System.Serializable]
public class ToyoEncounterRecord
{
    public ToyoBase toyo;
    public Vector2Int levelRange;
    public int chancePercentage;

    public int chanceLower { get; set; }
    public int chanceUpper { get; set; }
}
