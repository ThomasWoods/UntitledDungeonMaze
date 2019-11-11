using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public int turnCounter = 0;

    public enum TurnManagerState { idle, dequeueing}
    public TurnManagerState currentState;

    public enum TeamTurn { player, enemies}
    public TeamTurn currentTeamTurn;

    private Queue<CharacterBaseController> enemiesToAct = new Queue<CharacterBaseController>();
    private CharacterBaseController characterToAct;

    public void StartNewTurn()
    {
        turnCounter++;
        EnqueueEnemies();

        //DungeonBaseController.instance.TurnStart();

        SwitchTeamTurn(TeamTurn.player);
        SwitchCurrentState(TurnManagerState.dequeueing);
    }

    public void TurnManagerUpdate()
    {
        switch (currentState)
        {
            case TurnManagerState.dequeueing:
                DequeueCharacterToAct();
                break;
        }
    }
    /*
    public void RemakeQueue()
    {
        enemiesToAct.Clear();
        EnqueueEnemies();
    }
    */
    public void RemoveFromQueue(CharacterBaseController charBase)
    {
        Debug.Log("Queue: " + enemiesToAct.Count);

        if (enemiesToAct.Count != 0)
        {
            Queue<CharacterBaseController> tempQueue = new Queue<CharacterBaseController>();

            while (enemiesToAct.Count > 0)
            {
                CharacterBaseController enqueuedChar = enemiesToAct.Dequeue();

                if (enqueuedChar != charBase)
                    tempQueue.Enqueue(enqueuedChar);
            }

            while (tempQueue.Count > 0)
            {
                CharacterBaseController enqueuedChar = enemiesToAct.Dequeue();

                enemiesToAct.Enqueue(enqueuedChar);
            }
        }
    }

    public void EnqueueEnemies()
    {
        foreach(CharacterBaseController enemy in DungeonBaseController.instance.enemies)
        {
            enemiesToAct.Enqueue(enemy);
        }
    }

    public void DequeueCharacterToAct()
    {
        switch (currentTeamTurn)
        {
            case TeamTurn.player:
				bool ready = true;
				foreach (CharacterBaseController enemy in DungeonBaseController.instance.enemies)
				{
					if (enemy.currentCharacterStatus != CharacterStatus.idle) ready = false;
				}

				if (ready)
                {
                    CharacterBaseController playerController = DungeonBaseController.instance.m_PlayerController;

                    if (playerController.currentCharacterStatus == CharacterStatus.idle)
                    {
                        playerController.Activate();
                        SwitchCurrentState(TurnManagerState.idle);
                    }
                }
                
                break;

            case TeamTurn.enemies:

                if(enemiesToAct.Count > 0)
                {
                    characterToAct = enemiesToAct.Dequeue();

                    if(characterToAct != null)
                    {
                        ActivateEnemy();
                    }
                }
                else
                {
                    TurnIsOver();
                }

                break;
        }
        
    }

    public void ActionIsDone()
    {
        switch (currentTeamTurn)
        {
            case TeamTurn.player:
                SwitchTeamTurn(TeamTurn.enemies);
				SwitchCurrentState(TurnManagerState.dequeueing);
                break;

            case TeamTurn.enemies:
                DequeueCharacterToAct();
                break;
        }
    }

    private void ActivateEnemy()
    {
        // Check if hte player is standing on the tile in front of the enemy at the start of their turn.
        TileBase tile = characterToAct.m_MovementController.CheckTile(characterToAct.transform.forward);

		if (tile.occupant == DungeonBaseController.instance.m_Player)
		{
			Debug.Log("The player was cought!");
			DungeonBaseController.instance.m_PlayerController.TakeDamage(characterToAct.name);
		}
        characterToAct.Activate();

        SwitchCurrentState(TurnManagerState.idle);
    }


    void TurnIsOver()
    {
        //DungeonBaseController.instance.TurnEnd();
        StartNewTurn();
    }

    public void ResetTurnManager()
    {
        turnCounter = 0;
        enemiesToAct.Clear();
    }

    public void SwitchCurrentState(TurnManagerState newState)
    {
        currentState = newState;
    }


    public void SwitchTeamTurn(TeamTurn newTeamTurn)
    {
        currentTeamTurn = newTeamTurn;
    }

}
