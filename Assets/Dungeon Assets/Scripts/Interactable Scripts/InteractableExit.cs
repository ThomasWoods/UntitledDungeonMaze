using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableExit : InteractableBase
{
    public override void InteractedWith()
    {
        DungeonBaseController.instance.BuildNewDungeonFloor();
    }
}
