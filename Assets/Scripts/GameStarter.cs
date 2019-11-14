using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameStarter : MonoBehaviour
{
    public SceneChanger m_SceneChanger;

    public DungeonCard desertDungeonCard;
    public DungeonCard forestDungeonCard;

    public Animator m_FadePanelAnimator;

    public TextMeshProUGUI startButtonText;

    public string newGameText;
    public string dungeonSelectText;

    public GameObject mainMenuPanel;
    public GameObject levelSelectPanel;

    private void Start()
    {
        Game.DungeonData.hasBeenVictorious = true;

        if (Game.DungeonData.wasVictorious)
            Game.DungeonData.hasBeenVictorious = true;

        if (Game.DungeonData.hasBeenVictorious)
            startButtonText.text = dungeonSelectText;
        else
            startButtonText.text = newGameText;
    }

    public void StartButtonPressed()
    {
        if (Game.DungeonData.hasBeenVictorious)
        {
            levelSelectPanel.SetActive(true);
            mainMenuPanel.SetActive(false);
        }
        else
            StartGame(0);
    }

    public void StartGame(int dungeonID)
    {
        DungeonCard chosenCard = null;

        switch (dungeonID)
        {
            case 0:
                chosenCard = forestDungeonCard;
                break;

            case 1:
                chosenCard = desertDungeonCard;
                break;
        }

        if (chosenCard != null)
        {
            Game.DungeonData.dungeonCard = chosenCard;

            StartCoroutine(FadeToGameplay());
        }
        else
            Debug.LogWarning("Unvalid dungeonCardId given to the gameStarter!");
    }

    private IEnumerator FadeToGameplay()
    {
		StartCoroutine(Game.instance.FadeOutBGM(1f));
		m_FadePanelAnimator.SetBool("FadeOut", true);
		yield return new WaitForSeconds(1f);
		Game.instance.PlayNewGameSound();
		yield return new WaitForSeconds(4f);
		Game.instance.PlayDungeonMusic();
		m_SceneChanger.ToGame();
    }
}
