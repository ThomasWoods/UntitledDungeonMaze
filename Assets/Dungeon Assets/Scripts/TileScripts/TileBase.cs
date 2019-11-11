using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBase : MonoBehaviour
{
    public enum TileType { open, wall, trap, entrance, exit}
    public TileType type;

    public bool walkable;
    public GameObject occupant;



    public virtual void TileSteppedOn()
    {

    }
}
