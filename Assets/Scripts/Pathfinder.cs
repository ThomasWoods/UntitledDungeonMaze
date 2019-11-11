using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PathPoint
{
	public int x, y;
	public PathPoint(int _x, int _y) { x = _x; y = _y; }
}
[System.Serializable]
public class PathSegment
{
	public bool Exists = false;
	public bool Calculated = false;
	public PathSegment parent { get; set; }
	public PathPoint location;
	//G_Cost: distance from A, H_Cost: distance from B, F_Cost:G_Cost+H_Cost
	public int G_Cost,H_Cost,F_Cost;
	public TileBase Tile;
	public void CalcFCost() { F_Cost = G_Cost + H_Cost; }
	public PathSegment Copy()
	{
		return new PathSegment()
		{
			Exists = Exists, Calculated = Calculated,
			parent = parent, location = location,
			G_Cost = G_Cost, H_Cost = H_Cost, F_Cost = F_Cost,
			Tile = Tile
		};
	}
}
public class Pathfinder : MonoBehaviour
{
	public static PathPoint[] adjacentPaths = new PathPoint[] { new PathPoint(0, 1), new PathPoint(1, 0), new PathPoint(0, -1), new PathPoint(-1, 0) };
	public int SightRadius;
	public Vector3 Destination;
	public PathSegment[,] Area;
	public PathSegment ShortestPath;
	public Stack<PathSegment> ShortestPathReversed;

	public bool ShowGizmos = false;

	void ScopeArea(Vector3 startPos)
	{
		Area = new PathSegment[SightRadius * 2 + 1, SightRadius * 2 + 1];
		for (int x = 0; x < SightRadius*2+1; x++)
		for (int y = 0; y < SightRadius*2+1; y++)
		{
				Collider[] tiles = Physics.OverlapSphere(startPos + new Vector3(x - SightRadius, 0, y - SightRadius), 0.5f);
				if (tiles.Length > 0) foreach(Collider c in tiles){
						TileBase tile = c.GetComponent<TileBase>();
						if (tile != null)
						{
							Area[x, y] = new PathSegment();
							Area[x,y].Exists = true;
							Area[x,y].location = new PathPoint(x,y);
							Area[x,y].Tile = tile;
						}
						break;
				}
		}
	}

	public void FindPath(Vector3 startPos, Vector3 destination)
	{
		if (Vector3.Distance(startPos, destination) > SightRadius)
		{
			//Debug.Log("Destination outside of range");
			return;
		}
		ShortestPath = null;
		ScopeArea(startPos);
		if (!Area[SightRadius, SightRadius].Exists)
		{
			//Debug.Log("No starting location");
			return;
		}
		List<PathSegment> OpenPaths = new List<PathSegment>() { Area[SightRadius, SightRadius] };
		int safetyLimiter = 9999;
		while (OpenPaths.Count > 0 && safetyLimiter>0)
		{
			PathSegment OpenPath = OpenPaths[0];
			foreach (PathPoint adjPath in adjacentPaths)
			{
				PathPoint adjLocation = new PathPoint(OpenPaths[0].location.x + adjPath.x, OpenPaths[0].location.y + adjPath.y);
				if (adjLocation.x >= Area.GetLength(0) || adjLocation.x < 0 || adjLocation.y >= Area.GetLength(1) || adjLocation.y < 0)
					continue;
				PathSegment currentPath = Area[adjLocation.x, adjLocation.y];
				if (currentPath.Exists && currentPath.Tile != null && currentPath.Tile.walkable && 
					(currentPath.Tile.occupant == null || destination == currentPath.Tile.transform.position) &&
					!(currentPath.location.x==0 && currentPath.location.y == 0))
				{
					PathSegment newPath = currentPath.Copy();
					newPath.parent = OpenPath;
					newPath.G_Cost = (int)(Vector3.Distance(currentPath.Tile.transform.position, startPos) * 10);
					newPath.H_Cost = (int)(Vector3.Distance(currentPath.Tile.transform.position, destination) * 10);
					newPath.CalcFCost();
					newPath.Calculated = true;
					if (!currentPath.Calculated || newPath.F_Cost < currentPath.F_Cost)
					{
						currentPath = newPath;
						if (ShortestPath != null && currentPath.F_Cost > ShortestPath.F_Cost)
						{
							//Shortcircuit here because this path is longer than the shorest found path.
							break;
						}
						OpenPaths.Add(currentPath);

						if (currentPath.Tile.transform.position == destination &&
							(ShortestPath == null || currentPath.F_Cost < ShortestPath.F_Cost))
						{
							ShortestPath = currentPath;
						}
					}
				}
			}
			OpenPaths.RemoveAt(0);
			safetyLimiter--;
		}
		if (ShortestPath == null)
		{
			//Debug.Log("Did not find a path");
			return;
		}
		ShortestPathReversed = new Stack<PathSegment>();
		PathSegment Parent=ShortestPath;
		safetyLimiter = 100;
		Debug.Log("Reversing Path");
		while (!(Parent == null || Parent.Calculated == false) && safetyLimiter > 0)
		{
			Debug.Log(Parent.Tile.transform.position + ", "+ (Parent == null || Parent.Calculated == false));
			ShortestPathReversed.Push(Parent);
			Parent = Parent.parent;
			safetyLimiter--;
		}
		if (ShortestPathReversed.Count == 0)
		{
			//Debug.Log("Shortest Path Reversed is 0");
		}
	}
	
	void OnDrawGizmos()
	{
		if (!ShowGizmos) return;
		if (Area != null)
		{
			for (int x = 0; x < Area.GetLength(0); x++)
			{
				for (int y = 0; y < Area.GetLength(1); y++)
				{
					Gizmos.color = Color.red;
					if (Area[x, y]!=null && Area[x, y].Tile != null)
					{
						Gizmos.DrawSphere(Area[x, y].Tile.transform.position, 0.25f);
					}
				}
			}
		}
		if (ShortestPathReversed != null && ShortestPathReversed.Count > 0)
		{
			PathSegment[] path = ShortestPathReversed.ToArray();
			for (int i = 0; i < path.Length; i++)
			{
				
				Gizmos.color = Color.green;
				if (path[i].Tile != null)
				{
					Gizmos.DrawSphere(path[i].Tile.transform.position, 0.3f);
				}
			}
		}
	}
}
