using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovementController : MonoBehaviour
{
    public float movementSpeed;
    public float rotationSpeed;

    public TileBase currentTile;
    public CharacterBaseController m_BaseController;

    Vector3 targetTilePos;
    float checkRadius = 0.2f;
    Vector3 heading = Vector3.zero;
    Vector3 velocity = Vector3.zero;

    Quaternion targetRotaion;
    int turnRotation;
    float degreesRotated = 0;

    private void Awake()
    {
        m_BaseController = GetComponent<CharacterBaseController>();
    }

    public void MoveForward()
    {
        TileBase tile = CheckTileInfront();
        if (tile != null)
        {
            if (tile.walkable && tile.occupant == null)
            {
                // The tile in front of the character is walkable and empty
                currentTile.occupant = null;
                targetTilePos = tile.transform.position;
                m_BaseController.SwitchCharacterStatus(characterStatus.moving);
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

        m_BaseController.SwitchCharacterStatus(characterStatus.turning);
    }

    public TileBase CheckTileInfront()
    {
        TileBase tile = null;
        Collider[] tileColliders = Physics.OverlapSphere(transform.position + transform.forward, checkRadius);

        if (tileColliders.Length != 0)
        {
            tile = tileColliders[0].GetComponent<TileBase>();
        } else
        {
            Debug.Log("Found no tile");
        }

        return tile;
    }

    public TileBase CheckTile()
    {
        TileBase tile = null;
		Collider[] tileColliders = Physics.OverlapSphere(transform.position, checkRadius);

		if (tileColliders.Length != 0)
		{
			tile = tileColliders[0].GetComponent<TileBase>();
		}
		else { Debug.Log("No tiles within range"); }


		return tile;
    }

    public void UpdateMovement()
    {
        if (Vector3.Distance(transform.position, targetTilePos) >= 0.05)
        {
            // The target tile has not yet been reached
            CalculateHeading(targetTilePos);
            SetHorizontalVelocity();
            transform.forward = heading;
            transform.position += velocity * Time.deltaTime;
        }
        else
        {
            // The target tile has been reached
            transform.position = targetTilePos;
            OccupyTile();
            m_BaseController.SwitchCharacterStatus(characterStatus.hasMoved);
        }
    }

    public void OccupyTile()
    {
        currentTile = CheckTile();
		if(currentTile!=null)
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
            m_BaseController.SwitchCharacterStatus(characterStatus.selectingMovement);
        }
    }
}
