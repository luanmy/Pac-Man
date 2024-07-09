using Unity.VisualScripting;
using UnityEngine;

public class HomeInside : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ghost"))
        {
            Ghost ghost = other.GetComponent<Ghost>();
            if(ghost.stateName == GhostState.Home)
                
                GameManager.Instance.SetGhostState(ghost, GhostState.Chase);
        }
    }
}
