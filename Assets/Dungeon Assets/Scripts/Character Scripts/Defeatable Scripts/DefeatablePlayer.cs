using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatablePlayer : DefeatableBase
{
    public override void Defeated()
    {
        DungeonManager.instance.GameOver();
    }
}
