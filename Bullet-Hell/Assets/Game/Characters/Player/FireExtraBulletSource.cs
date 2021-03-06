﻿using UnityEngine;
using System.Collections;

public class FireExtraBulletSource : MonoBehaviour {

	public GameObject bulletPrefab;
	public float cooldownTimerStore;
	float cooldownTimer;
	Vector3 bulletPosition;
	
	private GameObject bulletInstance;

	void fire()
	{
		bulletInstance = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
		cooldownTimer = cooldownTimerStore;
		bulletInstance.gameObject.layer = 10;
	}

	public void firePattern()
	{
		fire();
	}

	// Use this for initialization
	void Start () {
		cooldownTimer = cooldownTimerStore;
		//playerSpawnScript = GetComponentInParent<PlayerSpawn>();
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
