using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FirstTimeOnly : MonoBehaviour
{
	public static bool Ran;
	public UnityEvent onFirstAwake = new UnityEvent();
	public UnityEvent onSubsequentAwakes = new UnityEvent();

	void Awake()
	{
		if (!FirstTimeOnly.Ran)
		{
			Debug.Log("First Time Run");
			onFirstAwake.Invoke();
			Ran = true;
		}
		else
		{
			Debug.Log("onSubsequentAwakes");
			onSubsequentAwakes.Invoke();
		}
	}
}
