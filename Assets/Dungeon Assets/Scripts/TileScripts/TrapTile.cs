using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTile : TileBase
{
    public StaticEnums.StatusEffect trapEffect;
    public int trapStr;

    public override void TileSteppedOn()
    {
        TrapTargetBase catchable = occupant.GetComponent<TrapTargetBase>();

        if(catchable != null)
        {
            catchable.OnTrapped(trapStr, trapEffect);
        }
    }
}
