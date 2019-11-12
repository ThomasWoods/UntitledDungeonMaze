using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatableBoss : DefeatableEnemy
{
    public override void Defeated()
    {
        DungeonManager.instance.Victory();
        RemoveCharacter();
    }
}
