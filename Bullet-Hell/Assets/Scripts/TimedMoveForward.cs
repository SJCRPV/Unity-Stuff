﻿using UnityEngine;
using System.Collections;

public class TimedMoveForward : MonoBehaviour {

	public float bulletSpeed;
	public float deathTimer;
	public float moveTimer;
	
	// Update is called once per frame
	void Update () {
		moveTimer -= Time.deltaTime;
		if(moveTimer <= 0)
		{
			deathTimer -= Time.deltaTime;

			Vector3 newPos = transform.position;
			
			Vector3 velocity = new Vector3(0, bulletSpeed * Time.deltaTime, 0);
			
			newPos += transform.rotation * velocity;
			
			transform.position = newPos;
		}
		if(deathTimer <= 0)
		{
			Destroy(gameObject);
		}
	}
}
