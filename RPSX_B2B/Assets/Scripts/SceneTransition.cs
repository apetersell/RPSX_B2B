using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour {

    public int SceneToLoad;
    public bool canLoadScene;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetButtonDown("Start_P1") || Input.GetButton("Start_P2"))
        {
            if (canLoadScene)
            {
                SceneManager.LoadScene(SceneToLoad);
            }
        }
	}
}
