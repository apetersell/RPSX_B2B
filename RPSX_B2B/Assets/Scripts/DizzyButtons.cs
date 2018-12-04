using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DizzyButtons : MonoBehaviour {

    public int playerNum;
    Animator anim;
    float scaleX;
    public Player myPlayer;
	// Use this for initialization
	void Start () 
    {
        anim = GetComponent<Animator>();
        scaleX = transform.localScale.x;
	}
	
	// Update is called once per frame
	void Update () 
    {
        transform.localScale = new Vector3(scaleX * myPlayer.directionMod, transform.localScale.y, transform.localScale.z);
        anim.SetBool("CanUndizzy", CanUndizzy());
	}

    bool CanUndizzy()
    {
        return PlayerManager.dizzyTimers[playerNum - 1] >= PlayerManager.maxDizzyTime || myPlayer.Graced();
    }
}
