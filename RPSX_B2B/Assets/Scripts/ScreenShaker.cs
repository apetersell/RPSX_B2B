using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShaker : MonoBehaviour {

    public int shakeTimer;
    public float shakeAmount;
    public Vector2 shakePos;
    public Vector2 pos;

    // Use this for initialization
    void Start ()
    {
        pos = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () 
    {
        //Test with
        if (Input.GetKeyDown(KeyCode.Space))
        {
            shake(20);
        }

        if (shakeTimer < 0)
        {
            shakeTimer = 0;
        }

        if (shakeTimer > 0)
        {
            shakeTimer--;
            shakePos = Random.insideUnitCircle * shakeAmount;
        }
        else
        {
            shakePos = Vector2.zero;
        }
        transform.position = new Vector3(pos.x + shakePos.x, pos.y + shakePos.y, transform.position.z);
    }

    public void shake(int duration)
    {
        shakeTimer = duration;
    }
}
