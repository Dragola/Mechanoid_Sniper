using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detection : MonoBehaviour 
{
	public Titan ScriptT;
	public Vector3 enemy;

	void Start()
	{
		ScriptT = GetComponent<Titan> ();
	}
	//if trigger
	void OnTriggerEnter (Collider col)
	{
		//enemy detected
		if (col.gameObject.tag == "Enemy") 
		{
			enemy = col.transform.position;
			ScriptT.enemyPos = enemy;
			ScriptT.engage = true;
		} 
	}
}
