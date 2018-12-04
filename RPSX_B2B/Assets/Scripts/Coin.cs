using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

    public RPS_State myState;
    public Sprite[] sprites;
    SpriteRenderer sr; 

	// Use this for initialization
	void Start () 
    {
        sr = GetComponent<SpriteRenderer>();
        if (myState == RPS_State.Rock)
        {
            sr.sprite = sprites[0];
            sr.color = RPSX.rockColor;
        }
        if (myState == RPS_State.Paper)
        {
            sr.sprite = sprites[1];
            sr.color = RPSX.paperColor;
        }
        if (myState == RPS_State.Scissors)
        {
            sr.sprite = sprites[2];
            sr.color = RPSX.scissorsColor;
        }
    }
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.GetComponent<Player>() != null)
        {
            Player player = coll.gameObject.GetComponent<Player>();
            if (!player.Dizzy() && !player.Graced())
            {
                player.ChangeRPSState(myState);
            }
        }
    }
}
