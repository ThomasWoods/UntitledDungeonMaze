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


    public enum dungeonGameState { init, dungeonExploring, Battling }
    public dungeonGameState currentDungeonGameState;

    private void Awake()
    {
        instance = this;
    }

    public void SwitchDungeonGameState(dungeonGameState newState)
    {
        currentDungeonGameState = newState;
    }
}
