using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Player : MonoBehaviour 
{
	public Camera cameraMain;
	public float movex;
	public float movez;
	float mouseX;
	float mouseY;
	public float sensitivity = 25f;
	Titan ScriptT;
	Gun ScriptG;
	Bullet ScriptB;
	bool Sprint = false;
	public float health = 10;
	public float regenT = 5;
	bool jump = false;
	Text healthUI;
	Scene current;
	// Use this for initialization
	void Start () 
	{
		//references scripts
		ScriptT = GameObject.Find ("Titan").GetComponent<Titan> ();
		ScriptG = GameObject.Find ("Ksvk").GetComponent<Gun>();
		healthUI = GameObject.Find ("Thealth").GetComponent<Text> ();

		// set's the main camera
		cameraMain = Camera.main;

		//makes the cursor invisible
		Cursor.visible = false;

		//locks the mouse (doesn't click something off screen)
		Cursor.lockState = CursorLockMode.Locked;

		//sets text
		healthUI.text = "Health: 10";
		current = SceneManager.GetActiveScene();
		//makes titan instant
		if (current.name == "Training") 
		{
			ScriptT.titanfall = 0;
		}
	}
	// Update is called once per frame
	void Update () 
	{
		//if escape is hit
		if (Input.GetKey (KeyCode.Escape)) 
		{
			//closes game
			Application.Quit();
		}

		//Gets mouse position
		mouseX = Input.GetAxis ("Mouse X");
		mouseY = -Input.GetAxis ("Mouse Y");

		if (mouseX != 0)
		{
			//rotates the camera on the Y-axis
			transform.Rotate(0, mouseX + sensitivity * Time.deltaTime, 0);
		}

		if (mouseY != 0)
		{
			//rotaes the camera ont he X-axis 
			cameraMain.transform.Rotate(mouseY + sensitivity * Time.deltaTime, 0, 0);
		}

        //sprint enabled
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Sprint = true;
        }

        //sprint disabled
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            Sprint = false;
        }
        
        //move forward
        if (Input.GetKey(KeyCode.W) && ScriptG.kill == false)
        {
            if (Sprint == true)
            {
                //sets float (movement speed)
                movez = 4f * Time.deltaTime;
            }
            else if (Sprint == false)
            {
                movez = 2f * Time.deltaTime;
            }
            //translates player on the Z-axis
            transform.Translate(0, 0, movez);
        }

        //move back
        if (Input.GetKey(KeyCode.S) && ScriptG.kill == false)
        {
            if (Sprint == true)
            {
                movez = -4f * Time.deltaTime;
            }
            else if (Sprint == false)
            {
                movez = -2f * Time.deltaTime;
            }
            transform.Translate(0, 0, movez);
        }

        //move left
        if (Input.GetKey(KeyCode.A) && ScriptG.kill == false)
        {
            if (Sprint == true)
            {
                movex = -4f * Time.deltaTime;
            }
            else if (Sprint == false)
            {
                movex = -2f * Time.deltaTime;
            }
            transform.Translate(movex, 0, 0);
        }

        //move right
        if (Input.GetKey(KeyCode.D) && ScriptG.kill == false)
        {
            if (Sprint == true)
            {
                movex = 4f * Time.deltaTime;
            }
            else if (Sprint == false)
            {
                movex = 2f * Time.deltaTime;
            }
            transform.Translate(movex, 0, 0);
        }

        //jump
        if (Input.GetKeyDown(KeyCode.Space) && jump == false)
        {
            GetComponent<Rigidbody>().AddForce(transform.up * 120);
            jump = true;
        }

        //reset jump
        if (this.transform.position.y <= 0.090)
        {
            jump = false;
        }

        //reload
        if (Input.GetKey(KeyCode.R) && ScriptG.kill == false)
        {
            ScriptG.reloadA = true;
            ScriptG.GunReload = 3;
        }

        //GameOver
        if (health <= 0)
        {
            SceneManager.LoadScene("GameOver");
        }

        //loads survival
        if (Input.GetKeyDown(KeyCode.P) && current.name == "Training")
        {
            SceneManager.LoadScene("Endless Mode #1");
        }
    }
	void FixedUpdate()
	{
		//health regen
		if (health < 10) 
		{
			if (ScriptT.Activate == false) 
			{
				healthUI.text = "Health: " + health.ToString ();
			}
			if (regenT > 0) 
			{
				regenT -= 1 * Time.deltaTime;
			}
			else if (regenT <= 0) 
			{
				health += 0.1f * Time.deltaTime;
			}
		} 
		else 
		{
			regenT = 5;
		}
	}
}