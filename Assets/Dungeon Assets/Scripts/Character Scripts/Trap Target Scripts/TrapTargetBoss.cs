using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTargetBoss : TrapTargetEnemy
{
    public override void KnockedOut()
    {
        Debug.Log("The Minobus has been defeated. Victory!");
    }
}
