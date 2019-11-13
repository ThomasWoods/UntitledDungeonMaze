using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInputBase : MonoBehaviour
{
    public CharacterBaseController m_CharacterBaseController;

	virtual protected void Awake()
	{
		m_CharacterBaseController = GetComponent<CharacterBaseController>();
	}

	public virtual CharacterAction CheckMovementInput()
    {
		CharacterAction backupDir = CharacterAction.Wait;
        Debug.LogWarning(gameObject.name + " doesn't override the base movement input method!");
        return backupDir;
    }

    public virtual bool CheckInteractionInput()
    {
        Debug.LogWarning(gameObject.name + " doesn't override the base interaction input method!");
        return false;
    }

    public CharacterAction GetRandomAction()
    {
        List<CharacterAction> possibleActions = new List<CharacterAction>();

        possibleActions.Add(CharacterAction.TurnLeft);
        possibleActions.Add(CharacterAction.TurnRight);

        TileBase tile = m_CharacterBaseController.m_MovementController.CheckTile(transform.forward);

        if(tile.walkable && tile.occupant == null)
            possibleActions.Add(CharacterAction.MoveForward);

        CharacterAction action = possibleActions[Random.Range(0, possibleActions.Count)];

        if (gameObject.tag == "Player" && action != CharacterAction.MoveForward)
            DungeonBaseController.instance.m_TurnManager.UseLongTime();
            
        m_CharacterBaseController.ActivationIsDone();
        return action;
    }
}
