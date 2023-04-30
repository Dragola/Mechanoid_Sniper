using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour 
{
	public Rigidbody bullet;
	public Camera BullCam;
	public Camera BullCamF;
	public Camera BullCamS;
	public Camera BullCamSM;
	public GameObject BulletCameraR;
	public GameObject BulletCameraSM;
	float BullDistance;
	int CameraSelect = 0;
	float life = 10;
	bool RandomS = false;
	bool RandomK = false;
	bool Slowmo = false;
	Gun ScriptG;

	// Use this for initialization
	void Start ()
	{
		//references script
		ScriptG = GameObject.Find("Ksvk").GetComponent<Gun> ();

		//gets component
		bullet = GetComponent<Rigidbody>();

		//adds a force of 1000 forward (z-axis)
		bullet.AddForce(transform.forward * 1000);
	}
	void Update()
	{
		//in case head shot misses
		if (life <= 0)
		{
			ScriptG.kill = false;
		}
	}
	void FixedUpdate()
	{
		//decrease life 
		life -= Time.deltaTime;

		//ray (origin, destination)
		Ray rayPos = new Ray(transform.position, transform.forward);

		//collects info on object hit
		RaycastHit detect;

		//raycast
		if (Physics.Raycast(rayPos, out detect, 100)) 
		{
			//stores distance
			BullDistance = detect.distance;

			//if bullet is going to hit enemies head
			if (BullDistance >= 25 && RandomS == false && (detect.collider.CompareTag("Head") || detect.collider.CompareTag("Center")))
			{
				//randomizes a number between 0 and 1
				CameraSelect = Random.Range(0, 3);
				ScriptG.kill = true;

				//slows time (code)
				Time.timeScale = 0.5f;

				//camera view -front view
				if (CameraSelect == 0) {
					ForwardView();
				}
				//camera view -back view
				else if (CameraSelect == 1) 
				{
					BackView();
				} 
				//camera view -rotation view
				else if (CameraSelect == 2) 
				{
					RotateView();
				}
				RandomS = true;
			}
			//when the bullet is close the the enemies head
			else if (BullDistance < 4 && (detect.collider.CompareTag("Head") || detect.collider.CompareTag("Center")))
            {
                //runs slowMo
                if (RandomK == false) 
				{
					SlowMo();
				}
				RandomK = true;
				ScriptG.kill = true;
			}
		}
	}

	//when bullet hits object with collision
	void OnCollisionEnter(Collision col)
	{
		Time.timeScale = 1;

		//enemy
		if (col.gameObject.CompareTag("Enemy") || col.gameObject.CompareTag("Head")) {
			//set's hit (from other script) to true and destroys bullet
			ScriptG.kill = false;
			col.gameObject.GetComponent<Enemy>().health -= 1;

			Destroy (gameObject);
		} 
		//target
		else if (col.gameObject.CompareTag("Target")) 
		{
			Destroy(gameObject);
		}
		//if bullet hits anything else
		else 
		{
			ScriptG.kill = false;
			//destroys bullet
			Destroy(gameObject);
		}
	}
	//sets bool and adds force (slower)
	void SlowMo()
	{
		//changes camera's
		if (Slowmo == false) 
		{
			//changes camera depth
			BullCamS.depth = 0;
			BullCamF.depth = 0;
			BullCam.depth = 0;
			BullCamSM.depth = 3;

			//stops bullet
			bullet.velocity = Vector3.zero;

			//adds a force of 110 forward (z-axis)
			bullet.AddForce(transform.forward * 110);

			Slowmo = true;
		}
	}
	//changes camera view to forward
	void ForwardView()
	{
		BullCamF.depth = 3;
	}
	//changes camera view to behind
	void BackView()
	{
		BullCamS.depth = 3;
	}
	//changes camera view to rotate
	void RotateView()
	{
		BullCam.depth = 3; 

		//moves camera around the bullet in a circular motion
		BulletCameraR.transform.RotateAround (bullet.gameObject.transform.position, Vector3.up, 90 * Time.deltaTime);
	}
}
