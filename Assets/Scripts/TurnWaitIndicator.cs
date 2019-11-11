using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnWaitIndicator : MonoBehaviour
{
	public int rotation = 0;
	public float waitTimer = 0;
	public bool spun = false;
	public int spinSpeed = 1;
	public float waitTime = 0.5f;

	void OnEnable()
	{
		waitTimer = 0.1f;
		spun = false;
		rotation = 0;
		transform.eulerAngles = new Vector3(0, 0, rotation);
	}
	void Update()
	{
		if (waitTimer <= 0)
		{
			rotation += spinSpeed;
			if (rotation > 180 && !spun)
			{
				spun = true;
				waitTimer = waitTime;
			}
			if (rotation > 360)
			{
				spun = false;
				rotation = 0;
				waitTimer = waitTime;
			}
		}
		else waitTimer -= Time.deltaTime;
		transform.eulerAngles = new Vector3(0, 0, rotation);
	}

	public void UpdateIndicator(CharacterStatus status)
	{
		if (status == CharacterStatus.selectingMovement || status == CharacterStatus.moving || status == CharacterStatus.turning || status==CharacterStatus.defeated)
			gameObject.SetActive(false);
		else gameObject.SetActive(true);
	}
}
