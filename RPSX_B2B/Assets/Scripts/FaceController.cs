using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anima2D;

public class FaceController : MonoBehaviour 
{
    SpriteMeshAnimation sma;
    Player myPlayer;
    public int PlayerNum;

	// Use this for initialization
	void Start () 
    {
        sma = GetComponent<SpriteMeshAnimation>();
        myPlayer = GameObject.Find("ARMS_P" + PlayerNum).GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (myPlayer.Dizzy())
        {
            sma.frame = 1;
        }
        else 
        {
            sma.frame = 0;
        }
	}
}
