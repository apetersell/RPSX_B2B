using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burst_Attack : Attack
{
    SpriteRenderer sr;
	// Use this for initialization
	void Start () 
    {
        sr = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        sr.color = RPSX.StateColor(myState);
	}

    public override void hitPlayer(Player player)
    {
        Vector2 angle = (player.transform.position - owner.transform.position).normalized;
        player.TakeHit(angle, baseKnockback, RPS_Result.Tie);
    }
}
