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
		//CharacterAction.TurnLeft,
		//CharacterAction.TurnRight,
		CharacterAction.Wait,
	};
	public override CharacterAction CheckMovementInput()
	{
		CharacterAction nextAction = AI_Actions[Random.Range(0, AI_Actions.Length - 1)];
		
		return nextAction;
	}

	public override bool CheckInteractionInput()
	{
		return false;
	}
}
