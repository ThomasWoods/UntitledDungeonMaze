using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public int turnCounter = 0;
    public float turnTime = 0.5f;

    public enum TurnManagerState { idle, dequeueing, acting}
    public TurnManagerState currentState;

    public enum TeamTurn { player, enemies}
    public TeamTurn currentTeamTurn;

    private Queue<CharacterBaseController> enemiesToAct = new Queue<CharacterBaseController>();
    private CharacterBaseController characterToAct;

    private float timer = 0f;

    public void StartNewTurn()
    {
        turnCounter++;
		DungeonBaseController.instance.TurnStart();
        EnqueueEnemies();

        SwitchTeamTurn(TeamTurn.player);
        SwitchCurrentState(TurnManagerState.dequeueing);
    }

    public void TurnManagerUpdate()
    {
        switch (currentState)
        {
            case TurnManagerState.acting:
                timer -= Time.deltaTime;
                if (timer <= 0)
                    TurnIsOver();
                break;

            case TurnManagerState.dequeueing:
                DequeueCharacterToAct();
                break;
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

                DungeonBaseController.instance.m_PlayerController.Activate();
                SwitchCurrentState(TurnManagerState.idle);
				bool ready = true;
				foreach (CharacterBaseController enemy in DungeonBaseController.instance.enemies)
				{
					if (!(enemy.currentCharacterStatus == CharacterStatus.idle ||
						enemy.currentCharacterStatus == CharacterStatus.defeated)) ready = false;
				}

				if (ready)
                {
					CharacterBaseController playerController = DungeonBaseController.instance.m_PlayerController;

                    if (playerController.currentCharacterStatus == CharacterStatus.idle)
					{
						CharacterBaseController[] allCharacters = new CharacterBaseController[DungeonBaseController.instance.allCharacters.Count];
						DungeonBaseController.instance.allCharacters.CopyTo(allCharacters);

						playerController.Activate();
                        SwitchCurrentState(TurnManagerState.idle);
                    }
                }
                
                break;

            case TeamTurn.enemies:

                if(enemiesToAct.Count > 0)
                {
                    characterToAct = enemiesToAct.Dequeue();

					if (characterToAct != null)
					{
						ActivateEnemy();
					}
					else Debug.Log("No character to act?");
                }
                else
                {
                    SwitchCurrentState(TurnManagerState.acting);
                }

                break;
        }
        
    }

    public void ActionIsDone()
    {
        switch (currentTeamTurn)
        {
            case TeamTurn.player:
                timer = turnTime;
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
			DungeonBaseController.instance.m_PlayerController.TakeDamage(characterToAct.name);
            DequeueCharacterToAct();
		}
        else
        {
            characterToAct.Activate();
            SwitchCurrentState(TurnManagerState.idle);
        }
    }

    void TurnIsOver()
    {
        DungeonBaseController.instance.TurnEnd();
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
