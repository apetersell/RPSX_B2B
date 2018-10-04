using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {

    Player owner;
    public float baseKnockback; //How much knockback the damage does.
    public Vector2 baseKnockbackAngle; //At what angle does the attack send the player.
    public List<Player> playersHit = new List<Player>(); //List of players hit by the attack.  Used so that the hit player doesn't get hit multiple times.
    public float directionMod;
    public float stickInputX;
    public float stickInputY;
    public float KBInfluenceX;
    public float KBInfluenceY;
    public static float winKnockbackGrownth = 1.5f;
    public static float loseKnockbackGrowth = 0.25f;

    // Use this for initialization
    void Start () 
    {
		
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    public virtual void OnTriggerEnter2D(Collider2D coll)
    {
        //Handles collisions with players.
        if (coll.gameObject.GetComponent<Player>() != null)
        {
            Player playerHit = coll.gameObject.GetComponent<Player>();
            if (playerHit != owner && !playersHit.Contains(playerHit))
            {
                hitPlayer(playerHit);
                playersHit.Add(playerHit);
            }
        }
    }

    void hitPlayer (Player player)
    {
        Vector2 effectiveKBA = Vector2.zero;
        float effectiveKB = baseKnockback;
        //RPS_Result result = RPSX.determineWinner(owner.currentState, player.currentState);
        //switch (result)
        //{
        //    case RPS_Result.Tie:
        //        effectiveKB = baseKnockback;
        //        break;
        //    case RPS_Result.Win:
        //        effectiveKB = baseKnockback * winKnockbackGrownth;
        //        break;
        //    case RPS_Result.Loss:
        //        effectiveKB = baseKnockback * loseKnockbackGrowth;
        //        break;
        //}
        float knockbackX = baseKnockbackAngle.x * directionMod; //* (KBInfluenceX * stickInputX) * directionMod;
        float knockbackY = baseKnockbackAngle.y; //* (KBInfluenceY * stickInputY);
        effectiveKBA = new Vector2(knockbackX, knockbackY);
        player.TakeHit(effectiveKBA, effectiveKB);
    }
}
