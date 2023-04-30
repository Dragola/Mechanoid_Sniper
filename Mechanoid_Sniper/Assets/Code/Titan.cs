using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Titan : MonoBehaviour 
{
	public Transform ArmEndR;
	public Transform ArmEndL;
	public GameObject Body;
	GameObject ArmR;
	public GameObject ArmL;
	public GameObject Bull; 
	public GameObject Heart;
	public float Distance;
	float TitanSpeed = 2f;
	public bool Activate = false; 
	Vector3 PPosition;
	float mouseX;
	float mouseY;
	float sensitivity;
	Player ScriptP;
	GameObject player;
	Gun ScriptG;
	public Camera TitanEye;
	bool set = false;
	bool setAI = false;
	bool fallen = false;
	public bool grounded = false;
	bool position = false;
	public Text titanFall;
	public float titanfall = 300;
	Vector3 tLanding;
	Transform titanExit;
	public bool engage= false;
	public Vector3 enemyPos;
	int ammo = 60;
	float reload = 4;
	bool guardM = false;
	bool boost = false;
	float boostCool = 4;
	public float health = 50;
	Text warning;
	Text thealth;
	int direction;

	// Use this for initialization
	void Start () 
	{
		//reference code
		ScriptP = GameObject.Find ("Player").GetComponent<Player>();
		ArmEndR = GameObject.Find ("ArmEnd(R)").GetComponent<Transform>();
		ArmEndL = GameObject.Find ("ArmEnd(L)").GetComponent<Transform>();
		Body = GameObject.Find ("Body");
		ArmL = GameObject.Find ("Arm(L)");
		ArmR = GameObject.Find ("Arm(R)");
		TitanEye = GameObject.Find ("Titan Camera").GetComponent<Camera>();
		Heart = GameObject.Find ("UI");
		ScriptG = GameObject.Find ("Ksvk").GetComponent<Gun>();
		GetComponent<Rigidbody> ().useGravity = false;
		titanFall = GameObject.Find ("TitanFall").GetComponent<Text> ();
		player = GameObject.Find ("Player");
		titanExit = GameObject.Find ("Titan Exit").GetComponent<Transform> ();
		warning = GameObject.Find ("Warning").GetComponent<Text> ();
		thealth = GameObject.Find ("Thealth").GetComponent<Text> ();
		warning.gameObject.SetActive (false);
	}

	// Update is called once per frame8
	void Update () 
	{
		//Gets mouse position
		mouseX = Input.GetAxis ("Mouse X");
		mouseY = -Input.GetAxis ("Mouse Y");

		//set's values
		sensitivity = ScriptP.sensitivity;
		
		//finds distance
		Distance = Vector3.Distance (Body.transform.position, transform.position);
		
		//player's position
		PPosition = Body.GetComponent<Transform>().position;

		//critical health
		if (health <= 10) 
		{
			warning.gameObject.SetActive (true);
		}

		//titan ready
		if (titanfall <= 0 && fallen == false) 
		{
			titanFall.text = "Titanfall: Ready";
		} 

		//AI mode
		if (Activate == false && grounded == true && guardM == false) 
		{
			//looks at player
			transform.LookAt (PPosition);

			//locks Titan rotation, not needed
			transform.rotation = Quaternion.Euler (0, transform.rotation.eulerAngles.y, 0);

			//moves towards player faster (further away)
			if (Vector3.Angle (transform.forward, PPosition) >= 0.9f && Distance > 6.99f) {
				TitanSpeed = 4;
				transform.Translate (0, 0, TitanSpeed * Time.deltaTime);
			}
			//moves towards player slowly
			else if (Vector3.Angle (transform.forward, PPosition) >= 0.9f && Distance > 9.99f) {
				TitanSpeed = 2;
				transform.Translate (0, 0, TitanSpeed * Time.deltaTime);
			} 
			else if (Distance <= 6.99f) 
			{

			}
		}

		//AI mode
		if (engage == true && grounded == true && Activate == false) 
		{
			transform.LookAt (enemyPos);

			//locks Titan rotation, not needed
			transform.rotation = Quaternion.Euler (0, transform.rotation.eulerAngles.y, 0);

			//set's titan's arms (attacking)
			if (setAI == false) 
			{
				if (set == false) 
				{
					ArmL.transform.Rotate (-90, 0, 0);
					ArmR.transform.Rotate (-90, 0, 0);
				}
				TitanEye.transform.Rotate (0, 0, 0);
				setAI = true;
			}
			//shoots if there is ammo
			if (ammo > 0) 
			{
				Instantiate (Bull, ArmEndL.position, ArmEndL.rotation);
				Instantiate (Bull, ArmEndR.position, ArmEndR.rotation);
				ammo -= 1;
			}
		}

		//while inside the titan
		if (Activate == true) 
		{
			if (mouseX != 0)
			{
				//rotates the camera on the Y-axis
				transform.Rotate(0, mouseX + sensitivity * Time.deltaTime, 0);
			}

			//sets the arm and camera's angles and hides/shows UI
			if (set == false)
			{
				if (setAI == false) 
				{
					ArmL.transform.Rotate (-90, 0, 0);
					ArmR.transform.Rotate (-90, 0, 0);
				}
				titanFall.gameObject.SetActive (false);
				thealth.gameObject.SetActive (true);
				set = true;
			}
			//vertical movement for arms and camera
			if (mouseY != 0)
			{
				ArmL.transform.Rotate(mouseY + sensitivity * Time.deltaTime, 0, 0);
				ArmR.transform.Rotate(mouseY + sensitivity * Time.deltaTime, 0, 0);
				TitanEye.transform.Rotate(mouseY + sensitivity * Time.deltaTime, 0, 0);
			}
		}

		//drops titan
		if (Input.GetKeyDown (KeyCode.V) && fallen == false && titanfall <= 0) 
		{
			position = true;
		} 
		//guard mode activvation
		else if (Input.GetKeyDown (KeyCode.V) && grounded == true && guardM == false) 
		{
			guardM = true;
			titanFall.text = "Titan Mode: Guarding";
		}
		else if (Input.GetKeyDown (KeyCode.V) && grounded == true && guardM == true) 
		{
			guardM = false;
			titanFall.text = "Titan Mode: Following";
		}

		//exit titan
		if (Input.GetKeyDown (KeyCode.E) && Activate == true) 
		{
			ScriptG.TitanAccess = false;
			TitanEye.depth = 0;
			Heart.gameObject.SetActive(true);
			player.SetActive (true);
			player.transform.position = titanExit.position;
			Activate = false;
		}

		//player's inside
		if (Activate == true) 
		{
			thealth.text = "Health :" + health.ToString ();
			//moves forward
			if (Input.GetKey(KeyCode.W) && Activate == true) 
			{
				//translates/moves titan forward
				transform.Translate (0, 0, 2 * Time.deltaTime);
				direction = 0;
			}
			//moves back
			if (Input.GetKey(KeyCode.S) && Activate == true) 
			{
				transform.Translate (0, 0, -2 * Time.deltaTime);
				direction = 1;
			}
			//moves left
			if (Input.GetKey(KeyCode.A) && Activate == true) 
			{
				transform.Translate (-2 * Time.deltaTime, 0, 0);
				direction = 2;
			}
			//moves right
			if (Input.GetKey(KeyCode.D) && Activate == true) 
			{
				transform.Translate (2 * Time.deltaTime, 0, 0);
				direction = 3;
			} 
			if (Input.GetMouseButton(0)) 
			{
				if (ammo > 0) 
				{
					Instantiate (Bull, ArmEndL.position, ArmEndL.rotation);
					Instantiate (Bull, ArmEndR.position, ArmEndR.rotation);
					ammo -= 1;
				}
			}
			//zoomes in
			if (Input.GetMouseButtonDown(1)) 
			{
				TitanEye.fieldOfView = 60;
			}
			//zoomes out
			else if (Input.GetMouseButtonUp(1))
			{
				TitanEye.fieldOfView = 90;
			}
			//boost
			if (Input.GetKeyDown(KeyCode.Space) && boost == false) 
			{
				boost = true;
				if (direction == 0) 
				{
					transform.Translate (0, 0, 10);
				}
				else if (direction == 1) 
				{
					transform.Translate (0, 0, -10);
				}
				else if (direction == 2) 
				{
					transform.Translate (-10, 0, 0);
				}
				else if (direction == 3) 
				{
					transform.Translate (10, 0, 0);
				}
			}
		}
		//sets titan's position and drops
		if (position == true) 
		{
			//players forward position (looking)
			Vector3 playerForward = player.transform.forward;
			//places titan on the player's position 
			tLanding = player.transform.position + playerForward * 20;
			tLanding.y = 30;
			this.transform.position = tLanding;
			fallen = true;
			GetComponent<Rigidbody> ().useGravity = true;
			position = false;
		}

		//set titanfall timer to 0 (immediate titan)
		if (Input.GetKeyDown(KeyCode.Keypad9))
		{
			titanfall = 0;
		}
	}
	void FixedUpdate()
	{
		if (health <= 0) 
		{
			float eject = 5;
			eject -= 1f * Time.deltaTime;
			
			if (eject <= 0 && player.activeInHierarchy == false) 
			{
				ScriptP.health = 0;
			} 
			else 
			{
				transform.position = new Vector3 (0, 50, 0);
				health = 50;
				grounded = false;
				Activate = false;
				fallen = false;
				engage = false;
			}
		}

		//boost recharge
		if (boost == true) 
		{
			if (boostCool > 0) 
			{
				boostCool -= 1 * Time.deltaTime;
			} 
			else if (boostCool <= 0) 
			{
				boostCool = 4;
				boost = false;
			}
		}

		//reload
		if (ammo <= 0)
		{
			reload -= 1 * Time.deltaTime;
			if (reload <= 0) 
			{
				reload = 4;
				ammo = 60;
			}
		}

		//countdown timer (titanfall)
		if (titanfall >= 0 && grounded == false) 
		{
			titanfall -= Time.deltaTime;
			titanFall.text = "Titanfall: " + titanfall.ToString (); 
		}

		//detects distance from ground
		if (fallen == true) 
		{
			titanFall.text = "TitanFall: Inbound";
			//detect titan distance from ground
			RaycastHit hit;
			if (Physics.Raycast (transform.position, -Vector3.up, out hit, 100f)) 
			{
				if (hit.distance < 1.36f && grounded == false) 
				{
					grounded = true;
					fallen = false;
					GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezePositionY;
					titanFall.text = "Titan Mode: Follow";
					titanfall = 300;
				}		
			}
		}
	}

}
