using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {

    public Player owner;
    public RPS_State myState;
    public float baseKnockback; //How much knockback does the attack inflict.
    public Vector2 baseKnockbackAngle; //At what angle does the attack send the player.
    public List<Player> playersHit = new List<Player>(); //List of players hit by the attack.  Used so that the hit player doesn't get hit multiple times.
    public float directionMod;
    public float stickInputX;
    public float stickInputY;
    public float KBInfluenceX;
    public float KBInfluenceY;
    public static float winKnockbackGrownth = 1.25f;
    public int dizzyDamage;

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
        float effectiveKB = baseKnockback;
        RPS_Result result = RPSX.determineWinner(myState, player.currentState);
        float knockbackX = (baseKnockbackAngle.x + (stickInputX * KBInfluenceX)) * directionMod;
        float knockbackY = baseKnockbackAngle.y + (stickInputY * KBInfluenceY);
        Vector2 effectiveKBA = new Vector2(knockbackX, knockbackY);
        switch (result)
        {
            case RPS_Result.Tie:
                effectiveKB = baseKnockback;
                break;
            case RPS_Result.Win:
                effectiveKB = baseKnockback * winKnockbackGrownth;
                PlayerManager.TakeDizzy(dizzyDamage, player.playerNum);
                break;
        }
        if (result != RPS_Result.Loss)
        {
            if (!player.Invincible())
            {
                player.TakeHit(effectiveKBA, effectiveKB);
            }
        }
        else 
        {
            GameObject parrySpark = Instantiate(Resources.Load("Prefabs/ParrySpark")) as GameObject;
            parrySpark.transform.position = player.gameObject.transform.position;
            parrySpark.GetComponent<SpriteRenderer>().color = RPSX.StateColor(player.currentState);
            player.Parry(effectiveKBA);
            PlayerManager.RecoverDizzy(dizzyDamage, player.playerNum);
            owner.Stagger(effectiveKBA * -1, baseKnockback * 0.5f);
        }
    }
}
