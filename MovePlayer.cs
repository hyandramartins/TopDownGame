using System;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovePlayer : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody2D rigidbodyPlayer;
    private Vector2 moveInput;
    
    void Start()
    {
       rigidbodyPlayer = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rigidbodyPlayer.linearVelocity = speed * moveInput;
    }

    public void Move (InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        //moveInput = context.ReadValue<Vector2>().normalized; 
    }
}
