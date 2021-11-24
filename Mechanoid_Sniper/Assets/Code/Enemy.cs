using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour 
{
	public Animator anim;
	public GameObject Bullet;
	public Transform SGunEnd;
	public GameObject player;
	public GameObject titan;
	public int health = 1;
	float distanceP;
	float distanceT;
	float rate = 1;
	bool shot = false;
	Titan scriptT;
	Gun scriptG;
	bool pactive;
	// Use this for initialization
	void Start ()
	{
		anim = GetComponent<Animator> ();
		titan = GameObject.Find ("Titan");
		scriptT = GameObject.Find ("Titan").GetComponent<Titan> ();

		//if player is active
		if (scriptT.Activate == false) 
		{
			pactive = true;
			player = GameObject.Find ("Player");
			scriptG = GameObject.Find ("Ksvk").GetComponent<Gun> ();
		} 
		//if player isn't active
		else if (scriptT.Activate == true) 
		{
			pactive = false;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		//titan distance
		distanceT = Vector3.Distance (titan.transform.position, transform.position);
		if (pactive == false) 
		{
			transform.LookAt (titan.transform.position);
		}
		//player distance
		else if (pactive == true) 
		{
			distanceP = Vector3.Distance (player.transform.position, transform.position);
			transform.LookAt (player.transform.position);
		}
			//attacks
			if (distanceP <= 30 || distanceT <= 50) 
			{
				//shoot at player
				if (shot == false && pactive == true) 
				{
					//if slow-mo isn't active then shoot 
					if (scriptG.kill == false) 
					{
						anim.SetBool ("Run", false);
						anim.SetBool ("Combat", true);
						Instantiate (Bullet, SGunEnd.position, SGunEnd.rotation);
						shot = true;
					}
				}
				//shoots at titan
				else if (shot == false && pactive == false) 
				{
					anim.SetBool ("Run", false);
					anim.SetBool ("Combat", true);
					Instantiate (Bullet, SGunEnd.position, SGunEnd.rotation);
					shot = true;
				}
			}
			//idle
			else if (distanceP > 30 || distanceT > 50) 
			{
				anim.SetBool ("Combat", false);
				anim.SetBool ("Run", true);

				//change speed (slow-mo effect)
				if (scriptG.kill == true) 
				{
					transform.Translate (0, 0, 1 * Time.deltaTime);
				} 
				else 
				{
					transform.Translate (0, 0, 2 * Time.deltaTime);
				}
			}
		//dead
		if (health <= 0) 
		{
			Destroy (this.gameObject);
		}
	}
	void FixedUpdate()
	{
		//reload
		if (shot == true) 
		{
			anim.SetBool ("Combat", false);
			if (rate > 0) 
			{
				rate -= 0.1f * Time.deltaTime;
			}
			else if (rate <= 0) 
			{
				shot = false;
				rate = 1;
			}
		}
	}
}
