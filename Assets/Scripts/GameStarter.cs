﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStarter : MonoBehaviour
{
    public SceneChanger m_SceneChanger;

    public DungeonCard desertDungeonCard;
    public DungeonCard forestDungeonCard;

    public Animator m_FadePanelAnimator;

    public void StartGame(int dungeonID)
    {
        DungeonCard chosenCard = null;

        switch (dungeonID)
        {
            case 0:
                chosenCard = desertDungeonCard;
                break;

            case 1:
                chosenCard = forestDungeonCard;
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
