using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

public class Game : MonoBehaviour
{
	//singleton
	public static Game _instance;
	public static Game instance
	{
		get {
			if (_instance) return _instance;
			else
			{
				Game findGame = FindObjectOfType<Game>();
				if (findGame != null) return findGame;
				else
				{
					GameObject newGameObj = new GameObject("Game");
					findGame = newGameObj.AddComponent<Game>();
					return findGame;
				}
			}
		}
		set {
			if (_instance != null) Destroy(_instance.gameObject);
			_instance = value;
		}
	}

	//Audio
	public AudioMixer audioMixer;
	public GameObject audioPlayerPrefab;
	[HideInInspector]
	public AudioSource BGMPlayer, SFXPlayer;
	public float MasterVolume {
		get { float volume = 0.0f; audioMixer.GetFloat("MasterVolume",out volume); return volume; }
		set { audioMixer.SetFloat("MasterVolume", value); } }
	public float BGMVolume {
		get { float volume = 0.0f; audioMixer.GetFloat("BGMVolume", out volume); return volume; }
		set { audioMixer.SetFloat("BGMVolume", value); } }
	public float SFXVolume {
		get { float volume = 0.0f; audioMixer.GetFloat("SFXVolume", out volume); return volume; }
		set { audioMixer.SetFloat("SFXVolume", value); } }

	public void PlayButtonSound() { SFXPlayer.PlayOneShot(ButtonSFX); }
	public void PlayFootstep() { SFXPlayer.PlayOneShot(WalkSFX); }
	public void PlayNewGameSound() { SFXPlayer.PlayOneShot(NewGameSFX); }
	public void PlayMainMenuMusic()
	{
		BGMPlayer.clip = MainMenuBGM;
		BGMPlayer.loop = true;
		BGMPlayer.Play();
	}
	public void PlayDungeonMusic()
	{
		BGMPlayer.clip = DungeonBGM;
		BGMPlayer.loop = true;
		BGMPlayer.Play();
	}
	public IEnumerator PlayVictoryMusic()
	{
		yield return StartCoroutine(FadeOutBGM(1f));
		BGMPlayer.clip = VictoryBGM;
		BGMPlayer.loop = false;
		BGMPlayer.Play();
	}
	public void PlayDeathMusic()
	{
		BGMPlayer.clip = DeathBGM;
		BGMPlayer.loop = false;
		BGMPlayer.Play();
	}
	public IEnumerator FadeOutBGM(float time)
	{
		float startVolume = BGMPlayer.volume;
		while (BGMPlayer.volume > 0)
		{
			BGMPlayer.volume -= Time.deltaTime / time;
			yield return 0;
		}
		BGMPlayer.Stop();
		BGMPlayer.volume = startVolume;
	}
	public AudioClip MainMenuBGM, DungeonBGM, VictoryBGM, DeathBGM,
		ButtonSFX, WalkSFX, NewGameSFX;
	public bool SettingsLoaded = false;


	//Pause
	public bool _isPaused = false;
	public bool isPaused
	{
		get { return _isPaused; }
		set { if (_isPaused == value) return; if (!_isPaused) OnPause.Invoke(); else OnUnpause.Invoke(); _isPaused = value; }
	}
	public UnityEvent OnPause = new UnityEvent();
	public UnityEvent OnUnpause = new UnityEvent();

    void Awake()
    {
		if (_instance != null)
		{
			Destroy(gameObject);
			return;
		}
		_instance = this;
		DontDestroyOnLoad(gameObject);

		if (audioMixer == null) audioMixer = Resources.Load("GameAudioMixer") as AudioMixer;
		if (audioPlayerPrefab == null) audioPlayerPrefab = Resources.Load("Prefabs/AudioPlayer") as GameObject;
		if (audioPlayerPrefab == null) { Destroy(gameObject); Debug.Log("No AudioPlayer resource!"); return; }

		GameObject newObj = Instantiate(audioPlayerPrefab,transform);
		BGMPlayer = newObj.GetComponent<AudioSource>();
		BGMPlayer.outputAudioMixerGroup = audioMixer.FindMatchingGroups("BGM")[0];
		newObj = Instantiate(audioPlayerPrefab,transform);
		SFXPlayer = newObj.GetComponent<AudioSource>();
		SFXPlayer.outputAudioMixerGroup = audioMixer.FindMatchingGroups("SFX")[0];

		MainMenuBGM = Resources.Load("Philammon Music/Going_Nowhere") as AudioClip;
		VictoryBGM = Resources.Load("Philammon Music/Going_Nowhere_Victory_Theme") as AudioClip;
		DeathBGM = Resources.Load("Philammon Music/Labyrinth_Death_Theme") as AudioClip;
		DungeonBGM = Resources.Load("Philammon Music/Labyrinth_Theme") as AudioClip;

		ButtonSFX = Resources.Load<AudioClip>("GDC Audio/button_002");
		WalkSFX = Resources.Load<AudioClip>("GDC Audio/S23_SFX_Footsteps_Gravel_Loafers_Loops_Walk_Normal (modified)");
		NewGameSFX = Resources.Load<AudioClip>("Philammon Music/MinobusLine");
	}

	void Start()
	{
		LoadSettings();
		PlayMainMenuMusic();
	}

	void LoadSettings()
	{
		if (PlayerPrefs.HasKey("MasterVolume")) MasterVolume = PlayerPrefs.GetFloat("MasterVolume"); else Debug.Log("No saved MasterVolume value");
		if (PlayerPrefs.HasKey("BGMVolume")) BGMVolume = PlayerPrefs.GetFloat("BGMVolume"); else Debug.Log("No saved BGMVolume value");
		if (PlayerPrefs.HasKey("SFXVolume")) SFXVolume = PlayerPrefs.GetFloat("SFXVolume"); else Debug.Log("No saved SFXVolume value");
		SettingsLoaded = true;

		Debug.Log("Settings Loaded");
	}

	public void SaveSettings()
	{
		PlayerPrefs.SetFloat("MasterVolume", MasterVolume);
		PlayerPrefs.SetFloat("BGMVolume", BGMVolume);
		PlayerPrefs.SetFloat("SFXVolume", SFXVolume);

		Debug.Log("Settings Saved");
	}

	void OnApplicationQuit()
	{
		SaveSettings();
	}

	public void Pause()
	{
		isPaused = !isPaused;
	}
	public void Unpause()
	{
		isPaused = false;
	}

    public class DungeonData
    {
        public static DungeonCard dungeonCard;

        public static bool wasVictorious = false;
    }

}
