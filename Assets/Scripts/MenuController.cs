using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
	public Slider MasterVolumeSlider, BGMSlider, SFXSlider;

	public void ChangeMasterVolume(float f) { Game.instance.MasterVolume = MasterVolumeSlider.value; }
	public void ChangeBGMVolume(float f) { Game.instance.BGMVolume = BGMSlider.value; }
	public void ChangeSFXVolume(float f) { Game.instance.SFXVolume = SFXSlider.value; }

	void OnEnable()
	{
		InitializeVolumeSliders();
	}

	void InitializeVolumeSliders()
	{
		if (MasterVolumeSlider != null)
		{
			MasterVolumeSlider.value = Game.instance.MasterVolume;
			MasterVolumeSlider.onValueChanged.AddListener(ChangeMasterVolume);
		}
		if (BGMSlider != null)
		{
			BGMSlider.value = Game.instance.BGMVolume;
			MasterVolumeSlider.onValueChanged.AddListener(ChangeMasterVolume);
		}
		if (SFXSlider != null)
		{
			SFXSlider.value = Game.instance.SFXVolume;
			MasterVolumeSlider.onValueChanged.AddListener(ChangeMasterVolume);
		}
	}
}
