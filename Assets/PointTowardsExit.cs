using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointTowardsExit : MonoBehaviour
{
	public GameObject exitTile = default;
    // Start is called before the first frame update
    void OnEnable()
    {
		LookForExitTile();
	}

    // Update is called once per frame
    void Update()
    {
		if (exitTile != null )
			transform.LookAt(exitTile.transform.position);
		else LookForExitTile();
    }

	void LookForExitTile()
	{
		TileBase[] tiles = FindObjectsOfType<TileBase>();
		foreach (TileBase tile in tiles) if (tile.name.Contains("Exit")) exitTile = tile.gameObject;
		if (exitTile == null)
		{
			CharacterBaseController[] characters = FindObjectsOfType<CharacterBaseController>();
			foreach (CharacterBaseController character in characters) if (character.name.Contains("Mino")) exitTile = character.gameObject;
		}
		if (exitTile == null) gameObject.SetActive(false);
	}

	public void ClearTile()
	{
		exitTile = null;
	}
}
