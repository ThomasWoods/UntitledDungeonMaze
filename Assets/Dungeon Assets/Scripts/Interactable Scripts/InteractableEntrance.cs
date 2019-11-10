using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableEntrance : InteractableBase
{
    public override void InteractedWith()
    {
        Debug.Log("The player leaves the dungeon.");
    }
}
