using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class ToyoParty : MonoBehaviour
{
    [SerializeField] List<Toyo> toyoParty;
    // Start is called before the first frame update

    public List<Toyo> ToyoPartyList
    {
        get { return toyoParty; }
        set { toyoParty = value; }
    }

    void Start()
    {
        foreach (Toyo toyo in toyoParty)
        {
            toyo.Init();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Toyo GetHealthyToyo()
    {
        return toyoParty.Where(x => x.HP > 0).FirstOrDefault();

    }
}
