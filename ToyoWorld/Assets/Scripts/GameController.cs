using GDEUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController i { get; private set; }
    private void Awake()
    {
        i = this;
    }

    public StateMachine<GameController> StateMachine { get; private set; }
    private void Start()
    {
        StateMachine = new StateMachine<GameController>(this);
        StateMachine.ChangeState(FreeRoamState.i);
    }

    private void Update()
    {
        StateMachine.Execute();
    }
}
