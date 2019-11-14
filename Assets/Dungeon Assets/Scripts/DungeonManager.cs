using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    public GameObject raycastShield;

    public TextMeshProUGUI victoryText;
    public TextMeshProUGUI gameOverText;

    public SceneChanger m_SceneChanger;

    public enum dungeonGameState { init, dungeonExploring, victory, defeat}
    public dungeonGameState currentDungeonGameState;

	public DungeonBaseController dungeonController;

    public Options options;
    
    private void Awake()
    {
        instance = this;
		dungeonController = FindObjectOfType<DungeonBaseController>();

        if (Game.DungeonData.dungeonCard != null)
            dungeonCard = Game.DungeonData.dungeonCard;
    }

    private void Start()
    {
        victoryText.text = dungeonCard.victoryText;
        dungeonCamera.backgroundColor = dungeonCard.skyColour;
        options.InitializeVolumeSliders();
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
        StartCoroutine(Fadeout()); 
    }

    private IEnumerator Fadeout()
    {
        raycastShield.SetActive(true);
        Game.instance.FadeOutBGM(1f);
        DungeonBaseController.instance.m_FadeOutAnimator.SetBool("FadeOut", true);
        yield return new WaitForSeconds(1f);
        m_SceneChanger.ToMenu();
    }

    [System.Serializable]
    public class Options
    {
        public Slider MasterVolumeSlider, BGMSlider, SFXSlider;

        public void ChangeMasterVolume(float f) { Game.instance.MasterVolume = MasterVolumeSlider.value; }
        public void ChangeBGMVolume(float f) { Game.instance.BGMVolume = BGMSlider.value; }
        public void ChangeSFXVolume(float f) { Game.instance.SFXVolume = SFXSlider.value; }

        public void InitializeVolumeSliders()
        {
            if (MasterVolumeSlider != null)
            {
                MasterVolumeSlider.value = Game.instance.MasterVolume;
                MasterVolumeSlider.onValueChanged.AddListener(ChangeMasterVolume);
            }
            if (BGMSlider != null)
            {
                BGMSlider.value = Game.instance.BGMVolume;
                BGMSlider.onValueChanged.AddListener(ChangeBGMVolume);
            }
            if (SFXSlider != null)
            {
                SFXSlider.value = Game.instance.SFXVolume;
                SFXSlider.onValueChanged.AddListener(ChangeSFXVolume);
            }
        }
    }
}
