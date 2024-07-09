using UnityEngine;

public class GhostStateScatter : GhostStateBase
{
    Transform ScatterSlot;
    private Ghost ghost;
    public GhostStateScatter(Rigidbody2D rigidbody2D, 
        Movement movement, Movement targetMovement, 
        Transform ScatterSlot, Ghost ghost)
    {
        SetInformation(rigidbody2D, movement, targetMovement);
        this.ScatterSlot = ScatterSlot;
        this.ghost = ghost;
    }
    
    public override Vector2 GetDirection()
    {
        return PathFinding(ScatterSlot.position, ghost.IsHome());
    }

    public override void Enter()
    {
        ghost.SetStateWithDelay(GhostState.Chase, 7);
    }
}
