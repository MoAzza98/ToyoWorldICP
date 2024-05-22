using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AIToyoState { Idle, Wander, Chase }

public class WildToyo : MonoBehaviour
{
    [field: SerializeField] public Toyo Toyo { get; private set; }

    [SerializeField] Vector2 idleTimeRange = new Vector2(2, 6);
    [SerializeField] Vector2 wanderRange = new Vector2(3, 8);
    [SerializeField] LayerMask groundLayer;

    float idleTimer = 0f;
    Vector3 wanderTarget;

    AIToyoState state;


    NavMeshAgent navAgent;
    Animator animator;
    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.updatePosition = true;
        navAgent.updateRotation = true;

        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        Toyo.Init();
        Toyo.SetModel(gameObject);

        StartWanderState();
    }

    private void Update()
    {
        if (state == AIToyoState.Idle)
        {
            idleTimer -= Time.deltaTime;
            if (idleTimer <= 0f)
            {
                StartWanderState();
            }
        }
        else if (state == AIToyoState.Wander)
        {
            Debug.DrawLine(transform.position + Vector3.up * 0.5f, wanderTarget, Color.red);

            if (navAgent.remainingDistance <= 1f)
            {
                StartIdleState();
            }
        }
        else if (state == AIToyoState.Chase)
        {
            navAgent.SetDestination(PlayerController.i.transform.position);
        }

        animator.SetFloat("speed", navAgent.velocity.magnitude / navAgent.speed, 0.2f, Time.deltaTime);
    }

    void StartIdleState()
    {
        state = AIToyoState.Idle;
        idleTimer = Random.Range(idleTimeRange.x, idleTimeRange.y);
    }

    void StartWanderState()
    {
        state = AIToyoState.Wander;

        var randomDir = new Vector3(Random.Range(wanderRange.x, wanderRange.y) * (Random.Range(0, 2) == 0? 1 : -1), 0f, 
            Random.Range(wanderRange.x, wanderRange.y) * (Random.Range(0, 2) == 0 ? 1 : -1));

        wanderTarget = transform.position + randomDir;
        //NavMesh.SamplePosition(wanderTarget, out NavMeshHit navHit, wanderTarget.y, groundLayer);
        //wanderTarget = navHit.position;

        navAgent.SetDestination(wanderTarget);
    }
}
