using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    public TextMeshProUGUI gameOverText;

    public SceneChanger m_SceneChanger;

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
		StartCoroutine(Game.instance.PlayVictoryMusic());
		SwitchDungeonGameState(dungeonGameState.victory);
        Game.DungeonData.wasVictorious = true;
        pausePanel.SetActive(false);
        victoryPanel.SetActive(true);
    }

    public void GameOver()
    {
		Game.instance.PlayDeathMusic();
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(true);
        gameOverText.text = DungeonBaseController.instance.m_PlayerController.damageSource;
        SwitchDungeonGameState(dungeonGameState.defeat);
    }

    public void ReturnToMainMenu()
    {
        Debug.Log("Check");
        StartCoroutine(Fadeout()); 
    }

    private IEnumerator Fadeout()
    {
        DungeonBaseController.instance.m_FadeOutAnimator.SetBool("FadeOut", true);
        yield return new WaitForSeconds(1f);
        m_SceneChanger.ToMenu();
    }
}
