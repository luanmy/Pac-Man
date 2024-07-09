using UnityEngine;

public class GhostStateStayHome: GhostStateBase
{
    private Transform homeSlot;
    public GhostStateStayHome(Rigidbody2D rigidbody2D, 
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
    
    public override float CalculateCostToTarget(int x, int y, Vector2 target)
    {
        return 0;
    }

    public override void Exit()
    { 
        movement.normalSpeed();
        movement.SetDirection(GetDirection());
    }
    
    public override void Enter()
    {
        movement.freeze();
    }
    
}
