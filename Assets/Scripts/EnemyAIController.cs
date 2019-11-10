using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIController : CharacterInputBase
{

	public CharacterAction[] AI_Actions = new CharacterAction[] {
		CharacterAction.MoveForward,
		CharacterAction.Backstep,
		CharacterAction.MoveLeft,
		CharacterAction.MoveRight,
		CharacterAction.TurnLeft,
		CharacterAction.TurnRight,
		CharacterAction.Wait,
	};
	public override CharacterAction CheckMovementInput()
	{
        //CharacterAction nextAction = AI_Actions[Random.Range(0, AI_Actions.Length - 1)];
        //return nextAction;

        TileBase tile = m_CharacterBaseController.m_MovementController.CheckTile(transform.forward);

        if (tile.walkable == true && tile.occupant == null)
            return CharacterAction.MoveForward;
        else
        {
            TileBase lTile = m_CharacterBaseController.m_MovementController.CheckTile(transform.right * -1);

            m_CharacterBaseController.ActivationIsDone();

            if (lTile.walkable == true && lTile.occupant == null)
                return CharacterAction.TurnLeft;
            else
                return CharacterAction.TurnRight;
        }
	}

	public override bool CheckInteractionInput()
	{
		return false;
	}
}
