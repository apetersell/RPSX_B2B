using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPSEffect : MonoBehaviour {

    public Sprite[] sprites;
    public RPS_State state;
    SpriteRenderer sr;
	// Use this for initialization
	void Start () 
    {
        sr = GetComponent<SpriteRenderer>();
	}

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case RPS_State.Rock:
                sr.sprite = sprites[0];
                break;
            case RPS_State.Paper:
                sr.sprite = sprites[1];
                break;
            case RPS_State.Scissors:
                sr.sprite = sprites[2];
                break;
        }
    }
}
