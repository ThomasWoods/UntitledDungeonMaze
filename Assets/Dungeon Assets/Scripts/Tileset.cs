using UnityEngine;

[CreateAssetMenu(fileName = "newTileset", menuName = "Scriptable Objects/Tileset")]
public class Tileset : ScriptableObject
{
    public GameObject[] openTiles;
    public GameObject[] lineTiles;
    public GameObject[] ellTiles;
    public GameObject[] endTiles;
    public GameObject[] crossTiles;
}
