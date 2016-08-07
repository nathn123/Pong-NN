﻿using UnityEngine;
using System.Collections;

public class TimeScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown(KeyCode.P))
        {
            Time.timeScale *= 2;
            Debug.Log("Speed Up");
        }
		else if (Input.GetKeyDown(KeyCode.L))
        {
            Time.timeScale /= 2;
            Debug.Log("Speed Down");
        }
	
	}
}
