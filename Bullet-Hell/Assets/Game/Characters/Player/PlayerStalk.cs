﻿using UnityEngine;
using System.Collections;

public class PlayerStalk : MonoBehaviour {

    GameObject player;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = player.transform.position;
	}
}