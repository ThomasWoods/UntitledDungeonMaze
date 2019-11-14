using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NoisyButton : Button
{
	public override void OnPointerEnter(PointerEventData eventData)
	{
		if(IsInteractable())Game.instance.PlayButtonSound();
		base.OnPointerEnter(eventData);
	}
	public override void OnSelect(BaseEventData eventData)
	{
		Game.instance.PlayButtonSound();
		base.OnSelect(eventData);
	}
}
