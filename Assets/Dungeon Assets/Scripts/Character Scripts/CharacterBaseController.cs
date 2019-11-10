using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum CharacterStatus { setup, idle, selectingMovement, moving, turning, hasMoved, defeated }
public enum CharacterAction { NoAction, MoveForward, Backstep, MoveLeft, MoveRight, TurnLeft, TurnRight, Wait, Interact}
public class CharacterBaseController : MonoBehaviour
{
    public CharacterMovementController m_MovementController;
    public CharacterInputBase m_Input;

    public CharacterStatus currentCharacterStatus;
	public CharacterAction LastAction;

	public CharacterStatusEvent OnStatusChange = new CharacterStatusEvent();
	public UnityEvent OnDefeated = new UnityEvent();

    private void Awake()
    {
        m_MovementController = GetComponent<CharacterMovementController>();
        m_Input = GetComponent<CharacterInputBase>();

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
                SwitchCharacterStatus(CharacterStatus.idle);
                break;

            case CharacterStatus.idle:
				// When it's the character's turn, m_DungeonBaseControl switches the character's status to selectingMovement.
				SwitchCharacterStatus(CharacterStatus.selectingMovement);
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

            case CharacterStatus.hasMoved:
                DungeonBaseController.instance.ClearActiveObject();
                SwitchCharacterStatus(CharacterStatus.idle);
                break;

            case CharacterStatus.defeated:
				OnDefeated.Invoke();
                break;
        }
    }

    public void SwitchCharacterStatus(CharacterStatus newStatus)
    {
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
				m_MovementController.Move(transform.forward*-1);
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
				SwitchCharacterStatus(CharacterStatus.hasMoved);
				break;
		}
	}

    public void Activate()
    {
        SwitchCharacterStatus(CharacterStatus.selectingMovement);
    }
}
