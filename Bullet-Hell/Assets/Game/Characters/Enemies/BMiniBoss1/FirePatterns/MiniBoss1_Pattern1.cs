﻿using UnityEngine;
using System.Collections;
using System;

public class MiniBoss1_Pattern1 : MonoBehaviour, IFire {
    [SerializeField]
	private GameObject bulletPrefab;
	[SerializeField]
    private GameObject explodingBulletPrefab;
	[SerializeField]
    private float startingDegrees;
	[SerializeField]
    private float cooldownTimer;
	[SerializeField]
    private float maxDegrees;
	[SerializeField]
    private float degreeIncreasePerIteration;
	[SerializeField]
    private float movingTimeBetweenBursts;
	[SerializeField]
    private float stillTimeBetweenBursts;

    private Movement_Generic genericMovementScript;
    private Movement_Boss bossMovementScript;
    private float cooldownTimerStore;
    private float startingDegreesStore;
    private GameObject bulletInstance;
    private Quaternion bulletRotation;

    void FireMoving()
    {
        for (; startingDegrees <= maxDegrees; startingDegrees += degreeIncreasePerIteration)
        {
            bulletRotation = Quaternion.identity;
            bulletRotation.eulerAngles = new Vector3(0, 0, startingDegrees);
            bulletInstance = Instantiate(bulletPrefab, transform.position, bulletRotation);
            bulletInstance.gameObject.layer = 11;
        }
        startingDegrees = startingDegreesStore;
        cooldownTimer = cooldownTimerStore;
    }
    void FireStill()
    {
        bulletInstance = Instantiate(explodingBulletPrefab, transform.position - Vector3.up/2 - Vector3.left/2, new Quaternion(0, 0, 180, 0));
        bulletInstance.gameObject.layer = 11;
        bulletInstance = Instantiate(explodingBulletPrefab, transform.position - Vector3.up/2 - Vector3.right/2, new Quaternion(0, 0, 180, 0));
        bulletInstance.gameObject.layer = 11;
        cooldownTimer = stillTimeBetweenBursts;
    }

    public void firePattern()
    {
        if(bossMovementScript.getIsMoving() == true && genericMovementScript.getIsMoving() == false)
        {
            FireMoving();
            Invoke("FireMoving", movingTimeBetweenBursts);
            Invoke("FireMoving", movingTimeBetweenBursts * 2);
        }
        else
        {
            FireStill();
        }
    }

    public void assignMovement()
    {
        genericMovementScript = gameObject.GetComponentInParent<Movement_Generic>();
        bossMovementScript = gameObject.GetComponentInParent<Movement_Boss>();
    }

    // Use this for initialization
    void Start()
    {
        startingDegreesStore = startingDegrees;
        cooldownTimerStore = cooldownTimer;
        assignMovement();
    }

    // Update is called once per frame
    void Update()
    {
        if (genericMovementScript.getIsMoving() == false)
        {
            cooldownTimer -= Time.deltaTime;
        }

        if (cooldownTimer <= 0)
        {
            firePattern();
        }
    }
}
