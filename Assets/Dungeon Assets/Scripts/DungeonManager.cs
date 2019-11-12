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

    public GameObject gameOverPanel;

    public enum dungeonGameState { init, dungeonExploring, victory, defeat}
    public dungeonGameState currentDungeonGameState;

	public DungeonBaseController dungeonController;

    private void Awake()
    {
        instance = this;
		dungeonController = FindObjectOfType<DungeonBaseController>();
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
        gameOverPanel.SetActive(true);
        SwitchDungeonGameState(dungeonGameState.defeat);
        Debug.Log("Game Over!");
    }
}
