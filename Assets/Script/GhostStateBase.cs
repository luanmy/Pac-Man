using System.Collections.Generic;
using UnityEngine;

struct PathFindingNode
{
    public float costToTarget;
    public int x;
    public int y;

    public PathFindingNode(Vector2Int pos)
    {
        this.costToTarget = -1;
        this.x = pos.x;
        this.y = pos.y;
    }
}

public abstract class GhostStateBase
{
    protected Rigidbody2D ridigbody2D;
    protected Movement movement;
    protected Movement targetMovement;

    public void SetInformation(Rigidbody2D rigidbody2D, Movement movement,  Movement targetMovement)
    {
        this.ridigbody2D = rigidbody2D;
        this.movement = movement;
        this.targetMovement = targetMovement;
    }

    public virtual Vector2 GetDirection()
    {
        return Vector2.zero;
    }

    protected Vector2 PathFinding(Vector2 target, bool ishomeBlockOkay = false)
    {
        int selfX = (int)ridigbody2D.position.x;
        int selfY = (int)ridigbody2D.position.y;
        PathFindingNode[] surrounding = new PathFindingNode[3];
        Vector2 lastDirection = movement.GetDirection();
        //only consider the direction that is not the opposite of the last direction
        if (lastDirection.x == 0)
        {
            int directionY = (int)lastDirection.y;
            
            
            //last direction is up
            if (directionY > 0)
            {
                surrounding[0] = new PathFindingNode(Vector2Int.up);
                surrounding[1] = new PathFindingNode(Vector2Int.left);
                surrounding[2] = new PathFindingNode(Vector2Int.right);
            }
            //last direction is down
            else
            {
                surrounding[0] = new PathFindingNode(Vector2Int.left);
                surrounding[1] = new PathFindingNode(Vector2Int.down);
                surrounding[2] = new PathFindingNode(Vector2Int.right);
            }
        }
        else
        {
            int directionX = (int)lastDirection.x;
            //last direction is right
            if (directionX > 0)
            {
                surrounding[0] = new PathFindingNode(Vector2Int.up);
                surrounding[1] = new PathFindingNode(Vector2Int.down);
                surrounding[2] = new PathFindingNode(Vector2Int.right);
                
            }
            //last direction is left
            else
            {
                surrounding[0] = new PathFindingNode(Vector2Int.up);
                surrounding[1] = new PathFindingNode(Vector2Int.left);
                surrounding[2] = new PathFindingNode(Vector2Int.down);
            }
        }
        for(int i = 0; i < 3; i++)
        {
            int x = surrounding[i].x + selfX;
            int y = surrounding[i].y + selfY;
            
            if(!ishomeBlockOkay && GameManager.Instance.IsHomeSlot(x, y))
                continue;
            
            if (IsNodeValid(x, y))
            {
                float costToTarget = CalculateCostToTarget(x, y, target);
                surrounding[i].costToTarget = costToTarget;
            }
        }

        float lowestCost = int.MaxValue;
        int bestNodeIndex = -1;
        for (int i = 0; i < 3; i++)
        {
            if(surrounding[i].costToTarget == -1)
                continue;
            if (surrounding[i].costToTarget < lowestCost)
            {
                bestNodeIndex = i;
                lowestCost = surrounding[i].costToTarget;
            }
        }
        if(bestNodeIndex == -1)
            return Vector2.zero;
        return new Vector2(surrounding[bestNodeIndex].x, surrounding[bestNodeIndex].y);
    }
    
    public virtual float CalculateCostToTarget(int x, int y, Vector2 target)
    {
        return Vector2.Distance(new Vector2(x, y), target);
    }   
    
    private bool IsNodeValid(int x, int y)
    {
        //out of scope
        return GameManager.Instance.isBlockInMap(y,x) && !GameManager.Instance.isBlockHasWall(y, x);
    }
    
    public virtual void Enter() {}
    public virtual void Exit() {}

}
