using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovementController : MonoBehaviour
{
    public float movementSpeed;
    public float rotationSpeed;

    public bool canMoveAfterTurning = false;

    public TileBase currentTile;
    public CharacterBaseController m_BaseController;

    Vector3 targetTilePos;
    float checkRadius = 0.2f;
    Vector3 heading = Vector3.zero;
    Vector3 velocity = Vector3.zero;

    Quaternion targetRotaion;
    int turnRotation;
    float degreesRotated = 0;

	public bool AlwaysFaceForward = false;

    private void Awake()
    {
        m_BaseController = GetComponent<CharacterBaseController>();
    }

    public void Move(Vector3 relativePos = new Vector3())
    {
        TileBase tile = CheckTile(relativePos);
        if (tile != null)
        {
            if (tile.walkable && tile.occupant == null)
            {
                // The tile in front of the character is walkable and empty
                currentTile.occupant = null;
                OccupyNextTile(tile);
                targetTilePos = tile.transform.position;
                m_BaseController.SetWalking();
            }
        }
        else
        {
			if (tile != null)
				Debug.LogWarning(tile.gameObject.name + " is in the Tiles layer but doesn't have a BaseTile component!");
			else Debug.LogWarning("Tile does not exist.");
        }
    }
	

    public void Turn(int dir)
    {
        turnRotation = dir;
        targetRotaion = transform.rotation;
        targetRotaion *= Quaternion.Euler(new Vector3(0, 90f * turnRotation, 0));

        m_BaseController.SwitchCharacterStatus(CharacterStatus.turning);
    }
	

    public TileBase CheckTile(Vector3 relativePos = new Vector3())
    {
        TileBase tile = null;
		Collider[] tileColliders = Physics.OverlapSphere(transform.position+relativePos, checkRadius);

		if (tileColliders.Length != 0)
		{
			tile = tileColliders[0].GetComponent<TileBase>();
		}


		return tile;
    }

	public TileBase FindNearestOpenTile(Vector3 SearchFrom, bool includeSpecialTiles = false)
	{
		TileBase tile = null;
		List<TileBase> checkedTiles = new List<TileBase>();
		int growingRadius = 1;
		do {
			Collider[] tileColliders = Physics.OverlapSphere(SearchFrom, growingRadius++);
			for(int i=0;i<tileColliders.Length;i++)
			{
				TileBase check = tileColliders[i].GetComponent<TileBase>();
				if (check != null && check.walkable && check.occupant == null && 
					(includeSpecialTiles || (check != DungeonBaseController.instance.entranceTile && check != DungeonBaseController.instance.exitTile)) )
				{
					tile = check;
					growingRadius = 20;
					break;
				}
			}
		}
		while (growingRadius<20);

		return tile;
	}

    public void UpdateMovement()
    {
        if (Vector3.Distance(transform.position, targetTilePos) >= 0.05)
        {
            // The target tile has not yet been reached
            CalculateHeading(targetTilePos);
            SetHorizontalVelocity();
            if(AlwaysFaceForward) transform.forward = heading;
            transform.position += velocity * Time.deltaTime;
        }
        else
        {
            // The target tile has been reached
            transform.position = targetTilePos;

            currentTile.TileSteppedOn();

            m_BaseController.DoneWalking();
        }
    }

    public void OccupyTile()
    {
        currentTile = CheckTile();
		if(currentTile!=null)
			currentTile.occupant = gameObject;
    }

    void OccupyNextTile(TileBase nextTile)
    {
        currentTile = nextTile;
        currentTile.occupant = gameObject;
    }

    void CalculateHeading(Vector3 targetPos)
    {
        heading = targetPos - transform.position;
        heading.Normalize();
    }

    void SetHorizontalVelocity()
    {
        velocity = heading * movementSpeed;
    }

    public void UpdateTurn()
    {
        if (degreesRotated <= 89)
        {
            float t = rotationSpeed * Time.deltaTime;
            degreesRotated += t;
            Vector3 turn = new Vector3(0, t * turnRotation, 0);
            transform.Rotate(turn);
        }
        else
        {
            transform.rotation = targetRotaion;
            degreesRotated = 0;

            if (canMoveAfterTurning && !m_BaseController.confused)
                m_BaseController.SwitchCharacterStatus(CharacterStatus.selectingMovement);
            else 
                m_BaseController.SwitchCharacterStatus(CharacterStatus.idle);
        }
    }
}
