using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonBaseController : MonoBehaviour
{
    public static DungeonBaseController instance;

    public GameObject m_Player;
    public GameObject entranceTile;
    public GameObject exitTile;
    
    public DungeonGenerator m_DungeonGenerator;
    public FloorSweeper m_FloorSweeper;

    public enum dungeonTurnState { setup, sot, player, neutral, enemy, eot, battle }
    public dungeonTurnState currentDungeonTurnState;
    public dungeonTurnState lastTurnState;

    public int floorNumber;
    public int dungeonTurn;
    
    public List<GameObject> hasNotActed = new List<GameObject>();
    public GameObject activeCharacter;

    int growthOdds;

    private void Awake()
    {
        instance = this;
        
        m_DungeonGenerator = GetComponentInChildren<DungeonGenerator>();
        m_FloorSweeper = GetComponentInChildren<FloorSweeper>();
        m_Player = GameObject.FindWithTag("Player");
    }

    private void Start()
    {
        floorNumber = 1;
        m_DungeonGenerator.floorWidth = DungeonManager.instance.dungeonCard.startingWidth;
        m_DungeonGenerator.floorDepth = DungeonManager.instance.dungeonCard.StartingDepth;

        m_DungeonGenerator.CacheDungeonCard();
        BuildDungeonFloor();
    }

    void PlacePlayer()
    {
        Vector3 posOffset = new Vector3(0, 0, 1);
        m_Player.transform.position = entranceTile.transform.position;
        //m_Player.transform.position = entranceTile.transform.position + posOffset;

        
        Debug.Log("Check1");
        m_Player.GetComponent<CharacterMovementController>().OccupyTile();
        Debug.Log("Check2");
        
    }
    
    public void SwitchDungeonTurnState(dungeonTurnState newState)
    {
        currentDungeonTurnState = newState;
    }

    public void ClearActiveObject()
    {
        activeCharacter = null;
    }

    public void BuildNewDungeonFloor()
    {
        if (floorNumber < DungeonManager.instance.dungeonCard.numberOfFloors)
        {
            m_FloorSweeper.SweepFloor();
            floorNumber++;

            int widthRoll = Random.Range(0, growthOdds);
            int depthRoll = Random.Range(0, growthOdds);

            if (widthRoll == 0)
            {
                m_DungeonGenerator.floorWidth++;
            }

            if (depthRoll == 0)
            {
                m_DungeonGenerator.floorDepth++;
            }

            BuildDungeonFloor();
        }
        else
        {
            // The bottom floor has been reached
            // TODO:
            // If dungeon boss hasn't been defeated, go to boss battle
            // else leave dungeon

            // Temporary code
            Debug.Log("You have reached the end of the dungeon.");
        }
    }

    void BuildDungeonFloor()
    {
        m_DungeonGenerator.ResetFloorGenerator();
        m_DungeonGenerator.GenerateDungeon();
        PlacePlayer();

        DungeonManager.instance.SwitchDungeonGameState(DungeonManager.dungeonGameState.dungeonExploring);
    }
}
