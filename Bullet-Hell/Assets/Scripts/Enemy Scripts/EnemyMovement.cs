﻿using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {

	SpawnEnemies spawnEnemiesScript;
	Ballistics ballisticsScript;
	ConePattern conePatternScript;
	GrazePattern grazePatternScript;
	Boss1Ballistics boss1PatternScript;

	public float speed;
	public float timerUntilObjectLeaves;
	public bool isMoving;
	public bool leftTheStage;
	public bool isShooting;

	private Vector3 startingPosition;
	private Vector3 endPosition;
	private Transform spawnPoint1;
	private Transform spawnPoint2;
	private Transform leavingPoint1;
	private Transform leavingPoint2;
	private Transform endPoint1;
	private Transform endPoint2;
	private float timerUntilObjectLeavesStore;
	private float endPosAdjustment;
    private Rigidbody2D rigidBody;
	private string pathName;


	
	public Vector3 adjustmentToEndPosition()
	{
		endPosAdjustment = spawnEnemiesScript.endPosAdjustment;
		Vector3 addOn;

		//Have the endPosition depend on whether the current phase is even or pair
		if(spawnEnemiesScript.levelDatabaseScript.currentLevelPhase % 2 == 0)
		{
			addOn = new Vector3( 1f * endPosAdjustment, 0, 0);
			//Debug.Log("addOn was given the value of: " + addOn);
		}
		else
		{
			addOn = new Vector3( 1f * endPosAdjustment + 1, 1, 0);
			//Debug.Log("addOn was given the value of: " + addOn);
		}
		
		spawnEnemiesScript.endPosAdjustment++;
		return addOn;
	}

	private void swapShootingStatus()
	{
		isShooting = !isShooting;
		if(gameObject.tag == "Basic")
		{
			ballisticsScript.enabled = isShooting;
		}
		else if(gameObject.tag == "Cone")
		{
			conePatternScript.enabled = isShooting;
		}
		else if(gameObject.tag == "Graze")
		{
			grazePatternScript.enabled = isShooting;
		}
		else if(gameObject.tag == "Boss1")
		{
			boss1PatternScript.enabled = true;
		}
	}

    //private void moveObject()
    //{
    //    startingPosition = transform.position;
    //    if(transform.position != endPosition)
    //    {
    //        transform.position = Vector3.MoveTowards(startingPosition, endPosition, speed * Time.deltaTime);
    //        isShooting = true;
    //        swapShootingStatus();
    //    }
    //    else
    //    {
    //        isMoving = false;
    //        swapShootingStatus();
    //    }
    //}

//	Vector3 desired;
//	Vector3 steering;
//	Vector3 velocity;
//
//	private Vector3 truncateVector(Vector3 vector)
//	{
//		vector.x = (float)System.Math.Round(vector.x, 1);
//		vector.y = (float)System.Math.Round(vector.y, 1);
//		vector.z = (float)System.Math.Round(vector.z, 1);
//
//		return vector;
//	}
//
//	private Vector3 seek(Vector3 target)
//	{
//		Vector3 force;
//
//		desired = target - transform.position;
//		desired.Normalize();
//		desired *= speed;
//
//		force = desired - velocity;
//		return force;
//	}
//
//    private void moveObject()
//    {
//		//StartingPosition is StartingPosition
//		steering = seek(endPosition);
//
//		velocity += steering;
//		//velocity = truncateVector(velocity);
//
//		if(transform.position != endPosition)
//	    {
//			//Debug.Log(steering);
//			rigidBody.AddForce(velocity);
//			//transform.position += velocity;
//			//transform.position = startingPosition;
//	        isShooting = true;
//	        swapShootingStatus();
//	    }
//	    else
//	    {
//	        isMoving = false;
//	        swapShootingStatus();
//	    }
//    }

//	float distance;
//	float distanceStore;
//	float relativePercentage;
//	Vector3 distanceVector;
//	Vector3 force;
//
//	private void storeDistance()
//	{
//		distance = Vector3.Distance(startingPosition, endPosition);
//		distanceStore = distance;
//	}
//
//	private Vector3 seek()
//	{
//		Vector3 temp;
//
//		temp = distanceVector + Vector3.up;
//		temp.Normalize();
//		temp *= speed;
//		return temp;
//	}
//
//	private void moveObject()
//	{
//		startingPosition = transform.position;
//		distance = Vector3.Distance(startingPosition, endPosition);
//		distanceVector = endPosition - startingPosition;
//		force = seek();
//
//		if(distance >= 10)
//	    {
//			rigidBody.AddForce(force);
//			//rigidBody.AddForce(force - force * relativePercentage);
//	        isShooting = true;
//	        swapShootingStatus();
//	    }
//		else if(distance == 0)
//		{
//			isMoving = false;
//			swapShootingStatus();
//		}
//	    else
//	    {
//			Debug.Log("Hi!");
//			rigidBody.velocity = Vector3.zero;
//			rigidBody.angularVelocity = 0;
//			transform.position = Vector3.MoveTowards(transform.position, endPosition, speed*Time.deltaTime);
//			isShooting = true;
//			swapShootingStatus();
//	    }
//	}




	private void moveObject()
	{
		iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath(pathName), "time", speed, "easetype", iTween.EaseType.linear));
	}

	private void whichComponentsToGet()
	{
		if(gameObject.tag == "Basic")
		{
			//Debug.Log("Got a basic here!");
			ballisticsScript = GetComponent<Ballistics>();
			ballisticsScript.enabled = false;
		}
		else if(gameObject.tag == "Cone")
		{
			//Debug.Log("Got a cone here!");
			conePatternScript = GetComponent<ConePattern>();
			conePatternScript.enabled = false;
		}
		else if(gameObject.tag == "Graze")
		{
			//Debug.Log("Got a graze here!");
			grazePatternScript = GetComponent<GrazePattern>();
			grazePatternScript.enabled = false;
		}
		else if(gameObject.tag == "Boss1")
		{
			//Debug.Log("Got a boss here!");
			boss1PatternScript = GetComponent<Boss1Ballistics>();
			boss1PatternScript.enabled = false;
		}
        else
        {
            Debug.LogError("I don't know what component to get!");
        }
	}

	private void whereTo()
	{
		isMoving = true;
		if(gameObject.transform.position == spawnPoint1.position)
		{
			endPosition = endPoint1.position - adjustmentToEndPosition();
			pathName = "EnemySpawnPath1";
			//Debug.Log(gameObject.name + " is moving to: " + endPosition);
			//Debug.Log("isMoving is " + isMoving);
		}
		else if(gameObject.transform.position == spawnPoint2.position)
		{
			endPosition = endPoint2.position + adjustmentToEndPosition();
			pathName = "EnemySpawnPath2";
			//Debug.Log(gameObject.name + " is moving to: " + endPosition);
			//Debug.Log("isMoving is " + isMoving);
		}
		else if(gameObject.transform.parent == GameObject.Find("BossSpawnPoint").transform)
		{
			endPosition = GameObject.Find("BossEndPoint").transform.position;
			//Debug.Log(gameObject.name + " is moving to: " + endPosition);
			//Debug.Log("isMoving is " + isMoving);
		}
		else
		{
			Debug.LogError("I didn't find my destination!");
		}
	}

	private void backTo()
	{
		isMoving = true;
		leftTheStage = true;
		//This is dirty. See if you can find a way to fix it
		if(transform.position.y == endPoint1.position.y || transform.position.y == endPoint1.position.y - 1)
		{
			endPosition = leavingPoint1.position;
		}
		if (transform.position.y == endPoint2.position.y || transform.position.y == endPoint2.position.y + 1)
		{
			endPosition = leavingPoint2.position;
		}
		//Debug.Log(gameObject.name + " is returning to " + endPosition);
	}

	// Use this for initialization
	void Start () 
	{
		startingPosition = transform.position;
		spawnEnemiesScript = GetComponentInParent<SpawnEnemies>();
		spawnPoint1 = GameObject.Find("EnemySpawnPoint1").transform;
		spawnPoint2 = GameObject.Find("EnemySpawnPoint2").transform;
		leavingPoint1 = GameObject.Find("LeavingPoint1").transform;
		leavingPoint2 = GameObject.Find("LeavingPoint2").transform;
		endPoint1 = GameObject.Find("EnemyEndPoint1").transform;
		endPoint2 = GameObject.Find("EnemyEndPoint2").transform;
        rigidBody = GetComponent<Rigidbody2D>();
		timerUntilObjectLeavesStore = timerUntilObjectLeaves;
		isShooting = false;
		whichComponentsToGet();
		whereTo();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(leftTheStage == true && isMoving == false)
		{
			Destroy(gameObject);
		}
		if(isMoving)
		{
			moveObject();
		}
		else
		{
			timerUntilObjectLeaves -= Time.deltaTime;
			
			if(timerUntilObjectLeaves <= 0)
			{
				backTo();
				timerUntilObjectLeaves = timerUntilObjectLeavesStore;
			}
		}
	}
}