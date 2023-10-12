using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectHandler : MonoBehaviour
{

    private GameObject castObject;
    private GameObject projectileObject;

    public Transform playerPoint;
    public Transform enemyPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CastEffect(Move move, Transform target)
    {
        castObject = move.Base.Effect;
        Instantiate(castObject, target.position, Quaternion.identity);
        //Destroy(castObject, 1f);
        //castObject.transform.LookAt(target);
    }

    public void SendProjectile(Move move)
    {

    }

}
