using System.Collections.Generic;
using System.Collections;
using UnityEngine;



public enum GhostState {
    Chase,
    Frightened,
    Home,
    Scatter,
    StayHome,
}

public class Ghost : MonoBehaviour
{
    private Movement movement;
    private Rigidbody2D ghostRigidbody2D;
    private GhostStateBase stateNow;
    private Transform target;
    private Dictionary<GhostState, GhostStateBase> stateDictionary = new Dictionary<GhostState, GhostStateBase>();
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer spriteRendererFrightened;
    private Coroutine setStateCoroutine;
    public GhostState stateName;
    public Transform scatterSlot;
    public Transform homeSlot;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnDestroy()
    {
        movement.finishedMoveEvent.RemoveListener(NewMove);
    }
    
    public void Initialize(GhostState stateName)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRendererFrightened = transform.GetChild(0).GetComponent<SpriteRenderer>();
        target = GameManager.Instance.GetPacMan();
        movement = GetComponent<Movement>();
        ghostRigidbody2D = GetComponent<Rigidbody2D>();
        Movement targetMovement = target.GetComponent<Movement>();
        stateDictionary.Add(GhostState.Chase, new GhostStateChase(ghostRigidbody2D, movement, targetMovement, this));
        stateDictionary.Add(GhostState.Frightened, new GhostStateFrightened(ghostRigidbody2D, movement, targetMovement, this));
        stateDictionary.Add(GhostState.Home, new GhostStateHome(ghostRigidbody2D, movement, targetMovement, homeSlot));
        stateDictionary.Add(GhostState.Scatter, new GhostStateScatter(ghostRigidbody2D, movement,targetMovement, scatterSlot, this));    
        stateDictionary.Add(GhostState.StayHome, new GhostStateStayHome(ghostRigidbody2D, movement, targetMovement, homeSlot));  
        movement.Initialize();
        SetState(stateName);
        movement.finishedMoveEvent.AddListener(NewMove);
    }
    
    void NewMove()
    {
        movement.SetDirection(stateNow.GetDirection());
    }

    public void SetState(GhostState state)
    {
        if (stateName == state && stateNow != null)
            return;
        if (stateNow != null)
        {
            stateNow.Exit();
            ClearCoroutine();
        }
        
        stateName = state;
        stateNow = stateDictionary[state];
        stateNow.Enter();
        movement.SetDirection(stateNow.GetDirection());
    }
    
    public IEnumerator SetStateCoroutine(GhostState state, float delay)
    {
        yield return new WaitForSeconds(delay);
        SetState(state);
    }

    public void SetStateWithDelay(GhostState state, float delay)
    {
        setStateCoroutine = StartCoroutine(SetStateCoroutine(state, delay));
    }
    
    public void ClearCoroutine()
    {
        if (setStateCoroutine != null)
        {
            StopCoroutine(setStateCoroutine);
            setStateCoroutine = null;
        }
    }

    public void FrighenedSpriteChange()
    {
        spriteRenderer.enabled = false;
        spriteRendererFrightened.enabled = true;
    }
    
    public void FrighenedSpriteChangeBack()
    {
        spriteRenderer.enabled = true;
        spriteRendererFrightened.enabled = false;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (stateName == GhostState.Frightened)
            {
                SetState(GhostState.Home);
            }
            else
            {
                GameManager.Instance.PlayerLoseLife();
                if (stateName == GhostState.Chase)
                {
                    SetState(GhostState.Scatter);
                }
            }
        }
    }

    public bool IsHome()
    {
        return GameManager.Instance.IsInHome(movement.GetLastPosition());
    }
    
    public void freeze()
    {
        movement.freeze();
    }
    
}
