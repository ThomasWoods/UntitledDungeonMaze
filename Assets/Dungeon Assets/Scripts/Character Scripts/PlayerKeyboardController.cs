using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActionKeyMap
{
	[Tooltip("Array Index to Action\n[0]: NoAction, [1]:Forward, [2]:Backstep, [3]:MoveLeft, [4]:MoveRight, [5]:TurnLeft, [6]:TurnRight, [7]:Wait, [8]:Interact")]
	public KeyCode[] KeyMap = new KeyCode[0];
}
public class PlayerKeyboardController : CharacterInputBase
{
	public List<ActionKeyMap> ActionKeys = new List<ActionKeyMap>();

    public override CharacterAction CheckMovementInput()
    {
		CharacterAction nextAction = CharacterAction.NoAction;

		foreach (ActionKeyMap Map in ActionKeys)
		{
			for (int i = 0; i < System.Enum.GetNames(typeof(CharacterAction)).Length; i++)
			{
				if (Map.KeyMap.Length <= i) continue;
				if (Input.GetKey(Map.KeyMap[i]))
					nextAction = (CharacterAction)i;
			}
		}
        return nextAction;
    }

    public override bool CheckInteractionInput()
	{
		foreach (ActionKeyMap Map in ActionKeys)
		{
			int i = (int)CharacterAction.Interact;
			if (Map.KeyMap.Length <= i) continue;
			if (Input.GetKey(Map.KeyMap[i])) return true;
		}
		return false;
	}
}
