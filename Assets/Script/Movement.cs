using System;
using UnityEngine;
using UnityEngine.Events;

public class Movement : MonoBehaviour
{
    [SerializeField]
    private Vector2 destination;
    private Vector2 direction;
    private Vector2 lastPosition;
    private Vector2 nextDirection;
    private Rigidbody2D moveRigidbody2D;
    public float speedNormal = 5.0f;
    [SerializeField]
    private float speed;
    public float duration = 0.5f;
    public Vector2 initialDirection;
    public UnityEvent finishedMoveEvent;
    
    public bool isInitialized = false;

    public void Initialize()
    {
        isInitialized = true;
        moveRigidbody2D = GetComponent<Rigidbody2D>();
        lastPosition = moveRigidbody2D.position;
        direction = initialDirection;
        destination = moveRigidbody2D.position + direction;
        speed = speedNormal;
    }

    public void freeze()
    {
        speed = 0;
    }
    
    public void normalSpeed()
    {
        speed = speedNormal;
    }
    private void FixedUpdate()
    {
        if (!isInitialized)
            return;
        if (!IsFinished())
        {
            if (direction == Vector2.zero)
                return; 
            
            if (!IsValid(direction))
                return;
            
            Vector2 position = moveRigidbody2D.position;
            Vector2 translation = speed * Time.fixedDeltaTime * direction;
            moveRigidbody2D.MovePosition(position + translation);
        }
        else
        {
            moveRigidbody2D.MovePosition(destination);
            lastPosition = destination;
            finishedMoveEvent.Invoke();
            if (nextDirection != Vector2.zero)
            {
                if (IsValid(nextDirection))
                {
                    direction = nextDirection;
                    nextDirection = Vector2.zero;
                }
                
            }
            if (IsValid(direction))
            {
                destination = lastPosition + direction;
            }
            
        }
    }
    
    public void SetDirection(Vector2 direction)
    {
        nextDirection = direction;
    }
    
    private bool IsValid(Vector2 dir)
    {
        RaycastHit2D hit = Physics2D.Linecast(lastPosition, lastPosition + dir, 1 << LayerMask.NameToLayer("Wall"));
        return (hit == false);
    }
    
    private bool IsFinished()    
    {
        Vector2 pos = moveRigidbody2D.position;
        return Vector2.Distance(pos, destination) <= duration;
    }
    
    public Vector2 GetLastPosition()
    {
        return lastPosition;
    }
    
    public Vector2 GetDirection()
    {
        return direction;
    }
    
    public void FlipDirection()
    {
        direction = -direction;
        destination = lastPosition;
    }

    private void OnDestroy()
    {
        isInitialized = false;
    }
}
