using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float walkSpeed = 3;
    [SerializeField] float runSpeed = 6;
    [SerializeField] float angularSpeed = 500;

    [SerializeField] float throwRange = 15f;

    [SerializeField] CinemachineFreeLook thirdPersonCam;
    [SerializeField] GameObject aimCamera;
    [SerializeField] GameObject gameControls;
    [SerializeField] Transform aimTarget;

    [SerializeField] Pokeball pokeballPrefab;

    bool hasControl = true;

    public bool CanThrowPokeball { get; set; } = true;

    Quaternion targetRotation;

    bool isRunning;
    bool isAiming;

    bool inAction;

    float aimAngle = 0f;

    public Vector3 HandOffset { get; set; }

    Transform camTransform;
    Camera camera;
    public Animator animator;
    CharacterController characterController;
    ToyoParty playerParty;
    PartyWidget partyWidget;
    public CinemachineFreeLook freeLookCam;

    public static PlayerController i { get; private set; }
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        camera = Camera.main;
        camTransform = Camera.main.transform;
        
        characterController = GetComponent<CharacterController>();
        playerParty = GetComponent<ToyoParty>();

        partyWidget = FindObjectOfType<PartyWidget>();
        animator = GetComponent<Animator>();

        i = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                //Show cursor
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                //Freeze player and cam movement
                PlayerController.i.FreezeCamera(true);
                PlayerController.i.SetControl(false);
            }
            else
            {
                //Hide cursor
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                //Allow cam and player movement
                hasControl = true;
                freeLookCam.enabled = true;
                PlayerController.i.FreezeCamera(false);
                PlayerController.i.SetControl(true);
            }
        }

        if (inAction)
            return;

        if (!hasControl)
        {
            animator.SetFloat("moveAmount", 0f, 0.2f, Time.deltaTime);
            return;
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Run"))
            isRunning = !isRunning;

        if (Input.GetButtonDown("Jump"))
        {
            StartCoroutine(DoAction("Dive"));
        }

        if (CanThrowPokeball)
        {
            if (isAiming && Input.GetButtonDown("Throw"))
            {
                isAiming = false;
                animator.SetBool("isAiming", isAiming);

                StartCoroutine(DoAction("Throw"));

                StartCoroutine(AsyncUtil.RunAfterTime(0.5f, () =>
                {
                    aimCamera.SetActive(false);
                    gameControls.SetActive(true);
                }));
                
            }
            else if (Input.GetButtonDown("Throw"))
            {
                Aim();
            }
            
        }

        float moveAmount = Mathf.Clamp01(Mathf.Abs(h) + Mathf.Abs(v));
        var moveInput = new Vector3(h, 0, v);
        float camYRotation = camTransform.rotation.eulerAngles.y;
        var moveDir = Quaternion.Euler(0, camYRotation, 0) * moveInput;

        float moveSpeed = isRunning ? runSpeed : walkSpeed;
        Vector3 velocity = moveDir * moveSpeed;
        velocity = AdjustVelocityToSlope(velocity);

        characterController.Move(velocity * Time.deltaTime);

        if (isAiming)
        {
            // Horizontal Aiming - Rotate the player
            float rotationY = transform.rotation.eulerAngles.y;
            rotationY += Input.GetAxis("Mouse X");
            targetRotation = Quaternion.Euler(0, rotationY, 0);

            // Vertical Aiming - Rotate the Aim Target
            aimAngle += -1f * Input.GetAxis("Mouse Y");
            aimAngle = Mathf.Clamp(aimAngle, -40f, 40f);
            aimTarget.localRotation = Quaternion.Euler(aimAngle, 0, 0);
        }
        else
        {
            if (moveAmount > 0)
                targetRotation = Quaternion.LookRotation(moveDir);
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation,
            angularSpeed * Time.deltaTime);

        animator.SetFloat("moveAmount", moveAmount * moveSpeed / runSpeed, 0.2f, Time.deltaTime);
    }

    private Vector3 AdjustVelocityToSlope(Vector3 velocity)
    {
        var ray = new Ray(transform.position, Vector3.down);

        if(Physics.Raycast(ray, out RaycastHit hitInfo, 0.2f))
        {
            var slopeRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            var adjustedVelocity = slopeRotation * velocity;

            if(adjustedVelocity.y < 0)
            {
                return adjustedVelocity;
            }
        }

        return velocity;
    }

    IEnumerator DoAction(string animName, Action onOver = null)
    {
        inAction = true;

        animator.CrossFade(animName, 0.2f);
        yield return null;

        var animState = animator.GetNextAnimatorStateInfo(0);

        float timer = 0f;
        while (timer <= animState.length)
        {
            timer += Time.deltaTime;

            if (animator.IsInTransition(0) && timer > 0.4f)
                break;

            yield return null;
        }

        inAction = false;

        onOver?.Invoke();
    }

    Pokeball pokeballObj;
    void Aim()
    {
        isAiming = true;
        animator.SetBool("isAiming", isAiming);

        aimCamera.SetActive(true);
        gameControls.SetActive(false);

        pokeballObj = Instantiate(pokeballPrefab, animator.GetBoneTransform(HumanBodyBones.RightHand));
        pokeballObj.transform.localPosition = HandOffset;
    }

    Vector3 targetPos;
    void ThrowPokeball()
    {
        Vector3 rayOrgin = camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f));

        if (Physics.Raycast(rayOrgin, camera.transform.forward, out RaycastHit hit, throwRange))
            targetPos = hit.point;
        else
            targetPos = rayOrgin + camera.transform.forward * throwRange;

        pokeballObj.ToyoParty = playerParty;
        pokeballObj.ToyoToSpawn = partyWidget.SelectedToyo;
        pokeballObj.LaunchToTarget(targetPos);
    }

    public void SetControl(bool hasControl)
    {
        this.hasControl = hasControl;
    }

    public void FreezeCamera(bool freeze)
    {
        thirdPersonCam.gameObject.SetActive(!freeze);
    }
}
