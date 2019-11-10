﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTile : TileBase
{
    public override void TileSteppedOn()
    {
        TrapTargetBase catchable = occupant.GetComponent<TrapTargetBase>();

        if(catchable != null)
        {
            catchable.OnTrapped();
        }
    }
}
