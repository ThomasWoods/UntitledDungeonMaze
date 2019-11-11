using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ColorFadeIn : MonoBehaviour
{
	Color defaultColor;
	public Object ObjectToColor;
	public Color ColorToFadeTo;
	public bool FadeOnEnable;
	public float FadeTime;
	float FadeTimer = 0;
	bool _isFading = false;
	bool isFading { get { return _isFading; } set { _isFading = value; FadeTimer = 0;} }

	public UnityEvent OnFadeInComplete = new UnityEvent();
	void Awake()
	{
		defaultColor = GetObjectColor();
	}
    // Start is called before the first frame update
    void OnEnable()
    {
		if (FadeOnEnable)
		{
			SetObjectColor(defaultColor);
			isFading = true;
		}
    }

    // Update is called once per frame
    void Update()
	{
		if (FadeTimer / FadeTime > 1)
		{
			isFading = false;
			OnFadeInComplete.Invoke();
		}
		if (isFading)
		{
			FadeTimer += Time.deltaTime;

			SetObjectColor(Color.Lerp(GetObjectColor(), ColorToFadeTo, FadeTimer / FadeTime));
			/*
			//Is there a way to procedurally do this?
			if (ObjectToColor.GetType() == typeof(Image))
			{
				Image ImageToColor = ObjectToColor as Image;
				ImageToColor.color = Color.Lerp(ImageToColor.color, ColorToFadeTo, FadeTimer / FadeTime);
			}
			if (ObjectToColor.GetType() == typeof(Text))
			{
				Text ImageToColor = ObjectToColor as Text;
				ImageToColor.color = Color.Lerp(ImageToColor.color, ColorToFadeTo, FadeTimer / FadeTime);
			}
			*/
		}
    }

	Color GetObjectColor()
	{
		//Is there a way to procedurally do this?
		if (ObjectToColor.GetType() == typeof(Image))
		{
			Image ImageToColor = ObjectToColor as Image;
			return ImageToColor.color;
		}
		if (ObjectToColor.GetType() == typeof(Text))
		{
			Text ImageToColor = ObjectToColor as Text;
			return ImageToColor.color;
		}
		return defaultColor;
	}
	void SetObjectColor(Color c)
	{
		//Is there a way to procedurally do this?
		if (ObjectToColor.GetType() == typeof(Image))
		{
			Image ImageToColor = ObjectToColor as Image;
			ImageToColor.color = c;
		}
		if (ObjectToColor.GetType() == typeof(Text))
		{
			Text ImageToColor = ObjectToColor as Text;
			ImageToColor.color = c;
		}
	}
}
