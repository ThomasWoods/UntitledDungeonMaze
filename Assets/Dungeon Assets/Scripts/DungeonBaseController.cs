using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DungeonBaseController : MonoBehaviour
{
    public static DungeonBaseController instance;

    public GameObject m_Player;
    public CharacterBaseController m_PlayerController;
    public List<CharacterBaseController> allCharacters;
    public GameObject entranceTile;
    public GameObject exitTile;
    
    public DungeonGenerator m_DungeonGenerator;
    public FloorSweeper m_FloorSweeper;

    public enum dungeonTurnState { SettingUpDungeon, TurnStart, ProcessTurns, TurnEnd }
    public dungeonTurnState currentDungeonTurnState= dungeonTurnState.SettingUpDungeon;
    public dungeonTurnState lastTurnState;
	
	public UnityEvent OnNewTurn = new UnityEvent();
	public UnityEvent OnEndTurn = new UnityEvent();

    public int floorNumber;
    public int dungeonTurn;
    
    public CharacterBaseController activeCharacter;
	Stack<CharacterBaseController> ActionQueue = new Stack<CharacterBaseController>(); 

    int growthOdds;

    private void Awake()
    {
        instance = this;
        
        m_DungeonGenerator = GetComponentInChildren<DungeonGenerator>();
        m_FloorSweeper = GetComponentInChildren<FloorSweeper>();
        m_Player = GameObject.FindWithTag("Player");
		m_PlayerController = m_Player.GetComponent<CharacterBaseController>();
    }

    private void Start()
    {
        floorNumber = 1;
        m_DungeonGenerator.floorWidth = DungeonManager.instance.dungeonCard.startingWidth;
        m_DungeonGenerator.floorDepth = DungeonManager.instance.dungeonCard.StartingDepth;

        m_DungeonGenerator.CacheDungeonCard();
		StartCoroutine(BuildDungeonFloor());
	}

	void Update()
	{
		DungeonStateLogic();
	}

	void DungeonStateLogic()
	{
		switch (currentDungeonTurnState)
		{
			case dungeonTurnState.SettingUpDungeon: break;
			case dungeonTurnState.TurnStart:
				OnNewTurn.Invoke();
				ActionQueue = new Stack<CharacterBaseController>(allCharacters);
				SwitchDungeonTurnState(dungeonTurnState.ProcessTurns);
				break;
			case dungeonTurnState.ProcessTurns:
				if (activeCharacter == null || activeCharacter.currentCharacterStatus == CharacterStatus.idle)
					ActivateNextCharacter();
				break;
			case dungeonTurnState.TurnEnd:
				OnEndTurn.Invoke();
				SwitchDungeonTurnState(dungeonTurnState.TurnStart);
				break;
		}
	}

	void ActivateNextCharacter()
	{
		if (ActionQueue.Count > 0)
		{
			activeCharacter = ActionQueue.Pop();
			activeCharacter.Activate();
		}
		else
		{
			activeCharacter = null;
			SwitchDungeonTurnState(dungeonTurnState.TurnEnd);
		}
	}

    void PlacePlayer()
    {
        m_Player.transform.position = entranceTile.transform.position;
        m_Player.GetComponent<CharacterMovementController>().OccupyTile();
    }
    
    public void SwitchDungeonTurnState(dungeonTurnState newState)
    {
		lastTurnState = currentDungeonTurnState;
        currentDungeonTurnState = newState;
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
			
			StartCoroutine(BuildDungeonFloor());
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

    IEnumerator BuildDungeonFloor()
    {
        m_DungeonGenerator.ResetFloorGenerator();
        m_DungeonGenerator.GenerateDungeon();
		yield return 0;
		PlacePlayer();

        DungeonManager.instance.SwitchDungeonGameState(DungeonManager.dungeonGameState.dungeonExploring);
		SwitchDungeonTurnState(dungeonTurnState.TurnStart);
    }
	
}
