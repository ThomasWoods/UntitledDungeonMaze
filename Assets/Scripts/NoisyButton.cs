using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NoisyButton : Button
{
	public override void OnPointerEnter(PointerEventData eventData)
	{
		if(IsInteractable())Game.instance.PlaySound();
		base.OnPointerEnter(eventData);
	}
	public override void OnSelect(BaseEventData eventData)
	{
		Game.instance.PlaySound();
		base.OnSelect(eventData);
	}
}
