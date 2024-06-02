using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildToyoSpawner : MonoBehaviour
{
    [SerializeField] Vector2 idleTimeRange = new Vector2(2, 6);
    [SerializeField] Vector2 wanderRange = new Vector2(3, 8);

    private void Start()
    {
        var toyo = MapArea.i.GetRandomWildToyo();


        var model = Instantiate(toyo.Base.Model, transform.position, Quaternion.identity);
        Debug.Log(model);

        var wildToyo = model.AddComponent<WildToyo>();
        wildToyo.SetData(toyo, idleTimeRange, wanderRange);

        model.SetActive(true);
    }
}
