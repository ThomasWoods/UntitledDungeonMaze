using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInputBase : MonoBehaviour
{
    public virtual int[] CheckMovementInput()
    {
        int[] backupDir = new int[2];
        Debug.LogWarning(gameObject.name + " doesn't override the base movement input method!");
        return backupDir;
    }

    public virtual bool CheckInteractionInput()
    {
        Debug.LogWarning(gameObject.name + " doesn't override the base interaction input method!");
        return false;
    }
}
