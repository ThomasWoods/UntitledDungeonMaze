using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatableBoss : DefeatableBase
{
    public override void Defeated()
    {
        Debug.Log("The Minotaur has been defeated. Victory!");
    }
}
