using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObstacleManager : MonoBehaviour {

	public List<GameObject> spawnPoints = new List<GameObject> ();
	public GameObject obstacle_1;
	public GameObject obstalce_Big_1;
	public GameObject obstacle_Middle_1;
	public GameObject obstacle_Big_Ring_1;

	public bool spawning;
	public GameObject midSpawn;

	// Use this for initialization
	void Start () 
	{
		spawning = true;
		CharCollison.OnCollision += StopSpawning;

		foreach (Transform child in transform) 
		{
			GameObject obj = child.gameObject;
			spawnPoints.Add (obj);
		}

		StartCoroutine ("Level1");
	}

	void StopSpawning(string charNumb)
	{
		spawning = false;
	}

	IEnumerator Level1()
	{
		while (spawning) 
		{
			yield return new WaitForSeconds (GameManager.Instance.spawnTime);
			int tempIns = Random.Range (0, spawnPoints.Count);

			int randomNr = Random.Range (0, 12);
				
				if (spawnPoints [tempIns].gameObject.tag == "MidSpawn") 
				{
				Instantiate (obstalce_Big_1, spawnPoints [tempIns].gameObject.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));
				}

			else if (randomNr == 1 || randomNr == 2 || randomNr == 3) 
			{
				Instantiate (obstacle_Middle_1, midSpawn.gameObject.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));
			}

			else if(randomNr == 4)
				Instantiate (obstacle_Big_Ring_1, midSpawn.gameObject.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));
			else
			Instantiate (obstacle_1, spawnPoints [tempIns].gameObject.transform.position, spawnPoints [tempIns].gameObject.transform.rotation);
		}
	}

	// Update is called once per frame
	void Update () 
	{
		
	}
}
