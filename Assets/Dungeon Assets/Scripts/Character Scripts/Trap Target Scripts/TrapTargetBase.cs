using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTargetBase : MonoBehaviour
{
    public virtual void OnTrapped(int trapStr)
    {
        Debug.Log(gameObject.name + " stepped on a trap!");
    }
}
