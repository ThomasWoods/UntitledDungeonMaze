using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pauser : MonoBehaviour
{
	public UnityEvent OnPause = new UnityEvent();
	public UnityEvent OnUnpause = new UnityEvent();
	void InvokeOnPause() { OnPause.Invoke(); }
	void InvokeOnUnpause() { OnUnpause.Invoke(); }
	public void Start()
	{
		Game.instance.OnPause.AddListener(InvokeOnPause);
		Game.instance.OnUnpause.AddListener(InvokeOnUnpause);
	}
	public void Pause()
	{
		Game.instance.Pause();
	}
}
