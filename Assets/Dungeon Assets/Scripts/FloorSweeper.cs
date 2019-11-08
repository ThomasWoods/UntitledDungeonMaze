using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSweeper : MonoBehaviour
{
    public void SweepFloor()
    {
        foreach (Transform child in gameObject.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}
