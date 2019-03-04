using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
	float change = 30f;
	public GameObject Cam;
	public GameObject Cam2;
	public Camera CamM;
	public Camera camR;
	public GameObject Gun;

	// Use this for initialization
	void Start () 
	{
		//get's references (gameobjects)
		Cam = GameObject.Find ("Main Camera");
		Gun = GameObject.Find ("Gun");
		Cam2 = GameObject.Find ("Main CameraR");

	}
	// Update is called once per frame
	void Update ()
	{
		change -= Time.deltaTime;

		//if time is greater or equal to 17
		if (change >= 17) 
		{
			//move camera 
			Cam.transform.Translate (1.5f * Time.deltaTime, 0f, 0f);
		}
		//if time is less then or equal to 17
		if (change <= 17) 
		{
			//change camera depths and rotate second camera Gun
			camR.depth = 1;
			CamM.depth = 0;
			Cam2.transform.RotateAround (Gun.transform.position, Vector3.up, 60 * Time.deltaTime);
		}
		//when time runs out
		if (change <= 0) 
		{
			//reload scene
			SceneManager.LoadScene ("Main Menu");
		}
	}
	//closes game
	public void Exit()
	{
		Application.Quit ();
	}
	//loads scene
	public void Level()
	{
		SceneManager.LoadScene ("Levels");
	}
	//loads scene
	public void Control()
	{
		SceneManager.LoadScene ("Controls");
	}

}

