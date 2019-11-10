using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBase : MonoBehaviour
{
    public virtual void InteractedWith()
    {
        Debug.Log(gameObject.name + " was interacted with.");
    }
}
