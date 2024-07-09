using UnityEngine;

public class GhostStateFrightened  : GhostStateBase
{
    Ghost ghost;
    public GhostStateFrightened(Rigidbody2D rigidbody2D, 
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
    
    public override float CalculateCostToTarget(int x, int y, Vector2 target)
    {
        return Random.Range(0, 100);
    }
    
    public override void Enter()
    {
        ghost.FrighenedSpriteChange();
        ghost.SetStateWithDelay(GhostState.Scatter, 7);
        
    }
    
    public override void Exit()
    {
        ghost.FrighenedSpriteChangeBack();
    }
}
