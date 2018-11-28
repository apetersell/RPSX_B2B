using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifespan : MonoBehaviour {

    public float lifespan;
    float timer;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        timer++;
        if (timer >= lifespan)
        {
            Destroy(this.gameObject);
        }
	}

    public void Die()
    {
        Destroy(this.gameObject);
    }
}
