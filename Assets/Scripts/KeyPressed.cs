using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeyPressed : MonoBehaviour
{
	public List<KeyCode> Keys;
	public UnityEvent OnKey = new UnityEvent();
	public UnityEvent OnDown = new UnityEvent();
	public UnityEvent OnUp = new UnityEvent();
	
    void Update()
    {
		foreach (KeyCode k in Keys)
		{
			if (Input.GetKey(k)) OnKey.Invoke();
			if (Input.GetKeyDown(k)) OnDown.Invoke();
			if (Input.GetKeyUp(k)) OnUp.Invoke();
		}
    }
}
