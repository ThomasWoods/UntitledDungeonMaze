using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager instance;

    public DungeonCard dungeonCard;

    public Camera dungeonCamera;
    public Camera battleCamera;
    public GameObject CharacterStatusCanvas;

    public enum dungeonGameState { init, dungeonExploring, Battling }
    public dungeonGameState currentDungeonGameState;

    private void Awake()
    {
        instance = this;
    }

    public void StartRandomBattle()
    {
        CharacterStatusCanvas.SetActive(true);
        SwitchToBattleCamera();
        Debug.Log("Battle!");
    }

    public void SwitchDungeonGameState(dungeonGameState newState)
    {
        currentDungeonGameState = newState;
    }

    void SwitchToDungeonCamera()
    {
        dungeonCamera.gameObject.SetActive(true);
        battleCamera.gameObject.SetActive(false);
    }

    void SwitchToBattleCamera()
    {
        dungeonCamera.gameObject.SetActive(false);
        battleCamera.gameObject.SetActive(true);
    }
}
