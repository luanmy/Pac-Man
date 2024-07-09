using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public enum GhostColor
{
    Red,
    Pink,
    Blue,
    Orange
}
public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    private Transform player;
    private Movement playerMovement;
    private PacMan pacMan;
    private Tilemap tilemap;
    private TileBase[,] tileMap2dArray;
    private int horizontalLength;
    private int verticalLength;
    private Dictionary<GhostColor, Ghost> ghostDictionary = new Dictionary<GhostColor, Ghost>();
    private int foodInTotal = 0;
    private int foodEaten = 0;
    private Vector2 homeSlot = new Vector2(0, -1);
    private UI ui;
    public int mapXStart = 2;
    public int mapXEnd = 19;
    public int mapYStart = 2;
    public int mapYEnd = 28;
    public Vector2 origin = new Vector2(-8, -14);
    public Ghost ghostRed;
    public Ghost ghostPink;
    public Ghost ghostBlue;
    public Ghost ghostOrange;
    public int playerLife = 3;
    public float[] homeMapX = { -1, 1, 0, 0 };
    public float[] homeMapY = { -2, -2, -2, -1};
    public UnityEvent scoreChangedEvent;
    public UnityEvent lifeChangedEvent;
    public AudioClip[] audioClips;
    public AudioSource audioSource;
    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            Time.timeScale = 1;
            instance = this;
            player = GameObject.Find("PacMan").GetComponent<Transform>();
            playerMovement = player.GetComponent<Movement>();
            pacMan = player.GetComponent<PacMan>();
            ui = GameObject.Find("UI").GetComponent<UI>();
            audioSource = GetComponent<AudioSource>();
            audioSource.loop = false;
            InitisalizeMap();
            Phase1();
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    private void Start()
    {
        audioSource.clip = audioClips[0];
        audioSource.Play();
    }

    private void Update()
    {
        if(audioSource.isPlaying == false)
        {
            audioSource.clip = audioClips[1];
            audioSource.Play();
        }
    }

    private void Phase1()
    {
        ghostDictionary.Add(GhostColor.Red, ghostRed);
        ghostDictionary.Add(GhostColor.Pink, ghostPink);
        ghostDictionary.Add(GhostColor.Blue, ghostBlue);
        ghostDictionary.Add(GhostColor.Orange, ghostOrange);
        
        ghostDictionary[GhostColor.Red].Initialize(GhostState.Scatter);
        ghostDictionary[GhostColor.Pink].Initialize(GhostState.StayHome);
        ghostDictionary[GhostColor.Blue].Initialize(GhostState.StayHome);
        ghostDictionary[GhostColor.Orange].Initialize(GhostState.StayHome);

        Invoke(nameof(Phase2), 3f);
    }

    private void Phase2()
    {
        Scatter();
    }

    private void Scatter()
    {
        for(int i = 0; i < 4; i++)
        {
            ghostDictionary[(GhostColor)i].SetState(GhostState.Scatter);
        }
    }
    
    public Transform GetPacMan()
    {
        return player;
    }
    void InitisalizeMap()
    {
        tilemap = GameObject.Find("Grid/Walls").GetComponent<Tilemap>();
        BoundsInt boundsInt = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(boundsInt);
        horizontalLength = mapXEnd - mapXStart;
        verticalLength = mapYEnd - mapYStart;
        tileMap2dArray = new TileBase[verticalLength, horizontalLength];
        
        for (int i = 0; i < verticalLength; i++)
        {
            for(int j = 0; j < horizontalLength; j++)
            {
                tileMap2dArray[i,j] = allTiles[(i + mapXStart) * boundsInt.size.x + j + mapYStart];
            }
        }
        Tilemap foodMap = GameObject.Find("Grid/Pellets").GetComponent<Tilemap>();
        boundsInt = foodMap.cellBounds;
        allTiles = foodMap.GetTilesBlock(boundsInt);
        
        for(int i = 0; i < allTiles.Length; i++)
        {
            if (allTiles[i] != null)
            {
                foodInTotal++;
            }
        }
    }
    
    public bool isBlockHasWall(int y, int x)
    {
        return tileMap2dArray[y-(int)origin.y, x-(int)origin.x] != null;
    }
    
    public bool isBlockInMap(int y, int x)
    {
        return y >= origin.y && y < (origin.y + verticalLength) && x >= origin.x && x < (origin.x + horizontalLength);
    }
    
    public void AddScore()
    {
        foodEaten++;
        scoreChangedEvent.Invoke();
        if(foodEaten >= foodInTotal)
        {
            StartCoroutine(WinGame());
            GameOver();
        }
    }

    public IEnumerator WinGame()
    {
        ui.ShowWin(true);
        Freeze();
        yield return new WaitForSeconds(1f);
        GameOver();
    }

    public IEnumerator LoseGame()
    {
        ui.ShowLose(true);
        Freeze();
        yield return new WaitForSeconds(1f);
        GameOver();
    }

    private void Freeze()
    {
        playerMovement.freeze();
        for(int i = 0; i < 4; i++)
        {
            ghostDictionary[(GhostColor)i].freeze();
        }
    }

    public int GetScore()
    {
        return foodEaten;
    }
    
    public int GetScoreTotal()
    {
        return foodInTotal;
    }


    public void SuperPower()
    {
        for(int i = 0; i < 4; i++)
        {
            ghostDictionary[(GhostColor)i].SetState(GhostState.Frightened);
        }
    }

    public void PlayerLoseLife()
    {
        playerLife--;
        lifeChangedEvent.Invoke();
        if (playerLife <= 0)
        {
            StartCoroutine(LoseGame());
        }
    }
    
    public int GetLife()
    {
        return playerLife;
    }

    public void GameOver()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
    
    public void SetGhostState(Ghost ghost, GhostState state)
    {
        ghost.SetState(state);
    }
    
    public bool IsHomeSlot(int x, int y)
    {
        return x == homeSlot.x && y == homeSlot.y;
    }
    
    public bool IsInHome(Vector2 position)
    {
        for(int i = 0; i < 4; i++)
        {
            if(position.x == homeMapX[i] && position.y == homeMapY[i])
            {
                return true;
            }
        }
        return false;
    }
    
    public void SetPlayerDirection(Vector2 direction)
    {
        pacMan.SetDirection(direction);
        
    }

    void OnDestroy()
    {
        CancelInvoke();
    }
    
    
}
