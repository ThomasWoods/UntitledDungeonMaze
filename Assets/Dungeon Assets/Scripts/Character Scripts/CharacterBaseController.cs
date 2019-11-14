using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum CharacterStatus { setup, idle, selectingMovement, moving, turning, defeated }
public enum CharacterAction { NoAction, MoveForward, Backstep, MoveLeft, MoveRight, TurnLeft, TurnRight, Wait, Interact}
public class CharacterBaseController : MonoBehaviour
{
    public string displayName;

	public int _life = 3;
	public int life
	{
		get { return _life; }
		set { _life = value;
			if (tag == "Player") DungeonBaseController.instance.PlayerLifeUpdated.Invoke(life.ToString()); }
	}
	public int turn = 0;
    
    public CharacterMovementController m_MovementController;
    public CharacterInputBase m_Input;
    public GraphicsController m_GraphicsController;
    public DefeatableBase m_DefeatableBase;

    public CharacterStatus currentCharacterStatus=CharacterStatus.setup;
	public CharacterAction LastAction;

	public CharacterStatusEvent OnStatusChange = new CharacterStatusEvent();
	public StringEvent OnDefeated = new StringEvent();
	public StringEvent OnHit = new StringEvent();

	public bool UseStartPosition = false;
	public bool RandomStartPosition = false;
	public Vector3 StartPosition;

    public StaticEnums.StatusEffect attackEffect;
	public bool hasBeenDefeated = false;
	public bool hasBeenHit = false;
	public string damageSource;

	public bool stunned = false;
	public int stunTime = 0;
	public UnityEvent OnStunned = new UnityEvent();
	public UnityEvent OnStunEnd = new UnityEvent();

    public bool confused = false;
    public int confusedTime = 0;
    public UnityEvent OnConfused = new UnityEvent();

    public bool mirror;

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
                break;
        }
    }

    public void SwitchCharacterStatus(CharacterStatus newStatus)
    {
		if (currentCharacterStatus == CharacterStatus.defeated) return;
        currentCharacterStatus = newStatus;
    }

    void CheckCharacterMovementInput()
    {
        CharacterAction nextAction = CharacterAction.NoAction;
        bool interacting = false;

        if (stunned)
            nextAction = CharacterAction.Wait;
        else if (confused)
            nextAction = m_Input.GetRandomAction();
        else
        {
            nextAction = m_Input.CheckMovementInput();
            interacting = m_Input.CheckInteractionInput();
        }

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

            InteractableBase interactable = tile.GetComponentInChildren<InteractableBase>();

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

        if(currentCharacterStatus != CharacterStatus.defeated)
            SwitchCharacterStatus(CharacterStatus.idle);
    }

	virtual protected void OnNewTurn()
	{
        hasBeenHit = false;

		turn++;

		if (stunned)
		{
			if (stunTime <= 0)
			{
				stunned = false;
				OnStunEnd.Invoke();
			}
			stunTime--;
		}

        if (confused)
        {
            if (confusedTime <= 0)
            {
                confused = false;
            }
            confusedTime--;
        }


    }
	virtual protected void OnEndTurn()
	{
	}

	public void TakeDamage(StaticEnums.StatusEffect effect, int damage=1)
	{
        if(currentCharacterStatus != CharacterStatus.defeated)
        {
            if (!hasBeenHit)
                life -= damage;

            ApplyStatusEffect(damage, effect);

            if (life > 0)
                hasBeenHit = true;
            else
                SwitchCharacterStatus(CharacterStatus.defeated);
        }
	}

    public void ApplyStatusEffect(int effectStr, StaticEnums.StatusEffect effect)
    {
        switch (effect)
        {
            case StaticEnums.StatusEffect.mirror:
                if (gameObject.tag == "Player")
                    mirror = true;
                break;

            case StaticEnums.StatusEffect.stun:
                Stun(effectStr);
                break;

            case StaticEnums.StatusEffect.confusion:
                Confuse(effectStr);
                break;
        }
    }
    
	public void Stun(int turns = 1)
	{
		OnStunned.Invoke();
		stunned = true;
		stunTime = turns;
	}

    public void Confuse(int turns = 1)
    {
        OnConfused.Invoke();
        confused = true;
        confusedTime = turns;
    }
}
