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
    [SerializeField] float jumpPower = 5;
    [SerializeField] float jumpDuration = 0.5f;

    [SerializeField] Material seen;
    [SerializeField] Material unseen;

    Rigidbody body;
    Animator animator;

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

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (jumpAction.triggered)
        {
            StartCoroutine(Jump());
        }

        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y).normalized;

        bool movement = moveDirection != Vector3.zero;
        bool sprint = sprintAction.IsPressed();

        transform.forward = movement? moveDirection : transform.forward;
        body.velocity = moveDirection * (sprint ? sprintSpeed : moveSpeed);

        if (sprint)
        {
            animator.SetBool("IsRunning", true);
        }
        else
        {
            animator.SetBool("IsRunning", false);
        }

        if (movement)
        {
            animator.SetBool("IsWalking", true);
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }
    }

    IEnumerator Jump()
    {
        animator.SetBool("IsJumping", true);
        float elaspedTime = 0.0f;

        while(elaspedTime < jumpDuration / 2)
        {
            body.velocity = new Vector3(body.velocity.x, Mathf.Lerp(jumpPower, 0, elaspedTime / (jumpDuration / 2)), body.velocity.z);
            elaspedTime += Time.deltaTime;
            yield return null;
        }
        elaspedTime = 0.0f;

        while (elaspedTime < jumpDuration / 2)
        {
            body.velocity = new Vector3(body.velocity.x, Mathf.Lerp(0, -jumpPower, elaspedTime / (jumpDuration / 2)), body.velocity.z);
            elaspedTime += Time.deltaTime;
            yield return null;
        }
        body.velocity = Vector3.zero;

        animator.SetBool("IsJumping", false);
        yield return null;
    }
}
