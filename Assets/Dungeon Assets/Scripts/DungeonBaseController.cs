using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DungeonBaseController : MonoBehaviour
{
	public static DungeonBaseController instance;

	public GameObject m_Player;
	public CharacterBaseController m_PlayerController;
	public AttackablePlayer m_AttackablePlayer;
	public List<CharacterBaseController> allCharacters;

	public List<CharacterBaseController> enemies = new List<CharacterBaseController>();

	public GameObject entranceTile;
	public GameObject exitTile;

	public DungeonGenerator m_DungeonGenerator;
	public FloorSweeper m_FloorSweeper;
	public TurnManager m_TurnManager;
	public Animator m_FadeOutAnimator;

	public enum dungeonTurnState { SettingUpDungeon, TurnStart, ProcessTurns, TurnEnd }
	public dungeonTurnState currentDungeonTurnState = dungeonTurnState.SettingUpDungeon;
	public dungeonTurnState lastTurnState;

	public UnityEvent OnNewTurn = new UnityEvent();
	public UnityEvent OnEndTurn = new UnityEvent();
	public UnityEvent OnNewDungeonFloor = new UnityEvent();

	public int floorNumber;
	public int dungeonTurn;

	public CharacterBaseController activeCharacter;
	Stack<CharacterBaseController> ActionQueue = new Stack<CharacterBaseController>();

	int growthOdds = 2;

	public UnityEvent GetCompass = new UnityEvent();
	public UnityEvent GetSmokeBomb = new UnityEvent();
	public StringEvent SmokeBombsUpdated = new StringEvent();
	public IntEvent PlayerLifeUpdated = new IntEvent();
	public int _smokeBombs = 3;
	public int smokeBombs {
		get {return _smokeBombs;}
		set {_smokeBombs=value; SmokeBombsUpdated.Invoke(_smokeBombs.ToString());}
	}


    public Queue<CharacterBaseController> enemiesToAttack = new Queue<CharacterBaseController>();

    private bool playerIsInvincible = false;

    private void Awake()
    {
        instance = this;

        floorNumber = 1;
        m_DungeonGenerator = GetComponentInChildren<DungeonGenerator>();
        m_FloorSweeper = GetComponentInChildren<FloorSweeper>();
        m_TurnManager = GetComponent<TurnManager>();
    }

    private void Start()
    {
        m_DungeonGenerator.floorWidth = DungeonManager.instance.dungeonCard.startingWidth;
        m_DungeonGenerator.floorDepth = DungeonManager.instance.dungeonCard.StartingDepth;

        m_DungeonGenerator.CacheDungeonCard();
		StartCoroutine(BuildDungeonFloor());
	}

	public void DungeonStateLogic()
	{
        switch (currentDungeonTurnState)
        {
            case dungeonTurnState.SettingUpDungeon: break;

            case dungeonTurnState.TurnStart:
                m_TurnManager.StartNewTurn();
                SwitchDungeonTurnState(dungeonTurnState.ProcessTurns);
                break;

            case dungeonTurnState.ProcessTurns:
                m_TurnManager.TurnManagerUpdate();
                break;
        }
    }

    public void TurnStart()
    {
        //Called from the turn manager
        enemiesToAttack.Clear();
        OnNewTurn.Invoke();
    }

    public void TurnEnd()
    {
        //Called from the turn manager
        if(!playerIsInvincible)
            CheckIfAttacked();

        CheckDestroyedCharacters();
        OnEndTurn.Invoke();
    }

    private void CheckDestroyedCharacters()
    {
        Queue<CharacterBaseController> defeatedCharacters = new Queue<CharacterBaseController>();

        if (m_PlayerController.currentCharacterStatus == CharacterStatus.defeated)
            m_PlayerController.m_DefeatableBase.Defeated();
        else
        {
            foreach (CharacterBaseController character in enemies)
            {
                if (character.currentCharacterStatus == CharacterStatus.defeated)
                    defeatedCharacters.Enqueue(character);
            }

            while (defeatedCharacters.Count > 0)
            {
                CharacterBaseController character = defeatedCharacters.Dequeue();
                character.m_DefeatableBase.Defeated();
            }
        }
    }

    private void CheckIfAttacked()
    {
        while(enemiesToAttack.Count > 0)
        {
            CharacterBaseController enemy = enemiesToAttack.Dequeue();

            m_PlayerController.TakeDamage(enemy.attackEffect);
            instance.m_PlayerController.damageSource = enemy.displayName;
        }

        m_AttackablePlayer.CheckIfAttacked();
    }

    void PlacePlayer()
    {
        Vector3 offset = new Vector3(0, 0, 1);
        m_Player.transform.position = entranceTile.transform.position + offset;
        m_Player.GetComponent<CharacterMovementController>().OccupyTile();
    }
    
    void PlaceEnemies()
    {
        foreach(CharacterBaseController charBC in enemies)
        {
			if(charBC!=null)
				charBC.m_MovementController.OccupyTile();
        }
    }

    public void SwitchDungeonTurnState(dungeonTurnState newState)
    {
		lastTurnState = currentDungeonTurnState;
        currentDungeonTurnState = newState;
    }

	[ContextMenu("Go to next floor")]
    public void ExitReached()
    {
        playerIsInvincible = true;
        StartCoroutine(FloorTransition());
    }

    private IEnumerator FloorTransition()
    {
        m_FadeOutAnimator.SetBool("FadeOut", true);
        yield return new WaitForSeconds(1f);
        BuildNewDungeonFloor();
    }

    public void BuildNewDungeonFloor()
    {
		allCharacters.Clear();
		enemies.Clear();
        if (floorNumber < DungeonManager.instance.dungeonCard.numberOfFloors)
        {
            SwitchDungeonTurnState(dungeonTurnState.SettingUpDungeon);

            m_FloorSweeper.SweepFloor();
            m_TurnManager.ResetTurnManager();
            floorNumber++;

            int widthRoll = Random.Range(0, growthOdds);
            int depthRoll = Random.Range(0, growthOdds);

            if (widthRoll == 0)
                m_DungeonGenerator.floorWidth++;

            if (depthRoll == 0)
                m_DungeonGenerator.floorDepth++;
			
			StartCoroutine(BuildDungeonFloor());
        }
    }

	IEnumerator BuildDungeonFloor()
	{
		m_DungeonGenerator.ResetFloorGenerator();
		m_DungeonGenerator.GenerateDungeon();

		bool allready = true;
		do
		{
			PlacePlayer();
			PlaceEnemies();
			yield return 0;
			allready = true;
			foreach (CharacterBaseController character in allCharacters)
			{
				if (character.m_MovementController.currentTile == null)
				{
					allready = false;
					break;
				}
			}
		}
		while(allready==false);

		OnNewDungeonFloor.Invoke();

        m_FadeOutAnimator.SetBool("FadeOut", false);
		yield return new WaitForSeconds(1f);

        playerIsInvincible = false;

		DungeonManager.instance.SwitchDungeonGameState(DungeonManager.dungeonGameState.dungeonExploring);
        SwitchDungeonTurnState(dungeonTurnState.TurnStart);
    }

	public void getItem(string s)
	{
		Debug.Log("Got "+s);
		if(s.Contains("Compass")) GetCompass.Invoke();
		if(s.Contains("SmokeBomb")) GetSmokeBomb.Invoke();
	}

	public void AddSmokeBomb()
	{
		smokeBombs++;
	}

	public void UseSmokeBomb() {
		if (smokeBombs > 0)
		{
			TileBase tile = FindRandomOpenTile();
			if (tile)
			{
				smokeBombs--;
				m_PlayerController.m_MovementController.currentTile.occupant = null;
				m_Player.transform.position = tile.transform.position;
				m_PlayerController.m_MovementController.OccupyTile();
			}
		}
	}

	public TileBase FindRandomOpenTile()
	{
		TileBase tile = null;
		int safety = 100;
		do
		{
			TileBase[] tiles = FindObjectsOfType<TileBase>();
			tile=tiles[Random.Range(0, tiles.Length)];
			if (!(tile != null && tile.walkable && tile.occupant == null)) tile = null;
			safety--;
		} while (tile == null && safety > 0);
		if (safety == 0) Debug.Log("safety check");
		if (!tile) Debug.Log("No open tile found!");
		return tile;
	}
}
