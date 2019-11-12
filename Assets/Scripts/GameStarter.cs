using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStarter : MonoBehaviour
{
    public SceneChanger m_SceneChanger;

    public DungeonCard desertDungeonCard;
    public DungeonCard forestDungeonCard;

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

            m_SceneChanger.ToGame();
        }
        else
            Debug.LogWarning("Unvalid dungeonCardId given to the gameStarter!");
    }
}
