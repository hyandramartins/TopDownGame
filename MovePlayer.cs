using System;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovePlayer : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody2D rigidbodyPlayer;
    private Animator animatorPlayer;
    private Vector2 moveInput;
    
    void Start()
    {
       rigidbodyPlayer = GetComponent<Rigidbody2D>();
       animatorPlayer = GetComponent<Animator>();
    }

    void FixedUpdate()
    {   
        Debug.Log("Velocidade: " + rigidbodyPlayer.linearVelocity);
        Debug.Log("Input: " + moveInput);
        rigidbodyPlayer.linearVelocity = speed * moveInput;
    }

    public void Move (InputAction.CallbackContext context)
    {   
        animatorPlayer.SetBool("isWalking", true);

        if (context.canceled)
        {
            animatorPlayer.SetBool("isWalking", false);
            animatorPlayer.SetFloat("LastInputX", moveInput.x);
            animatorPlayer.SetFloat("LastInputY", moveInput.y);
        }

        moveInput = context.ReadValue<Vector2>();
        Debug.Log(moveInput);
        animatorPlayer.SetFloat("InputX", moveInput.x);
        animatorPlayer.SetFloat("InputY", moveInput.y);
        //moveInput = context.ReadValue<Vector2>().normalized; 
    }
}
