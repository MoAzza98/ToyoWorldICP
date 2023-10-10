using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    [SerializeField] GameObject lookAt;
    [SerializeField] Animator anim;
    public CharacterController controller;
    private CameraSetup cameraSetup;

    public float speed = 4f;
    public float runSpeed = 15f;
    private float currentSpeed = 0f;
    private float setSpeed = 0f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public float speedTransitionTime;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    // Start is called before the first frame update
    void Start()
    {
        cameraSetup = FindObjectOfType<CameraSetup>();
        cameraSetup.freeLookCam.LookAt = lookAt.transform;
        cameraSetup.freeLookCam.Follow = gameObject.transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        //Debug.Log(isGrounded);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Debug.Log("Jump pressed");
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraSetup.mainCam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                currentSpeed = Mathf.Lerp(currentSpeed, runSpeed, speedTransitionTime * Time.deltaTime);
                controller.Move(moveDir.normalized * currentSpeed * Time.deltaTime);
            }
            else
            {
                currentSpeed = Mathf.Lerp(currentSpeed, speed, speedTransitionTime * Time.deltaTime);
                controller.Move(moveDir.normalized * currentSpeed * Time.deltaTime);
            }
        }

        setSpeed = Mathf.Abs(direction.magnitude * currentSpeed);
        //Debug.Log($"Direction magnitude: {direction.magnitude} Speed + mag: {direction.magnitude * setSpeed}");

        anim.SetFloat("Speed", direction.magnitude * setSpeed);
    }
}
