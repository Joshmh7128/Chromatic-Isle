using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // script handles movement of the player
    [Header("Movement")]
    public Vector3 moveH;
    public Vector3 moveV, move, finalMove, processedFinalMove;
    [SerializeField] CharacterController characterController; // our character controller
    public float moveSpeed, gravity, jumpVelocity, moveSpeedAdjust; // set in editor for controlling
    public float moveLerpAxisDelta;
    RaycastHit groundedHit; // checking to see if we have touched the ground
    public float gravityValue, verticalVelocity, playerJumpVelocity; // hidden because is calculated
    public float gravityUpMultiplier = 1, gravityDownMultiplier = 1, gravityMidairMultiplier; // our multipliers for moving up and down with gravity
    public bool grounded;
    [SerializeField] Transform cameraRig; // our main camera rig

    [Header("Jump Stuff")]
    public float remainingJumps;
    public float maxJumps;
    public Vector3 knockbackVector; // how much we're getting knocked back
    public float knockbackRecoveryDelta; // how much we recover from knockback

    [Header("Collision and readout")]
    [SerializeField] public float velocity; // our velocity which we only want to read!
    [SerializeField] float playerHeight, playerWidth; // how tall is the player?
    [SerializeField] float groundCheckCooldown, groundCheckCooldownMax;
    public bool canMove = true; // can we move?
    int playerIgnoreMask;
    int ignoreLayerMask;

    // setup our instance
    public static PlayerController instance;
    public void Awake()
    {
        // compare our version number
        if (PlayerPrefs.GetString("version", Application.version) != Application.version)
        {
            PlayerPrefs.DeleteAll();
        }

        instance = this;
        // setup bit layer masks
        playerIgnoreMask = LayerMask.NameToLayer("PlayerIgnore");
        ignoreLayerMask = (1 << playerIgnoreMask);
        ignoreLayerMask = ~ignoreLayerMask;
    }


    private void Start()
    {
        // lock cursor
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        ProcessUpdateInputs();

        if (Input.GetKeyDown(KeyCode.F12))
        {
            PlayerPrefs.DeleteAll();
            SceneManager.LoadScene("Hub");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // process our movement inputs
        if (canMove)
        {
            ProcessMovement();
        }
    }

    void ProcessUpdateInputs()
    {
        // jumping input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (groundedHit.transform != null || remainingJumps > 0 && maxJumps > 0)
            {
                // jumping
                if (Input.GetKey(KeyCode.Space) && (groundCheckCooldown <= 0 || remainingJumps > 0))
                {
                    playerJumpVelocity = 0;
                    playerJumpVelocity = Mathf.Sqrt(-jumpVelocity * gravity);
                    remainingJumps--; // reduce jumps
                    groundCheckCooldown = groundCheckCooldownMax; // make sure we set the cooldown check

                }
            }
        }



    }

    // our movement function
    void ProcessMovement()
    {
        #region // Core Movement
        // declare our motion
        float pAxisV = Input.GetAxisRaw("Vertical");
        float pAxisH = Input.GetAxisRaw("Horizontal");
        Vector3 tmoveV = cameraRig.forward * pAxisV;
        Vector3 tmoveH = cameraRig.right * pAxisH;

        // then lerp
        moveV = tmoveV;
        moveH = tmoveH;

        if (groundCheckCooldown <= 0)
        {
            Physics.SphereCast(transform.position, playerWidth, Vector3.down, out groundedHit, playerHeight, ignoreLayerMask, QueryTriggerInteraction.Ignore);
        }

        if (groundCheckCooldown > 0)
        {
            playerJumpVelocity += gravityValue * Time.deltaTime;
            groundCheckCooldown -= Time.deltaTime;
        }

        // jump calculations
        if (gravityMidairMultiplier == 0) { gravityValue = gravity * gravityUpMultiplier * gravityDownMultiplier; } else { gravityValue = gravity * gravityMidairMultiplier; }

        if (groundedHit.transform == null)
        {
            playerJumpVelocity += gravityValue * Time.fixedDeltaTime;
            grounded = false;

        }

        if (groundedHit.transform != null)
        {
            if (!grounded)
            {
                // instantiate a visual effect
                remainingJumps = maxJumps;
                playerJumpVelocity = 0f;
                grounded = true;
            }
        }

        #endregion

        #region // Movement Application
        float finalMoveSpeed = moveSpeed * moveSpeedAdjust;
        // calculate vertical movement
        verticalVelocity = playerJumpVelocity;

        float moveX = Mathf.Clamp(moveH.x + moveV.x, -1, 1);
        float moveZ = Mathf.Clamp(moveH.z + moveV.z, -1, 1);
        // process all of our final moves
        move = new Vector3(moveX, verticalVelocity / moveSpeed, moveZ);
        move = AdjustVelocityToSlope(move);
        finalMove = Vector3.Lerp(finalMove, move, moveLerpAxisDelta * Time.deltaTime);
        Vector3 clampedFinal = Vector3.ClampMagnitude(new Vector3(finalMove.x, move.y, finalMove.z), 1);
        processedFinalMove = new Vector3(clampedFinal.x, move.y, clampedFinal.z);
        // knockback processing
        knockbackVector = Vector3.Lerp(knockbackVector, Vector3.zero, knockbackRecoveryDelta * Time.fixedDeltaTime);

        // add knockback vector
        processedFinalMove += knockbackVector;

        // apply final movement
        characterController.Move(processedFinalMove * Time.deltaTime * finalMoveSpeed);

        // output our velocity
        velocity = (Mathf.Abs(finalMove.x) + Mathf.Abs(finalMove.y) + Mathf.Abs(finalMove.z)) * finalMoveSpeed;
        #endregion
    }

    RaycastHit adjusterHit;
    private Vector3 AdjustVelocityToSlope(Vector3 velocity)
    {
        var ray = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(ray, out adjusterHit, 2f, ignoreLayerMask, QueryTriggerInteraction.Ignore))
        {
            var slopeRotation = Quaternion.FromToRotation(Vector3.up, adjusterHit.normal);
            var adjustedVelocity = slopeRotation * velocity; // this will align the velocity with the surface

            if (adjustedVelocity.y < 0)
            {
                return adjustedVelocity;
            }
        }

        return velocity;
    }

    // teleportation
    public void Teleport(Vector3 teleportPosition)
    {
        // reset velocities
        playerJumpVelocity = 0;
        verticalVelocity = 0;
        // turn off character controller
        characterController.enabled = false;
        StartCoroutine(TeleportBuffer(teleportPosition));
    }

    IEnumerator TeleportBuffer(Vector3 teleportPosition)
    {
        yield return new WaitForFixedUpdate();

        transform.position = teleportPosition + Vector3.up; // move the player up just a little bit so that they don't clip through the ground
        // turn on character controller
        characterController.enabled = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(adjusterHit.point, 0.1f);
    }



}
