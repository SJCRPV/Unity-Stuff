﻿using UnityEngine;
using System.Collections;

public class EnemySpawn : MonoBehaviour {

	GameObject[] enemyInstance = new GameObject[50];
	//Importing the LevelDatabase script.
	static LevelDatabase levelInfoScript;

	private Vector3 startPosition;

	//Flag that tells you if it's currently lerping or not
	private bool[] isLerping = new bool[50];
	//Had as some sort of limiter. Might be useless
	private bool[] stopLerping = new bool[50];
	//Time since the instance started lerping. Used to tell how close to the end it is
	private float[] timeLerpStart = new float[50];
	//End position
	private Vector3[] endVector = new Vector3[50];
	private float[] percentageComplete = new float[50];
	//How much it's added to the end position for the ship's position
	private Vector3 addOn;
	//It's what gets multiplied by a fixed ammount when deciding the ship's position
	private int looper;
	//Holds the value of the array at the index of the current phase
	private int phaseTotal;
	
	public float speed = 1f;
	public int health = 3;
	//How long (1.0f = 1 second) should the lerp take?
	public float lerpTime = 1.0f;
	//Timer that ticks down until you run SpawnEnemy() again.
	public float newSpawnTimer = 5.0f;
	public GameObject enemyPrefab;

	public bool isItLerping(int i)
	{
		return isLerping[i];
	}
	
	void StartLerping () {
		//Setting all the info for the Lerp
		for (int i = 0; i < phaseTotal; i++) {
			isLerping [i] = true;
			percentageComplete [i] = 0f;
			timeLerpStart [i] = Time.time;
			startPosition = transform.position;
			if (i < phaseTotal/2) {
				endVector [i] = GameObject.Find ("EnemyEndPoint1").transform.position + addOnDeterminer ();
			}
			else
			{
				endVector [i] = GameObject.Find ("EnemyEndPoint2").transform.position - addOnDeterminer ();
			}
		}
	}

	Vector3 addOnDeterminer(){
		looper++;
		if (looper > phaseTotal) 
		{
			looper = 0;
		}
		addOn = new Vector3 (0.8f * looper, 0, 0);
		
		return addOn;
	}
	
	void ChangePosition()
	{
		for(int i = 0; i < phaseTotal; i++)
		{
			if (isLerping[i])
			{
				percentageComplete[i] = timeLerpStart[i]/lerpTime;
				enemyInstance[i].transform.position = Vector3.Lerp(startPosition, endVector[i], percentageComplete[i]);

				if(percentageComplete[i] >= lerpTime)
				{
					isLerping[i] = false;
					stopLerping[i] = true;
				}
			}
		}
	}

	void whichLevel()
	{
		switch(levelInfoScript.currentLevel)
		{
		case 1:
			levelInfoScript.Level1 ();
			phaseTotal = levelInfoScript.levelArray[levelInfoScript.currentLevelPhase];
			break;
		default:
		Debug.Log("Error!");
		Debug.Log(levelInfoScript.levelArray.Length);
		Debug.Log(levelInfoScript.currentLevel);
		Debug.Log(levelInfoScript.currentLevelPhase);
		break;
		}
	}

	void SpawnEnemy()
	{
		//This is temporary. Remove after implementing phase and level timers.
		//levelInfoScript.Level1 ();

		whichLevel ();
	
		//What you want is for the loop to go through each of the phase's lenght
		for (int i = 0; i < phaseTotal; i++) 
		{
			//This creates an enemy!
			enemyInstance[i] = (GameObject)Instantiate (enemyPrefab, transform.position, Quaternion.identity);
		}
	}

	void setLerps()
	{
		for (int i = 0; i < isLerping.Length; i++) {
			isLerping[i] = false;
			stopLerping [i] = false;
		}
	}

	void newPhase()
	{
		if(levelInfoScript.currentLevelPhase > levelInfoScript.levelArray.Length)
		{
			levelInfoScript.currentLevel++;
			levelInfoScript.currentLevelPhase = 0;
			Debug.Log("New level!");
			Debug.Log (phaseTotal = levelInfoScript.levelArray[levelInfoScript.currentLevelPhase]);
		}
		else
		{
			Debug.Log("New Phase!");
			levelInfoScript.currentLevelPhase++;
			Debug.Log(phaseTotal = levelInfoScript.levelArray[levelInfoScript.currentLevelPhase]);
			newSpawnTimer = 5.0f;
		}
	}

	// Use this for initialization
	void Start () {
		setLerps ();
		levelInfoScript = GetComponent<LevelDatabase> ();
		levelInfoScript.currentLevel = 1;
		SpawnEnemy();
	}

	// Update is called once per frame
	void Update () {
		StartLerping();
		ChangePosition();

		//Run spawn enemy again after a certain time!
		newSpawnTimer -= Time.deltaTime;
		if(newSpawnTimer <= 0)
		{
			newPhase();
			SpawnEnemy();
		}
	}


}