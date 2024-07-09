using UnityEngine;

public class GhostStateHome : GhostStateBase
{
    private Transform homeSlot;
    public GhostStateHome(Rigidbody2D rigidbody2D, 
        Movement movement,  Movement targetMovement, 
        Transform homeSlot)
    {
        SetInformation(rigidbody2D, movement, targetMovement);
        this.homeSlot = homeSlot;
    }
    
    public override Vector2 GetDirection()
    {
        return PathFinding(homeSlot.position, true);
    }
    
    public override void Exit()
    { 
        movement.FlipDirection();
    }
}
