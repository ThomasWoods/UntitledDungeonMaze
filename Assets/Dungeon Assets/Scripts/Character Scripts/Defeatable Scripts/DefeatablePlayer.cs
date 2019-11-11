using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatablePlayer : DefeatableBase
{
    public override void Defeated()
    {
        Debug.Log("The player was defeated");

        DungeonManager.instance.GameOver();
    }
}
