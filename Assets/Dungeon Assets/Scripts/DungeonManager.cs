using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager instance;

    public DungeonCard dungeonCard;

    public Camera dungeonCamera;
    public GameObject CharacterStatusCanvas;

    public GameObject tileParentObj;
    public GameObject enemyParentObj;
    
    public enum dungeonGameState { init, dungeonExploring, victory, defeat}
    public dungeonGameState currentDungeonGameState;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        dungeonCamera.backgroundColor = dungeonCard.skyColour;
    }

    public void SwitchDungeonGameState(dungeonGameState newState)
    {
        currentDungeonGameState = newState;
    }

    public void Victory()
    {
        SwitchDungeonGameState(dungeonGameState.victory);
    }

    public void GameOver()
    {
        SwitchDungeonGameState(dungeonGameState.defeat);
        Debug.Log("Game Over!");
    }
}
