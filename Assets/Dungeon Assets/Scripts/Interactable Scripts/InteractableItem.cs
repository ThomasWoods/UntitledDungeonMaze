using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItem : InteractableBase
{
	public override void InteractedWith()
	{
		DungeonBaseController.instance.getItem(name);
		gameObject.SetActive(false);
	}
}
