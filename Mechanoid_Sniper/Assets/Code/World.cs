using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class World : MonoBehaviour 
{
	public GameObject Target;
	public GameObject soldier;
	int wave = 0;
	int spawnS = 0;
	int spawnSS = 0;
	int numberS = 5;
	int numberSS = 2;
	float countdown = 10;
	int count = 0;
	bool waveN = false;
	Transform[] spawn; 
	string SceneCurrent;
	Text waveT;

	// Use this for initialization
	void Start () 
	{
		waveT = GameObject.Find ("Wave").GetComponent<Text> ();
		waveT.text = "Wave: " + wave.ToString (); 
		spawn = new Transform[4];
		Scene current = SceneManager.GetActiveScene ();
		SceneCurrent = current.name;

        spawn[0] = GameObject.Find("Spawn1").GetComponent<Transform>();
        spawn[1] = GameObject.Find("Spawn2").GetComponent<Transform>();
        spawn[2] = GameObject.Find("Spawn3").GetComponent<Transform>();
        spawn[3] = GameObject.Find("Spawn4").GetComponent<Transform>();

        //spawns targets
        if (SceneCurrent == "Training") 
		{
			Instantiate (Target, spawn [0].position, spawn [0].rotation); 
			Instantiate (Target, spawn [1].position, spawn [1].rotation); 
			Instantiate (Target, spawn [2].position, spawn [2].rotation); 
			Instantiate (Target, spawn [3].position, spawn [3].rotation); 
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
        if (SceneCurrent == "Endless Mode #1") 
		{
			//spawn super soldier
			if (wave >= 5 && spawnSS > 1) 
			{
                int rdm = Random.Range(0, 4);
                if (rdm == 0) 
				{
					Instantiate(soldier, spawn [0].position, spawn [0].rotation); 
				} 
				else if (rdm == 1) 
				{
					Instantiate(soldier, spawn [1].position, spawn [1].rotation); 
				}
				else if (rdm == 2) 
				{
					Instantiate(soldier, spawn [2].position, spawn [2].rotation); 
				} 
				else if (rdm == 3) 
				{
					Instantiate(soldier, spawn [3].position, spawn [3].rotation); 
				}
				spawnSS -= 1;
			}
			//soldier spawning
			if (wave >= 0 && spawnS > 0) 
			{
                int rdm = Random.Range(0, 4);
                if (rdm == 0) {
					Instantiate(soldier, spawn [0].position, spawn [0].rotation); 
				} 
				else if (rdm == 1) 
				{
					Instantiate(soldier, spawn [1].position, spawn [1].rotation); 
				} 
				else if (rdm == 2) 
				{
					Instantiate(soldier, spawn [2].position, spawn [2].rotation); 
				} 
				else if (rdm == 3) 
				{
					Instantiate(soldier, spawn [3].position, spawn [3].rotation); 
				}
				spawnS -= 1;
			}
			//wave refresh initiate
			if (count == 0 && waveN == false) 
			{
				waveN = true;
			}
		}
	}

	void FixedUpdate()
	{
		//enemy count
		count = GameObject.FindGameObjectsWithTag("Enemy").Length;

		//wave refresh
		if (waveN == true && count == 0) 
		{
            countdown -= 1 * Time.deltaTime; 
			waveT.text = "Wave: Starting in " + countdown.ToString();
			
			//sets wave
			if (countdown <= 0 && wave >= 1) 
			{
				wave += 1;
				spawnS += numberS;
				numberS += 5;
				countdown = 10;
				waveT.text = "Wave: " + wave.ToString();

				//super soldier add
				if (wave >= 5) 
				{
					spawnSS += numberSS + 1; 
				}
				waveN = false;
			} 
			//wave 1 set
			else if (countdown <= 0 && wave == 0) 
			{
				spawnS = 5;
				countdown = 10;
				waveT.text = "Wave: 1";
				waveN = false;
			}
		}
	}
}
