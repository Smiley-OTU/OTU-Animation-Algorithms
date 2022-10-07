using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] InputAction moveAction;
    [SerializeField] InputAction sprintAction;
    [SerializeField] InputAction jumpAction;
    [SerializeField] float moveSpeed = 2;
    [SerializeField] float sprintSpeed = 4;
    [SerializeField] float jumpHeight = 5.0f;
    private float jumpVelocity;

    Rigidbody body;
    AnimationManager animationManager;

	public enum Animations
	{
		WALK,
		RUN,
		JUMP,
		IDLE
	}

	private void OnEnable()
    {
        moveAction.Enable();
        sprintAction.Enable();
        jumpAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        sprintAction.Disable();
        jumpAction.Disable();
    }

    void Start()
    {
        animationManager = GetComponent<AnimationManager>();
        body = GetComponent<Rigidbody>();
        jumpVelocity = Physics.ArcFromDistance(jumpHeight);

        // Animation time should match jump time
        Debug.Log(Physics.ArcDuration(jumpHeight, jumpVelocity));
    }

    void Update()
    {
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        float yaw = moveInput.x;
        Vector3 rotation = transform.eulerAngles;
        rotation.y += yaw;
        transform.eulerAngles = rotation;
        Vector3 direction = transform.rotation * Vector3.forward;

        bool movement = Mathf.Abs(moveInput.y) > Mathf.Epsilon;
        bool sprint = sprintAction.IsPressed();

        float groundSpeed = moveInput.y * (sprint ? sprintSpeed : moveSpeed);
        body.velocity = new Vector3(direction.x * groundSpeed, body.velocity.y, direction.z * groundSpeed);

        if (movement)
        {
            if (sprint)
                animationManager.Change(Animations.RUN);
            else
                animationManager.Change(Animations.WALK);
        }
        else
            animationManager.Change(Animations.IDLE);

        // TODO -- make a timer to prevent multiple jumps until jump timer has expired
        if (jumpAction.triggered)
        {
            animationManager.Change(Animations.JUMP);
            body.velocity = new Vector3(body.velocity.x, jumpVelocity, body.velocity.z);
        }
    }
}
