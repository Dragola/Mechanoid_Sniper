using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ControlScene : MonoBehaviour 
{
	//loads different scenes (for buttons)
	public void Return()
	{
		SceneManager.LoadScene ("Main Menu");
	}
	public void Survival()
	{
		SceneManager.LoadScene ("Endless Mode #1");
	}
	public void Training()
	{
		SceneManager.LoadScene ("Training");
	}
	public void Exit()
	{
		Application.Quit ();
	}
	public void Try()
	{
		SceneManager.LoadScene ("Survival");
	}
}
