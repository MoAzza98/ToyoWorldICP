using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ToyoParty : MonoBehaviour
{
    [SerializeField] List<Toyo> toyos;

    private void Awake()
    {
        foreach (var toyo in toyos)
        {
            toyo.Init();
        }
    }

    public Toyo GetHealthyToyo()
    {
        return toyos.Where(x => x.Hp > 0).FirstOrDefault();
    }

    public static GameObject SpawnModel(Toyo toyo, Vector3? position = null, Quaternion? rotation = null)
    {
        if (toyo.Model != null)
        {
            toyo.Model.SetActive(true);

            if (position != null)
                toyo.Model.transform.position = position.Value;

            if (rotation != null)
                toyo.Model.transform.rotation = rotation.Value;
        }
        else
        {
            toyo.SetModel(Instantiate(toyo.Base.Model, position.GetValueOrDefault(), rotation.GetValueOrDefault()));
        }

        return toyo.Model;
    }

    public List<Toyo> Toyos => toyos; 
}
