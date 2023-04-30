using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletO : MonoBehaviour 
{
	Titan scriptT;
	float wait = 10;
	//moves bullet
	void Awake ()
	{
		GetComponent<Rigidbody>().AddForce (transform.forward * 600);
	}
	//references (depending on the prefab)
	void Start () 
	{
		scriptT = GameObject.Find ("Titan").GetComponent<Titan> ();
	}
	//timer
	void FixedUpdate()
	{
		wait -= 1 * Time.deltaTime;
	}
	//timer check
	void Update()
	{
		if (wait <= 0) 
		{
			Destroy (gameObject);
		} 
	}
	//when bullet hits an object
	void OnTriggerEnter (Collider other)
	{
		if (name == "BulletE(Clone)") 
		{
			if (other.tag == "Player") 
			{
				Player player = GameObject.Find ("Player").GetComponent<Player> ();
				player.health -= 1;
				player.regenT = 5;
				Destroy (gameObject);
			} 
			else if (other.tag == "Enemy") 
			{
				other.GetComponent<Enemy> ().health -= 1;
				if (scriptT.titanfall < 300)
				{
					scriptT.titanfall -= 10;
				}
				Destroy (gameObject);
			} 
		}
		else if (name == "BulletS(Clone)") 
		{
			if (other.tag == "Titan") 
			{
				scriptT.health -= 1;
				Destroy (gameObject);
			} 
		}
	}
}
