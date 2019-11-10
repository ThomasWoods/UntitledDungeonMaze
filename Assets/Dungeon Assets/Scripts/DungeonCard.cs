using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newDungeoncard", menuName = "Scriptable Objects/Dungeon Card")]
public class DungeonCard : ScriptableObject
{
    public int startingWidth;
    public int StartingDepth;

    public int numberOfFloors;

    public GameObject firstEnemy;
    public GameObject secondEnemy;

    public GameObject bossEnemy;

    public GameObject entranceTile;
    public GameObject exitTile;

    public GameObject[] openTiles;
    public GameObject[] pillarTiles;
    public GameObject[] lineTiles;
    public GameObject[] cornerTiles;
    public GameObject[] tTiles;
    public GameObject[] endTiles;
    public GameObject[] crossTiles;
    public GameObject[] trapTiles;
}
