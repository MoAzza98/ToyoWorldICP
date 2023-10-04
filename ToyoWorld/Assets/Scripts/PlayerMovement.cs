using Cysharp.Threading.Tasks.Triggers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float runSpeed;
    private float currentSpeed;

    public float groundDrag;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;
    public Transform lookAt;

    public Animator animator;
    private CameraSetup cameraSetup;
    public GameObject playerObj;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    [SerializeField] public Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cameraSetup = FindObjectOfType<CameraSetup>();
        rb.freezeRotation = true;
        
        cameraSetup.playerCam.orientation = orientation;
        cameraSetup.playerCam.player = gameObject.transform;
        cameraSetup.playerCam.playerObj = gameObject.transform;
        cameraSetup.playerCam.rb = rb;

        cameraSetup.freeLookCam.LookAt = lookAt;
        cameraSetup.freeLookCam.Follow = gameObject.transform;

    }

    public void FixedUpdate()
    {
        //Debug.Log("Should be able to move");
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = runSpeed;
        }
        else
        {
            currentSpeed = moveSpeed;
        }

        MyInput();
        SpeedControl();

        if(grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
        MovePlayer();
    }

    void Update()
    {
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        rb.AddForce(moveDirection.normalized * currentSpeed * 10f, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if(flatVel.magnitude > currentSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * currentSpeed;

            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);

            animator.SetFloat("Speed", Mathf.Clamp(limitedVel.magnitude, 0 , runSpeed));
            //Debug.Log($"Speed: " + Mathf.Clamp(limitedVel.magnitude, 0, runSpeed));
        }
        animator.SetFloat("Speed", Mathf.Clamp(flatVel.magnitude, 0, runSpeed));
        //Debug.Log($"Speed: " + Mathf.Clamp(flatVel.magnitude, 0, runSpeed));
    }

}
