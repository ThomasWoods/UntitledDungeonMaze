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

    public GameObject victoryPanel;
    public GameObject gameOverPanel;
    public GameObject pausePanel;

    public enum dungeonGameState { init, dungeonExploring, victory, defeat}
    public dungeonGameState currentDungeonGameState;

	public DungeonBaseController dungeonController;

    private void Awake()
    {
        instance = this;
		dungeonController = FindObjectOfType<DungeonBaseController>();

        if (Game.DungeonData.dungeonCard != null)
            dungeonCard = Game.DungeonData.dungeonCard;
    }

    private void Start()
    {
        dungeonCamera.backgroundColor = dungeonCard.skyColour;
        Game.DungeonData.wasVictorious = false;
    }

	void Update()
	{
		switch (currentDungeonGameState) {
			case dungeonGameState.dungeonExploring:
				DungeonBaseController.instance.DungeonStateLogic();
				break;
		}
	}

    public void SwitchDungeonGameState(dungeonGameState newState)
    {
        currentDungeonGameState = newState;
    }

    public void Victory()
    {
        SwitchDungeonGameState(dungeonGameState.victory);
        Game.DungeonData.wasVictorious = true;
        pausePanel.SetActive(false);
        victoryPanel.SetActive(true);
    }

    public void GameOver()
    {
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(true);
        SwitchDungeonGameState(dungeonGameState.defeat);
    }
}
