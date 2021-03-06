﻿using UnityEngine;
using System.Collections;
using System;

public class Character_StandardEnemy : Character
{
    //CONSIDER: You might not need this script
    //BlockInteraction blockInteractionScript;

    private float invincibilityTimeStore;
    private GameObject blockInstance;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (getInvencibilityTime() <= 0)
        {
            //Debug.Log("Ow! ; _ ;");
            decreaseHealth();
            setInvencibilityTime(invincibilityTimeStore);
        }
    }

    public override void explode()
    {
        for (int i = 0; i < 10; i++)
        {
            switch (i)
            {
                case 0:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 9:
                    blockInstance = Instantiate(getPointBlock(), transform.position, Quaternion.identity);
                    break;

                case 1:
                case 8:
                    blockInstance = Instantiate(getPowerBlock(), transform.position, Quaternion.identity);
                    break;
                default:
                    Debug.LogError("Invalid number. I don't know what block to create with this. 'Tried to resolve the case for " + i);
                    break;
            }
            blockInstance.GetComponent<Rigidbody2D>().AddForceAtPosition(new Vector2(-100f + i * 40, 150), transform.position);
        }
        die();
    }

    // Use this for initialization
    protected new void Start()
    {
        base.Start();
        invincibilityTimeStore = getInvencibilityTime();
    }

    void Update()
    {
        decrementInvincibilityTime(Time.deltaTime);
        if (getCurrentHealth() <= 0)
        {
            explode();
        }
    }
}
