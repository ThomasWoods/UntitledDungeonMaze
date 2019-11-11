using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
	public void ToGame()
	{
		SceneManager.LoadScene(1);
	}
	public void ToMenu()
	{
		Game.instance.Unpause();
		SceneManager.LoadScene(0);
	}
	public void QuitGame()
	{
		Game.instance.SaveSettings();
		Application.Quit(0);
	}
}
