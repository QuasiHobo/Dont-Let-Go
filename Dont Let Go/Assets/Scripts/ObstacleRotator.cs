﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleRotator : MonoBehaviour {

	public int rotateDirection;
	public float rotateSpeed;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.Rotate(0,(rotateDirection*rotateSpeed)*Time.deltaTime,0);
	}
}
