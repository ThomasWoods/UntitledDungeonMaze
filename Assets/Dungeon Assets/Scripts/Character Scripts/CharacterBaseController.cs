using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBaseController : MonoBehaviour
{
    public CharacterMovementController m_MovementController;
    public CharacterInputBase m_Input;

    public enum characterStatus { setup, idle, selectingMovement, moving, turning, hasMoved, defeated }
    public characterStatus currentCharacterStatus;

    private void Awake()
    {
        m_MovementController = GetComponent<CharacterMovementController>();
        m_Input = GetComponent<CharacterInputBase>();
    }

    // Update is called once per frame
    void Update()
    {
        CharacterStateMachine();
    }

    void CharacterStateMachine()
    {
        switch (currentCharacterStatus)
        {
            case characterStatus.setup:
                // When the character has been initialized, switch.
                SwitchCharacterStatus(characterStatus.idle);
                break;

            case characterStatus.idle:
				// When it's the character's turn, m_DungeonBaseControl switches the character's status to selectingMovement.
				SwitchCharacterStatus(characterStatus.selectingMovement);
				break;

            case characterStatus.selectingMovement:

                CheckCharacterMovementInput();
                break;

            case characterStatus.moving:
                // When the character has moved, switch
                m_MovementController.UpdateMovement();
                break;
            case characterStatus.turning:
                m_MovementController.UpdateTurn();
                break;

            case characterStatus.hasMoved:
                DungeonBaseController.instance.ClearActiveObject();
                SwitchCharacterStatus(characterStatus.idle);
                break;

            case characterStatus.defeated:
                break;
        }
    }

    public void SwitchCharacterStatus(characterStatus newStatus)
    {
        currentCharacterStatus = newStatus;
    }

    void CheckCharacterMovementInput()
    {
        int[] inputDir = m_Input.CheckMovementInput();
        bool interacting = m_Input.CheckInteractionInput();

        if (inputDir[0] == 2)
        {
            // The character stands still for the turn
            SwitchCharacterStatus(characterStatus.hasMoved);
        }
        else if (inputDir[0] == 1)
        {
            // The character moves forward
            m_MovementController.MoveForward();
        }
        else if (inputDir[1] != 0)
        {
            // The character turns to the side
            m_MovementController.Turn(inputDir[1]);
        }
    }

    public void Activate()
    {
        SwitchCharacterStatus(characterStatus.selectingMovement);
    }
}
