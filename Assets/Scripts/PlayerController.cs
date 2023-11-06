using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Steamworks;

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

    [Header("Pause Menu Attributes")]
    [SerializeField] CanvasGroup activeCanvas;
    [SerializeField] CanvasGroup menuCanvas; // the canvas groups for our menus
    float activeTargetAlpha = 1, menuTargetAlpha = 0; // what are the target alphas for our menus?
    bool inMenu; // are we currently in the menu?
    [SerializeField] float canvasLerpSpeed;
    [SerializeField] GameObject mainParent, creditsParent, confirmButton; // the main and credits parent objects
    [SerializeField] TMPro.TextMeshProUGUI creditsButtonText; // the text on the credits button
    [SerializeField] Slider sensitivitySlider, volumeSlider; // our sliders on the pause menu

    // callback for our pause menu
    protected Callback<GameOverlayActivated_t> SteamOverlayActivated;

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
        // ensure we initialize the steamworks manager
        if (SteamManager.Initialized)
        {
            string name = SteamFriends.GetPersonaName();
            Debug.Log(name);

            // set our values
            SetValues();
        }
        // lock cursor
        Cursor.lockState = CursorLockMode.Locked;

        // create our steam overlay callback
        SteamOverlayActivated = Callback<GameOverlayActivated_t>.Create(OnGameOverlayActivated);
    }


    void SetValues()
    {
        // setup the sliders in the pause menu
        sensitivitySlider.value = PlayerPrefs.GetFloat("sensitivity", 7);
        volumeSlider.value = PlayerPrefs.GetFloat("volume", 0.5f);
        AudioListener.volume = PlayerPrefs.GetFloat("volume", 0.5f);
        PlayerCameraController.instance.aimSensitivity = PlayerPrefs.GetFloat("sensitivity", 7);
    }

    private void Update()
    {
        ProcessUpdateInputs();

        if (Input.GetKeyDown(KeyCode.F12))
        {
            ResetGame();
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

        // process our menu alpha
        UpdatePauseMenuAlpha();
    }

    // when the steam overlay is active
    private void OnGameOverlayActivated(GameOverlayActivated_t pCallback)
    {
        if (pCallback.m_bActive != 0)
        {
            OpenPauseMenu();
        }
    }

    void ProcessUpdateInputs()
    {
        // pausing input
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Backspace))
        {
            // toggle our pause menu
            TogglePauseMenu();
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

    // reset the entire game.
    public void ResetGame()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("Hub");
    }

    // toggles our pause menu on and off
    void TogglePauseMenu()
    {
        // toggle the menu
        inMenu = !inMenu;

        // set our values based on the menu state
        switch (inMenu)
        {
            case true:
                // set our alphas
                activeTargetAlpha = 0;
                menuTargetAlpha = 1;
                // make sure tat we cannot move, and can interact with the menu
                // menuCanvas.interactable = true;
                canMove = false;
                PlayerCameraController.instance.canControl = false;
                // set our mouse to unlocked and visible
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                // update our slider values from our preferences
                sensitivitySlider.value = PlayerPrefs.GetFloat("sensitivity", 7);
                volumeSlider.value = PlayerPrefs.GetFloat("volume", 0.5f);

                break;

            case false:
                // set our alphas
                activeTargetAlpha = 1;
                menuTargetAlpha = 0;
                // make sure we cannot interact by accident, and that we can move again
                // menuCanvas.interactable = false;
                canMove = true;
                PlayerCameraController.instance.canControl = true;
                // set our mouse to locked and invisible
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                // hide the confirmation button for resetting the game
                confirmButton.SetActive(false);
                break;
        }
    }

    void OpenPauseMenu()
    {
        // set our alphas
        activeTargetAlpha = 0;
        menuTargetAlpha = 1;
        // make sure tat we cannot move, and can interact with the menu
        // menuCanvas.interactable = true;
        canMove = false;
        PlayerCameraController.instance.canControl = false;
        // set our mouse to unlocked and visible
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        // update our slider values from our preferences
        sensitivitySlider.value = PlayerPrefs.GetFloat("sensitivity", 7);
        volumeSlider.value = PlayerPrefs.GetFloat("volume", 0.5f);

    }

    // lerp our pause menu alphas
    void UpdatePauseMenuAlpha()
    {
        activeCanvas.alpha = Mathf.Lerp(activeCanvas.alpha, activeTargetAlpha, canvasLerpSpeed * Time.deltaTime);
        menuCanvas.alpha = Mathf.Lerp(menuCanvas.alpha, menuTargetAlpha, canvasLerpSpeed * Time.deltaTime);
    }

    // the function called by the show/hide credits button
    public void ToggleCredits()
    {
        // toggle our parents activity
        mainParent.SetActive(!mainParent.activeSelf);
        creditsParent.SetActive(!creditsParent.activeSelf);

        // based on activity, change the text on the button
        _ = mainParent.activeSelf == true ? creditsButtonText.text = "Show Credits" : creditsButtonText.text = "Hide Credits";
    }

    // for quitting the game
    public void QuitGame()
    {
        Application.Quit();
    }

    // toggle fullscreen
    public void ToggleFullscreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }

    // set our mouse sensitivity
    public void SetSensitivity()
    {
        // only run this when we are in a menu
        if (!inMenu) return;
        // set our mouse sensitivity
        PlayerCameraController.instance.aimSensitivity = sensitivitySlider.value;
        // save our mouse sensitivity to preferences
        PlayerPrefs.SetFloat("sensitivity", PlayerCameraController.instance.aimSensitivity);
        Debug.Log("setting value... " + PlayerPrefs.GetFloat("sensitivity"));
    }

    // set our volume
    public void SetVolume()
    {
        // only run this when we are in a menu
        if (!inMenu) return;
        // set the audio of the game to our slider
        AudioListener.volume = volumeSlider.value;
        // save that to a floar
        PlayerPrefs.SetFloat("volume", volumeSlider.value);
        Debug.Log("setting value... " + PlayerPrefs.GetFloat("volume"));
    }

    // toggle confirmation
    public void ShowConfirm()
    {
        confirmButton.SetActive(true);
    }

}
