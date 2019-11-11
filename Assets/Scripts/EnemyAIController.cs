using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Pathfinder))]
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
	Pathfinder pathfinder;

	protected override void Awake()
	{
		base.Awake();
		pathfinder = GetComponent<Pathfinder>();
	}

	public override CharacterAction CheckMovementInput()
	{
		//CharacterAction nextAction = AI_Actions[Random.Range(0, AI_Actions.Length - 1)];
		//return nextAction;
		if (DungeonBaseController.instance.m_PlayerController != null &&
			DungeonBaseController.instance.m_PlayerController.m_MovementController != null &&
			DungeonBaseController.instance.m_PlayerController.m_MovementController.currentTile != null &&
			m_CharacterBaseController.m_MovementController.currentTile != null)
		{
			TileBase myTile = m_CharacterBaseController.m_MovementController.currentTile;
			TileBase playerTile = DungeonBaseController.instance.m_PlayerController.m_MovementController.currentTile;
			pathfinder.FindPath( myTile.transform.position, playerTile.transform.position);
			if (pathfinder.ShortestPathReversed != null && pathfinder.ShortestPathReversed.Count > 0)
			{
				TileBase nextTile = pathfinder.ShortestPathReversed.Peek().Tile;
				//Debug.Log("transform.position + transform.forward == nextTile.transform.position:" + (transform.position + transform.forward) + " == "+ nextTile.transform.position);
				if (transform.position + transform.forward == nextTile.transform.position)
				{
					TileBase tile = m_CharacterBaseController.m_MovementController.CheckTile(transform.forward);
					if (tile.walkable == true && tile.occupant == null)
						return CharacterAction.MoveForward;
					else
					{
						Debug.Log("Enemy cannot move forward towards player, waiting");
						return CharacterAction.Wait;
					}
				}
				else if (transform.position + transform.right == nextTile.transform.position)
				{
					m_CharacterBaseController.ActivationIsDone();
					return CharacterAction.TurnRight;
				}
				else
				{
					m_CharacterBaseController.ActivationIsDone();
					return CharacterAction.TurnLeft;
				}
			}
			else return SimpleMovement();
		}
		else return SimpleMovement();
	}

	CharacterAction SimpleMovement()
	{
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
