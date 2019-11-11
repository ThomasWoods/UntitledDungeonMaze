using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSweeper : MonoBehaviour
{
    public Transform tilesParent;
    public Transform enemyParent;

    public void SweepFloor()
    {
        foreach (Transform child in enemyParent)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in tilesParent)
        {
            Destroy(child.gameObject);
        }
    }
}
