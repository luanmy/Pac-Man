using UnityEngine;

public class GhostStateChase : GhostStateBase
{
    private Ghost ghost;
    public GhostStateChase(Rigidbody2D rigidbody2D, 
        Movement movement, Movement targetMovement, 
        Ghost ghost)
    {
        SetInformation(rigidbody2D, movement, targetMovement);
        this.ghost = ghost;
    }
    
    public override Vector2 GetDirection()
    {
        return PathFinding(targetMovement.GetLastPosition());
    }

    public override void Enter()
    {
        ghost.SetStateWithDelay(GhostState.Scatter, 7);

    }
}
