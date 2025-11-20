using GDEUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] List<GameObject> defaultObjects;

    public static GameController i { get; private set; }
    private void Awake()
    {
        i = this;
        DontDestroyOnLoad(transform.root.gameObject);

        ItemDB.Init();
        MoveDB.Init();
        ToyoDB.Init();
        QuestDB.Init();
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

    public void EnableDefaultObjects()
    {
        foreach (var item in defaultObjects)
        {
            item.SetActive(true);
        }
    }
}
