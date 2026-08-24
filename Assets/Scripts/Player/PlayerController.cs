using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

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

    private Vector3 knockbackVelocity;

    private float inputDirection = 1f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        controller = GetComponent<CharacterController>();
        jumpsRemaining = maxJumps;
    }


    void Update()
    {
        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * inputDirection * mouseSensitivity); // mouse look horizontal
        verticalRotation += Input.GetAxis("Mouse Y") * inputDirection * mouseSensitivity;            // mouse look vertical
        verticalRotation = Mathf.Clamp(verticalRotation, -lookClamp, lookClamp);    // clamp vertical
        cameraTransform.localEulerAngles = Vector3.left * verticalRotation;         // apply to camera

        Vector3 moveDirection = transform.forward * Input.GetAxis("Vertical") * inputDirection + transform.right * Input.GetAxis("Horizontal") * inputDirection;  // movement direction

        if (controller.isGrounded)
        {
            fallVelocity = -2f;
            jumpsRemaining = maxJumps; // reset jumps on landing
        }

        if (Input.GetKeyDown(KeyCode.Space) && jumpsRemaining > 0)
        {
            fallVelocity = jumpForce;
            jumpsRemaining--;
        }

        if (!controller.isGrounded)
        {
            fallVelocity += Physics.gravity.y * Time.deltaTime * gravityScale;
        }

        moveDirection.y = fallVelocity;                                                                                         // apply vertical velocity

        Vector3 verticalMove = new Vector3(0, moveDirection.y, 0);
        moveDirection.y = 0;                                                                                                    // remove vertical from horizontal movement

        controller.Move(moveDirection * moveSpeed * Time.deltaTime);                                                            // move player
        controller.Move(verticalMove * Time.deltaTime);                                                                         //vertical movement (gravity)
        controller.Move(knockbackVelocity * Time.deltaTime);
        knockbackVelocity = Vector3.Lerp(knockbackVelocity, Vector3.zero, Time.deltaTime * 5f);


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
