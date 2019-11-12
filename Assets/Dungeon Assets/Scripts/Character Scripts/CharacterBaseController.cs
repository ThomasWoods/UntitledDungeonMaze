using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum CharacterStatus { setup, idle, selectingMovement, moving, turning, defeated }
public enum CharacterAction { NoAction, MoveForward, Backstep, MoveLeft, MoveRight, TurnLeft, TurnRight, Wait, Interact}
public class CharacterBaseController : MonoBehaviour
{
	public int life = 3;

    public CharacterMovementController m_MovementController;
    public CharacterInputBase m_Input;
    public GraphicsController m_GraphicsController;
    public DefeatableBase m_DefeatableBase;

    public CharacterStatus currentCharacterStatus=CharacterStatus.setup;
	public CharacterAction LastAction;

	public CharacterStatusEvent OnStatusChange = new CharacterStatusEvent();
	public UnityEvent OnDefeated = new UnityEvent();
	public UnityEvent OnHit = new UnityEvent();
	public UnityEvent OnMirrorVision = new UnityEvent();

	public bool UseStartPosition = false;
	public bool RandomStartPosition = false;
	public Vector3 StartPosition;

	public bool hasBeenDefeated = false;
	public bool hasBeenHit = false;
	public string damageSource;

	private void Awake()
    {
        m_MovementController = GetComponent<CharacterMovementController>();
        m_Input = GetComponent<CharacterInputBase>();
        m_GraphicsController = GetComponent<GraphicsController>();
        m_DefeatableBase = GetComponent<DefeatableBase>();

	}

	IEnumerator Start()
	{
		DungeonBaseController.instance.OnNewTurn.AddListener(OnNewTurn);
		DungeonBaseController.instance.OnEndTurn.AddListener(OnEndTurn);
        
		if(!DungeonBaseController.instance.allCharacters.Contains(this))
			DungeonBaseController.instance.allCharacters.Add(this);

        switch (gameObject.tag)
        {
            case "Player":
                DungeonBaseController.instance.m_Player = gameObject;
                DungeonBaseController.instance.m_PlayerController = this;
                break;

            case "Enemy":
                DungeonBaseController.instance.enemies.Add(this);
                break;
        }

		if(UseStartPosition)
		{
			yield return 0;
			TileBase newTile = m_MovementController.FindNearestOpenTile(RandomStartPosition ? new Vector3(0,0,0) : StartPosition);
			transform.position = newTile.transform.position;
			m_MovementController.OccupyTile();
		}
		SwitchCharacterStatus(CharacterStatus.idle);
	}

    // Update is called once per frame
    void Update()
    {
		if (Game.instance.isPaused) return;
        CharacterStateMachine();
    }

    void CharacterStateMachine()
    {
        switch (currentCharacterStatus)
        {
            case CharacterStatus.setup:
                // When the character has been initialized, switch.
                break;

            case CharacterStatus.idle:
				// When it's the character's turn, m_DungeonBaseControl switches the character's status to selectingMovement.
				break;

            case CharacterStatus.selectingMovement:
                CheckCharacterMovementInput();
				break;

            case CharacterStatus.moving:
                // When the character has moved, switch
                m_MovementController.UpdateMovement();
                break;
            case CharacterStatus.turning:
                m_MovementController.UpdateTurn();
                break;

            case CharacterStatus.defeated:
				OnDefeated.Invoke();
                break;
        }
    }

    public void SwitchCharacterStatus(CharacterStatus newStatus)
    {
		if (currentCharacterStatus == CharacterStatus.defeated) return;
        currentCharacterStatus = newStatus;
		OnStatusChange.Invoke(currentCharacterStatus);
    }

    void CheckCharacterMovementInput()
    {
		CharacterAction nextAction = m_Input.CheckMovementInput();
        bool interacting = m_Input.CheckInteractionInput();

		switch (nextAction)
		{
			case CharacterAction.MoveForward:
				m_MovementController.Move(transform.forward);
				break;
			case CharacterAction.Backstep:
				m_MovementController.Move(transform.forward * -1);
				break;
			case CharacterAction.MoveLeft:
				m_MovementController.Move(transform.right * -1);
				break;
			case CharacterAction.MoveRight:
				m_MovementController.Move(transform.right);
				break;
			case CharacterAction.TurnLeft:
				m_MovementController.Turn(-1);
				break;
			case CharacterAction.TurnRight:
				m_MovementController.Turn(1);
				break;
			case CharacterAction.Wait:
                SwitchCharacterStatus(CharacterStatus.idle);
                ActivationIsDone();
				break;
		}

        if (interacting)
        {
            TileBase tile = m_MovementController.CheckTile(transform.forward);

            InteractableBase interactable = tile.GetComponent<InteractableBase>();

            if(interactable != null)
            {
                interactable.InteractedWith();
                SwitchCharacterStatus(CharacterStatus.idle);

                if(DungeonBaseController.instance.currentDungeonTurnState != DungeonBaseController.dungeonTurnState.SettingUpDungeon)
                    ActivationIsDone();
            } 
        }


		if (nextAction != CharacterAction.NoAction)
			LastAction = nextAction;
	}

    public void Activate()
    {
        SwitchCharacterStatus(CharacterStatus.selectingMovement);
    }

    public void ActivationIsDone()
    {
        DungeonBaseController.instance.m_TurnManager.ActionIsDone();
    }

    public void SetWalking()
    {
        if (m_GraphicsController != null)
            m_GraphicsController.StartWalking();

        SwitchCharacterStatus(CharacterStatus.moving);
        
        ActivationIsDone();
    }

    public void DoneWalking()
    {
        if (m_GraphicsController != null)
            m_GraphicsController.StopWalking();

        SwitchCharacterStatus(CharacterStatus.idle);
    }

	virtual protected void OnNewTurn() { }
	virtual protected void OnEndTurn() { }

	public void TakeDamage(string source, int damage=1)
	{
		life-=damage;
		damageSource = source;
		if (life > 0) hasBeenHit = true;
		else
		{
			hasBeenDefeated = true;
		}
	}

	public void ProcessLastTurnEvents()
	{
		if (hasBeenDefeated)
		{
			currentCharacterStatus = CharacterStatus.defeated;
			m_DefeatableBase.Defeated();
		}
		else if (hasBeenHit)
		{
			if (damageSource.Contains("Medusa")) OnMirrorVision.Invoke();
			hasBeenHit = false;
			OnHit.Invoke();
		}
	}
	
}
