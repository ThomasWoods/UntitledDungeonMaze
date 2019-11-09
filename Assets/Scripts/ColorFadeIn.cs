using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ColorFadeIn : MonoBehaviour
{
	public Object ObjectToColor;
	public Color ColorToFadeTo;
	public bool FadeOnEnable;
	public float FadeTime;
	float FadeTimer = 0;
	bool _isFading = false;
	bool isFading { get { return _isFading; } set { _isFading = value; FadeTimer = 0;} }

	public UnityEvent OnFadeInComplete = new UnityEvent();

    // Start is called before the first frame update
    void OnEnable()
    {
		if (FadeOnEnable) isFading = true;
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
		}
    }
}
