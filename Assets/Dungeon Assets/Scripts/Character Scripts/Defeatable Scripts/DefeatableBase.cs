using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatableBase : MonoBehaviour
{
    public virtual void Defeated()
    {
        Debug.Log(gameObject.name + " was defeated!");
    }
}
