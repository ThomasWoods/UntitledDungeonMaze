using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthIndicator : MonoBehaviour
{
	public GameObject[] images = new GameObject[3];

	public void SetHearts(int i)
	{
		if (i > 0) images[0].SetActive(true); else images[0].SetActive(false);
		if (i > 1) images[1].SetActive(true); else images[1].SetActive(false);
		if (i > 2) images[2].SetActive(true); else images[2].SetActive(false);
	}
}
