using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    [SerializeField] Slider health;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetHP(float value)
    {
        health.value = value;
    }

    public IEnumerator SetHPSmooth(float newHP)
    {
        float currentHP = health.value;
        float changeAmount = currentHP - newHP;

        while(currentHP - newHP > Mathf.Epsilon)
        {
            currentHP -= changeAmount * Time.deltaTime;
            health.value = currentHP;
            yield return null;
        }
        health.value = newHP;
    }

}
