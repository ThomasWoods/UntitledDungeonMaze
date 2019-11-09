using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
	public Slider MasterVolumeSlider, BGMSlider, SFXSlider;

	public void ChangeMasterVolume(float f) { Debug.Log("Menu changed Master Volume"); Game.instance.MasterVolume = MasterVolumeSlider.value; }
	public void ChangeBGMVolume(float f) { Game.instance.BGMVolume = BGMSlider.value; }
	public void ChangeSFXVolume(float f) { Game.instance.SFXVolume = SFXSlider.value; }

	IEnumerator Start()
	{
		while (!Game.instance.SettingsLoaded)
			yield return 0;
		InitializeVolumeSliders();
	}

	void InitializeVolumeSliders()
	{
		if (MasterVolumeSlider != null)
		{
			Debug.Log("Initializing Master Volume Slider");
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
