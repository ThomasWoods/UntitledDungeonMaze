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
		get { float volume = 0.5f; audioMixer.GetFloat("MasterVolume",out volume); return volume; }
		set { audioMixer.SetFloat("MasterVolume", value); } }
	public float BGMVolume {
		get { float volume = 0.5f; audioMixer.GetFloat("BGMVolume", out volume); return volume; }
		set { audioMixer.SetFloat("BGMVolume", value); } }
	public float SFXVolume {
		get { float volume = 0.5f; audioMixer.GetFloat("SFXVolume", out volume); return volume; }
		set { audioMixer.SetFloat("SFXVolume", value); } }

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

		GameObject newObj = Instantiate(audioPlayerPrefab);
		BGMPlayer = newObj.GetComponent<AudioSource>();
		newObj = Instantiate(audioPlayerPrefab);
		SFXPlayer = newObj.GetComponent<AudioSource>();

		DontDestroyOnLoad(gameObject);

		LoadSettings();

		instance = this;
	}

	void LoadSettings()
	{
		if (PlayerPrefs.HasKey("MasterVolume")) MasterVolume = PlayerPrefs.GetFloat("MasterVolume");
		if (PlayerPrefs.HasKey("BGMVolume")) BGMVolume = PlayerPrefs.GetFloat("BGMVolume");
		if (PlayerPrefs.HasKey("SFXVolume")) SFXVolume = PlayerPrefs.GetFloat("SFXVolume");
	}

	public void SaveSettings()
	{
		PlayerPrefs.SetFloat("MasterVolume", MasterVolume);
		PlayerPrefs.SetFloat("BGMVolume", BGMVolume);
		PlayerPrefs.SetFloat("SFXVolume", SFXVolume);
	}

	void OnApplicationQuit()
	{
		SaveSettings();
	}
}
