using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetButtonDown ("Start_P1") || Input.GetButtonDown ("Start_P2"))
		{
			SceneManager.LoadScene (0);
		}
	}
}
