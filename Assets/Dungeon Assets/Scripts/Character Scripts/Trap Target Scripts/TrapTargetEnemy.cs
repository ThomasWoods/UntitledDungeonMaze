using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTargetEnemy : TrapTargetBase
{
    public int baseHp;

    private int currentHp;

    private void Start()
    {
        currentHp = baseHp;
    }

    public override void OnTrapped(int trapStr)
    {
        currentHp -= trapStr;

        if (currentHp <= 0)
            KnockedOut();
    }

    public virtual void KnockedOut()
    {
        Debug.Log(gameObject.name + " was knocked out!");
    }
}
