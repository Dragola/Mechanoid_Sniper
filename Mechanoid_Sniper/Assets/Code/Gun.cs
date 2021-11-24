using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class Gun : MonoBehaviour 
{
	float mouseY;
	float sensitivity;
	public Camera cameraMain;
	public Camera cameraAim;
	public GameObject bullet;
	public Transform gunEnd;
	public float GunReload = 2;
	public TextMesh ReloadM;
	public TextMesh ReloadA;
	public float heartBeat = 115;
	public Text BPM;
	Vector3 LastPos;
	bool ZoomIn= false; 
	bool ZoomOut= false;
	public bool TitanAccess = false;
	public Text Interact;
	public float Distance; 
	Titan ScriptT;
	Player ScriptP;
	public GameObject Player;
	public bool kill = false;
	public int ammo = 5;
	public bool reloadA;
	bool reload;
	Text clip;
	// Use this for initialization
	void Start () 
	{
		//sets references 
		ScriptT = GameObject.Find ("Titan").GetComponent<Titan>();
		ScriptP = GameObject.Find ("Player").GetComponent<Player>();
		Interact = GameObject.Find ("Interact").GetComponent<Text>();
		clip = GameObject.Find ("Clip").GetComponent<Text> ();

		//sets parts of UI to false
		ReloadM.gameObject.SetActive (false);
		ReloadA.gameObject.SetActive (false);
		BPM.gameObject.SetActive (false);
		Interact.gameObject.SetActive (false);
		clip.text = "Clip: 5";
	}
	
	// Update is called once per frame
	void Update () 
	{
		//get's sensitivity from Player Script
		sensitivity = ScriptP.sensitivity;

		//gets Distance from Titan script
		Distance = ScriptT.Distance;

		//enter titan
		if (Input.GetKeyDown(KeyCode.E) && TitanAccess == true) 
		{
			ScriptT.Activate = true;
			GameObject.Find ("Titan Camera").GetComponent<Camera>().depth = 5;
			Interact.gameObject.SetActive (false);
			Player.SetActive(false);
		}
		//get's mouse input and rotates camera around X-axis
		mouseY = -Input.GetAxis ("Mouse Y");
		transform.Rotate (mouseY * sensitivity * Time.deltaTime, 0, 0);

		//Zoom in
		if (Input.GetMouseButtonDown(1)) 
		{
			//set's camera's depths and shows UI
			cameraMain.depth = 0;
			cameraAim.depth = 1;
			BPM.gameObject.SetActive (true);
		}
		//Zoom out
		if (Input.GetMouseButtonUp (1)) 
		{
			cameraAim.depth = 0;
			cameraMain.depth = 1;
			BPM.gameObject.SetActive (false);
			ZoomIn = false;
			ZoomOut = false;
			Time.timeScale = 1;

			//set's camera's FOV (zoom)
			cameraAim.fieldOfView = 6.5f;
		}
		//fires bullet (spawns it)
		if (Input.GetMouseButtonDown (0) && ammo > 0 && reload == false && kill == false) 
		{
			Instantiate (bullet, gunEnd.position, gunEnd.rotation);
			ammo -= 1;
			GunReload = 1f;
			reload = true;
			clip.text = "Clip: " + ammo.ToString ();
		}
		//zooms in if heart rate is below 60
		if (Input.GetKeyDown (KeyCode.E) && cameraAim.depth == 1 && heartBeat < 60) 
		{
			Time.timeScale = 0.5f;
			ZoomIn = true;
		}
		//zooms out if heart rate is above 60
		if (ZoomIn == true && heartBeat >= 60) 
		{
			Time.timeScale = 1;
			ZoomIn = false;
			ZoomOut = true;
		}
	}
	void FixedUpdate()
	{
		Ray ray = new Ray (gunEnd.transform.position, transform.forward);

		RaycastHit hit;
		//titan detect
		if (Physics.Raycast (ray, out hit, 5)) 
		{
			//if ray hits titan and player is less then 1.99 away
			if (hit.collider.gameObject.tag == "Titan" && Distance < 1.99f) 
			{
				//enables access and displays text
				TitanAccess = true;
				Interact.gameObject.SetActive (true);
				Interact.text = "Press E to Enter";
			} 
			//if ray hits titan and player is more then 1.99 away
			else if (hit.collider.gameObject.tag == "Titan" && Distance > 1.99f) 
			{
				//disable access (too far)
				TitanAccess = false;
				Interact.gameObject.SetActive (false);
			} 
			//if ray doesn't hit titan
			else if (hit.collider.gameObject.tag != "Titan")
			{
				//disable access (looking away)
				Interact.gameObject.SetActive (false);
				TitanAccess = false;
			}
		}
		//if the position is the same
		if (LastPos == transform.position) 
		{
			//lowers heart rate (not moving)
			if (heartBeat >= 56 && Time.timeScale == 1) 
			{
				heartBeat =heartBeat - 0.1f;

				//updates heart rate
				BPM.text = heartBeat.ToString();
			}
			//resets last position
			LastPos = transform.position;
		}
		//if the position is different
		if (LastPos != transform.position) 
		{
			//increases heart rate (moving)
			if (heartBeat <=115 && Time.timeScale == 1) 
			{
				heartBeat = heartBeat + 0.1f;

				BPM.text = heartBeat.ToString();
			}
			//resets last position
			LastPos = transform.position;
		}
		//if time is slowed (zoom in)
		if (Time.timeScale == 0.5f) 
		{
			heartBeat = heartBeat + 0.05f;

			BPM.text = heartBeat.ToString();
		}

		//checks if player is reloading
		if (reload == true) 
		{
			//subtracts Gunreload by Time.DeltaTime (1 second)
			GunReload -= Time.deltaTime;

			//if current camera is Main then enable the text associated with it
			if(cameraMain.depth == 1 && GunReload > 0)
			{
				ReloadM.gameObject.SetActive (true);
				ReloadA.gameObject.SetActive (false);
			}

			//if current camera is Aim then enable the text associated with it
			if (cameraAim.depth == 1 && GunReload > 0) 
			{
				ReloadA.gameObject.SetActive (true);
				ReloadM.gameObject.SetActive (false);
			}  
			//resets reload
			if (GunReload <= 0) 
			{
				GunReload = 1f;
				ReloadM.gameObject.SetActive (false);
				ReloadA.gameObject.SetActive (false);
				reload = false;
			}
		}
		//full reload
		else if (reloadA == true && reload == false && kill == false) 
		{
			GunReload = 3;
			ammo = 5;
			clip.text = "Clip: " + ammo.ToString ();
			reload = true;
			reloadA = false;
		}
		//slowly increases zoom (fieldofView) until it hits 2.5
		if (ZoomIn == true && cameraAim.fieldOfView > 2.5f) 
		{
			cameraAim.fieldOfView = cameraAim.fieldOfView - 0.1f;
		}
		//slowly decreases zoom (fieldofView) until it hits 2.5
		if (ZoomOut == true && cameraAim.fieldOfView < 6.5f) 
		{
			cameraAim.fieldOfView = cameraAim.fieldOfView + 0.1f;
		}
	}
}
