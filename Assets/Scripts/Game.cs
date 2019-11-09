using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Game : MonoBehaviour
{
	public AudioMixer audioMixer;
	public GameObject audioPlayerPrefab;
	[HideInInspector]
	public AudioSource BGMPlayer, SFXPlayer;
	public float MasterVolume {
		get {
			float volume = -100;
			audioMixer.GetFloat("MasterVolume",out volume);
			if (volume > -80)
			{
				Debug.Log("MasterVolume retrieved: "+volume);
				return volume;
			}
			else
			{
				Debug.Log("No MasterVolume in audioiMixer?" + audioMixer);
				return 0;
			}
		}
		set { Debug.Log("Setting Master Volume to " + value+", "+audioMixer); audioMixer.SetFloat("MasterVolume", value); } }
	public float BGMVolume {
		get { float volume = 0.0f; audioMixer.GetFloat("BGMVolume", out volume); return volume; }
		set { audioMixer.SetFloat("BGMVolume", value); } }
	public float SFXVolume {
		get { float volume = 0.0f; audioMixer.GetFloat("SFXVolume", out volume); return volume; }
		set { audioMixer.SetFloat("SFXVolume", value); } }

	public void PlaySound()
	{
		AudioClip sound = Resources.Load<AudioClip>("GDC Audio/button_002");
		if (sound != null) SFXPlayer.PlayOneShot(sound);
	}

	public bool SettingsLoaded = false;


	public static Game _instance;
	public static Game instance
	{
		get {
			if (_instance) return _instance;
			else {
				Game findGame = FindObjectOfType<Game>();
				if (findGame != null) return findGame;
				else {
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
	
    void Awake()
    {

		if (audioMixer == null) audioMixer = Resources.Load("GameAudioMixer") as AudioMixer;
		if (audioPlayerPrefab == null) audioPlayerPrefab = Resources.Load("Prefabs/AudioPlayer") as GameObject;
		if (audioPlayerPrefab == null) { Destroy(gameObject); Debug.Log("No AudioPlayer resource!"); return; }

		GameObject newObj = Instantiate(audioPlayerPrefab,transform);
		BGMPlayer = newObj.GetComponent<AudioSource>();
		BGMPlayer.outputAudioMixerGroup = audioMixer.FindMatchingGroups("BGM")[0];
		newObj = Instantiate(audioPlayerPrefab,transform);
		SFXPlayer = newObj.GetComponent<AudioSource>();
		SFXPlayer.outputAudioMixerGroup = audioMixer.FindMatchingGroups("SFX")[0];

		DontDestroyOnLoad(gameObject);

		instance = this;
	}

	void Start()
	{
		LoadSettings();
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
}
