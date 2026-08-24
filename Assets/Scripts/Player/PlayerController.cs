using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private bool isActive = false;
    private CharacterController controller;

    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float gravityScale;

    [SerializeField] Transform cameraTransform;
    [SerializeField] float mouseSensitivity;
    [SerializeField] float lookClamp;

    [SerializeField] int maxJumps = 1;
    private int jumpsRemaining;
    private float verticalRotation;
    private float fallVelocity;
    [SerializeField] float fallMultiplier = 2f;

    private Vector3 knockbackVelocity;
    private float inputDirection = 1f;

    [SerializeField] Transform groundCheck;
    [SerializeField] float groundDistance = 0.4f;
    [SerializeField] LayerMask groundMask;
    private bool isGrounded;

    void Start()
    {
        GameManager.OnGameStateChanged += HandleStateChange;
        controller = GetComponent<CharacterController>();
        jumpsRemaining = maxJumps;
    }

    void Update()
    {
        if (!isActive) return;

        // ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        //Debug.Log("isGrounded: " + isGrounded);

        // mouse look
        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * inputDirection * mouseSensitivity);
        verticalRotation += Input.GetAxis("Mouse Y") * inputDirection * mouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -lookClamp, lookClamp);
        cameraTransform.localEulerAngles = Vector3.left * verticalRotation;

        // horizontal movement
        Vector3 moveDirection = transform.forward * Input.GetAxis("Vertical") * inputDirection + transform.right * Input.GetAxis("Horizontal") * inputDirection;

        // grounded logic
        if (isGrounded && fallVelocity < 0)
        {
            fallVelocity = -2f;
            jumpsRemaining = maxJumps;
        }

        // jump
        if (Input.GetKeyDown(KeyCode.Space) && jumpsRemaining > 0)
        {
            fallVelocity = jumpForce;
            jumpsRemaining--;
        }

        // gravity
        if (!isGrounded)
        {
            if (fallVelocity < 0)
                fallVelocity += Physics.gravity.y * gravityScale * fallMultiplier * Time.deltaTime;
            else
                fallVelocity += Physics.gravity.y * gravityScale * Time.deltaTime;
        }

        // movement
        Vector3 horizontalMove = new Vector3(moveDirection.x, 0, moveDirection.z) * moveSpeed;
        Vector3 finalMove = horizontalMove + new Vector3(0, fallVelocity, 0);
        controller.Move(finalMove * Time.deltaTime);
        controller.Move(knockbackVelocity * Time.deltaTime);
        knockbackVelocity = Vector3.Lerp(knockbackVelocity, Vector3.zero, Time.deltaTime * 5f);
    }

    void OnDestroy()
    {
        GameManager.OnGameStateChanged -= HandleStateChange;
    }

    void HandleStateChange(GameManager.GameState newState)
    {
        switch (newState)
        {
            case GameManager.GameState.Arena:
            case GameManager.GameState.Corridor:
                isActive = true;
                break;
            case GameManager.GameState.MainMenu:
            case GameManager.GameState.PowerUpSelection:
            case GameManager.GameState.GameOver:
                isActive = false;
                break;
        }
    }

    public void IncreaseMoveSpeed(float amount)
    {
        moveSpeed += amount;
    }

    public void IncreaseJumpforce(float amount)
    {
        jumpForce += amount;
    }

    public void IncreaseMaxJumps(int amount)
    {
        maxJumps += amount;
    }

    public void ApplyKnockback(Vector3 direction, float force)
    {
        fallVelocity = force;
        knockbackVelocity = direction * force;
    }

    public IEnumerator InvertControls(float duration)
    {
        inputDirection = -1f;
        yield return new WaitForSeconds(duration);
        inputDirection = 1f;
    }
}