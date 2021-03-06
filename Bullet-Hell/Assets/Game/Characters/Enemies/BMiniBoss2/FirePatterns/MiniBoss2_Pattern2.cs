﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MiniBoss2_Pattern2 : MonoBehaviour, IFire {

    [SerializeField]
    private GameObject slicePrefab;
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private float cooldownTimer;
    [SerializeField]
    private float shotgunBulletSpread;
    [SerializeField]
    private float timeBetweenShells;
    [SerializeField]
    private int shotgunAngleSpread;
    [SerializeField]
    private int bulletsPerShotgunShot;

    private GameObject bulletInstance;
    private GameObject sliceInstance;
    private Movement_Generic genericMovementScript;
    private Movement_Boss bossMovementScript;
    private float cooldownTimerStore;
    private int startingShotgunAngle;

    private void setBulletPath(iTweenPath path, int incrementMult)
    {
        //Debug.Log("This is path: " + path.pathName);
        //Debug.Log("This is incrementMult " + incrementMult);
        //Debug.Log("This is shotgunBulletSpread " + shotgunBulletSpread);
        path.nodes[0] = transform.position;
        path.nodes[1] = new Vector3(transform.position.x, transform.position.y + (shotgunBulletSpread * incrementMult / 4), transform.position.z);
        Vector3[] temp = { path.nodes[0], path.nodes[1] };
        iTween.MoveTo(bulletInstance, iTween.Hash("path", temp, "speed", bulletInstance.GetComponent<MoveForward>().getBulletSpeed() / 4, "easetype", iTween.EaseType.easeOutQuart));
    }

    private void fireShotgun(Vector3 newEulerAngles)
    {
        Quaternion tempRot = Quaternion.identity;
        tempRot.eulerAngles = newEulerAngles;
        bulletInstance = Instantiate(bulletPrefab, transform.position, tempRot);
    }

    private void adjustStartingAngle(int counter)
    {
        switch (counter)
        {
            case 0:
                startingShotgunAngle -= shotgunAngleSpread;
                break;
            case 1:
                startingShotgunAngle += shotgunAngleSpread * 2;
                break;
            case 2:
                startingShotgunAngle -= shotgunAngleSpread;
                break;
            default:
                Debug.LogError("Unexpected number. I received " + counter);
                break;
        }
    }

    private IEnumerator unloadShell()
    {
        int counter = 0;
        while (counter < 3)
        {
            iTweenPath[] path = new iTweenPath[30];
            float tempSpread = shotgunBulletSpread;
            //Debug.Log("tempSpread is: " + tempSpread);
            for (int i = 0, bulletCounter = 0; i < bulletsPerShotgunShot; i++)
            {
                //Right
                fireShotgun(new Vector3(0, 0, startingShotgunAngle + shotgunAngleSpread));
                path[bulletCounter] = bulletInstance.GetComponent<iTweenPath>();
                path[bulletCounter].pathName = path[bulletCounter].pathName + bulletCounter;
                setBulletPath(path[bulletCounter++], i);

                //Left
                fireShotgun(new Vector3(0, 0, startingShotgunAngle - shotgunAngleSpread));
                path[bulletCounter] = bulletInstance.GetComponent<iTweenPath>();
                path[bulletCounter].pathName = path[bulletCounter].pathName + bulletCounter;
                setBulletPath(path[bulletCounter++], i);

                //Front
                fireShotgun(new Vector3(0, 0, startingShotgunAngle));
                path[bulletCounter] = bulletInstance.GetComponent<iTweenPath>();
                path[bulletCounter].pathName = path[bulletCounter].pathName + bulletCounter;
                setBulletPath(path[bulletCounter++], i);

                shotgunBulletSpread = tempSpread;
            }

            adjustStartingAngle(counter++);
            yield return new WaitForSeconds(timeBetweenShells);
        }
        StopCoroutine("unloadShell");
    }

    private IEnumerator wiggleToPos(Vector3 target)
    {
		Vector3[] targetPath = new Vector3[3];
		Vector3 distance = transform.position - target;
		targetPath[0] = transform.position;
		targetPath[1] = transform.position + new Vector3(distance.x/2, distance.y*2, distance.z/2);
		targetPath[2] = target;
		float pathCompletePercent = 0;
		while(pathCompletePercent < 1)
		{
			iTween.PutOnPath(gameObject, targetPath, pathCompletePercent);
			pathCompletePercent += 0.1f;
			yield return null;
		}
    }

    private IEnumerator fireSlice(bool inverted)
    {
        //TODO: Make the boss wiggle left and right with each slice using the wiggleToPos function
        int counter = 0;
        while (counter < 3)
        {
            Vector3 temp = transform.position;
            temp.y -= 1;
            Quaternion tempRot = Quaternion.identity;
            if (inverted)
            {
                tempRot.eulerAngles = new Vector3(0, 0, 50);
            }
            else
            {
                tempRot.eulerAngles = new Vector3(0, 0, -50);
            }

            sliceInstance = Instantiate(slicePrefab, temp, tempRot);
            Debug.Log("inverted is: " + inverted);
            if (inverted)
            {
                sliceInstance.GetComponent<ExplodeDownwards>().setInverted();
                //StartCoroutine("wiggleToPos", )
            }

            counter++;
            inverted = !inverted;
            yield return new WaitForSeconds(0.5f);
        }
        StopCoroutine("fireSlice");
    }

    public void firePattern()
    {
        //Debug.Log(bossMovementScript.getCurrentNodeTrioInUse());

        //If at the frontmost position
        //IDEA: Have the slices *slowly* move downwards before exploding. That way you can keep instantiating the slices in front of the boss
        if(bossMovementScript.getCurrentNodeTrioInUse() == 2)
        {
            //Debug.Log("FireFireFire!");
            StartCoroutine("fireSlice", false);
        }
        else
        {
            //Debug.Log("Shotguuuuun Blast!");
            //Debug.Log("startingShotgunAngle is: " + startingShotgunAngle);
            //Debug.Log("shotgunAngleSpread is: " + shotgunAngleSpread);
            StartCoroutine("unloadShell", startingShotgunAngle);
        }
    }

    public void assignMovement()
    {
        genericMovementScript = gameObject.GetComponentInParent<Movement_Generic>();
        bossMovementScript = gameObject.GetComponentInParent<Movement_Boss>();
        if(genericMovementScript == null)
        {
            Debug.LogError("genericMovementScript is empty. Did not get the component from parent");
        }
        if(bossMovementScript == null)
        {
            Debug.LogError("bossMovementScript is empty. Did not get the component from parent");
        }
    }

    // Use this for initialization
    void Start () {
        cooldownTimerStore = cooldownTimer;
        assignMovement();
        startingShotgunAngle = 180;
	}
	
	// Update is called once per frame
	void Update () {
        if (genericMovementScript.getIsMoving() == false && bossMovementScript.getIsMoving() == false)
        {
            cooldownTimer -= Time.deltaTime;
        }
        else
        {
            cooldownTimer = cooldownTimerStore;
        }

        if (cooldownTimer <= 0)
        {
            firePattern();
            cooldownTimer = cooldownTimerStore;
        }
    }
}
