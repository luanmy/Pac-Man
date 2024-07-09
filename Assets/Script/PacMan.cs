using System;
using UnityEngine;

public class PacMan : MonoBehaviour
{
    private bool isMovingHorizontal = false;
    private bool isMovingVertical = false;
    private Animator animator;
    private Movement movement;
    //public InputSystem_Actions InputSystemActions;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<Movement>();
        movement.Initialize();
        //InputSystemActions = new InputSystem_Actions();
    }
    
    // private void OnEnable()
    // {
    //     InputSystemActions.Enable();
    // }
    //
    // private void OnDisable()
    // {
    //     InputSystemActions.Disable();
    // }

    

    // Update is called once per frame
    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        // Vector2 temp = InputSystemActions.Player.Move.ReadValue<Vector2>();
        // float horizontal = temp.x;
        // float vertical = temp.y;
        isMovingHorizontal = horizontal != 0;
        isMovingVertical = vertical != 0;
        
        if (isMovingHorizontal)
        {
            SetAnimation(new Vector2(horizontal, 0));
            if (horizontal > 0)
            {
                movement.SetDirection(Vector2.right);
            }
            else if (horizontal < 0)
            {
                movement.SetDirection(Vector2.left);
            }
        }
        else if (isMovingVertical)
        {
            SetAnimation(new Vector2(0, vertical));
            
            if (vertical > 0)
            {
                movement.SetDirection(Vector2.up);
            }
            else if (vertical < 0)
            {
                movement.SetDirection(Vector2.down);
            }
        }
    }

    public void SetAnimation(Vector2 direction)
    {
        animator.SetFloat("InputHorizontal", direction.x);
        animator.SetFloat("InputVertical", direction.y);
    }
    
    public void SetDirection(Vector2 direction)
    {
        SetAnimation(direction);
        movement.SetDirection(direction);
    }
    
}
