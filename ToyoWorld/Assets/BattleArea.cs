using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleArea : MonoBehaviour
{
    [SerializeField] public GameController controller;
    float resetTimer = 5f;
    // Start is called before the first frame update
    void Start()
    {
        controller = FindAnyObjectByType<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        float isBattle = Random.Range(0f, 100f);

        if (isBattle > 66f)
        {

            Debug.Log("battle triggered");
            controller.CallBattleStartMethod();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        
        if (other.TryGetComponent(out ThirdPersonMovement playerMove) && resetTimer < 0)
        {
            BattleChance();
            resetTimer = 3f;
        }
        resetTimer -= Time.deltaTime;
        //Debug.Log(resetTimer);
    }

    private void BattleChance()
    {
        float isBattle = Random.Range(0f, 100f);

        if(isBattle > 10f)
        {
            
            Debug.Log("battle triggered");
            controller.CallBattleStartMethod();
        }

    }
}
