using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableExit : InteractableBase
{
    public override void InteractedWith()
    {
        Debug.Log("The player continues to the next floor.");
    }
}
