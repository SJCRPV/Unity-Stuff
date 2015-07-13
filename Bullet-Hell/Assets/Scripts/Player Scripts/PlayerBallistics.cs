﻿using UnityEngine;
using System.Collections;

public class PlayerBallistics : MonoBehaviour {
	
	PlayerSpawn playerSpawnScript;

	public GameObject bulletPrefab;
	public float cooldownTimerStore;
	float cooldownTimer;
	Vector3 verticalOffset = new Vector3(0, 0.5f, 0);
	Vector3 horizontalOffset = new Vector3(0.5f, 0, 0);
	Vector3 bulletPosition;
	
	private GameObject bulletInstance;
	private Transform objectParent;
	private GameObject extraBullerSource1;
	private GameObject extraBullerSource2;
	private GameObject extraBullerSource3;
	private GameObject extraBullerSource4;

	void fire()
	{
		bulletInstance = (GameObject)Instantiate(bulletPrefab, transform.position + verticalOffset, Quaternion.identity);
		cooldownTimer = cooldownTimerStore;
		bulletInstance.gameObject.layer = 10;
	}
	void firePattern()
	{
		fire();

	}

	// Use this for initialization
	void Start () {
		cooldownTimer = cooldownTimerStore;
		playerSpawnScript = GetComponentInParent<PlayerSpawn>();
	}
	
	// Update is called once per frame
	void Update () {
		cooldownTimer -= Time.deltaTime; 

		if(cooldownTimer <= 0 && Input.GetButton("Fire1"))
		{
			firePattern();
		}
	}
}