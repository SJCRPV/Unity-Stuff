﻿using UnityEngine;
using System.Collections;

public class Boss1_Pattern1 : MonoBehaviour {

	public GameObject bulletPrefab;
	public float angleDispersion;
	public float cooldownTimer;
	public int cooldownRoundLimiter;
	public float innerCooldownTimer;

	private float angleDispersionStore;
	private GameObject bulletInstance;
	private Quaternion bulletRotation;
	private float cooldownTimerStore;
	private float innerCooldownTimerStore;
	private int cooldownRoundLimiterStore;

	public void Fire(int extra)
	{
		for (angleDispersion = 111 - extra; angleDispersion <= 249 + extra; angleDispersion += angleDispersionStore) 
		{
			bulletRotation = Quaternion.identity;
			bulletRotation.eulerAngles = new Vector3(0,0,angleDispersion);
			bulletInstance = (GameObject)Instantiate(bulletPrefab, transform.position, bulletRotation);
			bulletInstance.gameObject.layer = 11;
		}
		angleDispersion = angleDispersionStore;
	}

	public void Fire()
	{
		for (angleDispersion = 111; angleDispersion <= 249; angleDispersion += angleDispersionStore) 
		{
			bulletRotation = Quaternion.identity;
			bulletRotation.eulerAngles = new Vector3(0,0,angleDispersion);
			bulletInstance = (GameObject)Instantiate(bulletPrefab, transform.position, bulletRotation);
			bulletInstance.gameObject.layer = 11;
		}
		angleDispersion = angleDispersionStore;
	}

	void FirePattern()
	{
		innerCooldownTimer -= Time.deltaTime;
		if(innerCooldownTimer <= 0)
		{
			if(cooldownRoundLimiter == 2)
			{
				Fire(11);
			}
			else if(cooldownRoundLimiter == 1)
			{
				Fire (7);
			}
			else
			{
				Fire();
			}
			cooldownRoundLimiter--;
			innerCooldownTimer = innerCooldownTimerStore;
			if(cooldownRoundLimiter <= 0)
			{
				cooldownRoundLimiter = cooldownRoundLimiterStore;
				cooldownTimer = cooldownTimerStore;
			}
		}
	}

	// Use this for initialization
	void Start () {
		angleDispersionStore = angleDispersion;
		cooldownTimerStore = cooldownTimer;
		innerCooldownTimerStore = innerCooldownTimer;
		cooldownRoundLimiterStore = cooldownRoundLimiter;
	}
	
	// Update is called once per frame
	void Update () {
		cooldownTimer -= Time.deltaTime;
		if(cooldownTimer <= 0)
		{
			FirePattern();
		}
	}
}
