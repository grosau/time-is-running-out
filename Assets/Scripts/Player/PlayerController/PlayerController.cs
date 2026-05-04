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
    private float verticalRotation;
    private float fallVelocity;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        controller = GetComponent<CharacterController>();
    }


    void Update()
    {
        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * mouseSensitivity); // mouse look horizontal
        verticalRotation += Input.GetAxis("Mouse Y") * mouseSensitivity;            // mouse look vertical
        verticalRotation = Mathf.Clamp(verticalRotation, -lookClamp, lookClamp);    // clamp vertical
        cameraTransform.localEulerAngles = Vector3.left * verticalRotation;         // apply to camera

        Vector3 moveDirection = transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal");  // movement direction

        if (controller.isGrounded)
        {
            fallVelocity = -2f;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                fallVelocity = jumpForce;
            }
        }
        else
        {
            fallVelocity += Physics.gravity.y * Time.deltaTime * gravityScale;
        }

        moveDirection.y = fallVelocity;                                                                                         // apply vertical velocity

        Vector3 verticalMove = new Vector3(0, moveDirection.y, 0);
        moveDirection.y = 0;                                                                                                    // remove vertical from horizontal movement

        controller.Move(moveDirection * moveSpeed * Time.deltaTime);                                                            // move player
        controller.Move(verticalMove * Time.deltaTime);                                                                         //vertical movement (gravity)


    }
}
