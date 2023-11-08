using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using Org.BouncyCastle.Security;

public class AILocomotion : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator anim;
    private Transform playerTarget;
    private Vector3 lastPosition;
    private float speed = 0f;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerTarget == null)
        {
            playerTarget = FindAnyObjectByType<ThirdPersonMovement>().transform;
        }

        try
        {
            agent.SetDestination(playerTarget.position);
            anim.SetFloat("Speed", speed);
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }
    }

    private void FixedUpdate()
    {
        speed = Mathf.Lerp(speed, (transform.position - lastPosition).magnitude / Time.deltaTime, 0.75f);
        lastPosition = transform.position;
    }
}
