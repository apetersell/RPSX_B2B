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
    public AudioClip swingSound;
    public AudioClip normalHitSound;
    public AudioClip superHitSound;

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

    public void PlayInitSound()
    {
        owner.actionAudio.PlayOneShot(swingSound);
    }

    public virtual void hitPlayer(Player player)
    {
        if (!player.Invincible())
        {
            float effectiveKB = baseKnockback;
            RPS_Result result = RPSX.determineWinner(myState, player.currentState);
            float knockbackX = (baseKnockbackAngle.x + (stickInputX * KBInfluenceX)) * directionMod;
            float knockbackY = baseKnockbackAngle.y + (stickInputY * KBInfluenceY);
            Vector2 effectiveKBA = new Vector2(knockbackX, knockbackY);
            AudioClip clipToPlay = null;
            switch (result)
            {
                case RPS_Result.Tie:
                    effectiveKB = baseKnockback;
                    clipToPlay = normalHitSound;
                    break;
                case RPS_Result.Win:
                    effectiveKB = baseKnockback * winKnockbackGrownth;
                    clipToPlay = superHitSound;
                    if (PlayerManager.dizzyTotals[player.playerNum - 1] - dizzyDamage <= 0 && !player.Dizzy())
                    {
                        SoundManager sm = GameObject.Find("Manager").GetComponent<SoundManager>();
                        player.impactAudio.PlayOneShot(sm.KOSound);
                        player.loopingAudio.clip = sm.DizzySound;
                        player.loopingAudio.Play();
                    }
                    PlayerManager.TakeDizzy(dizzyDamage, player.playerNum);
                    break;
            }
            if (result != RPS_Result.Loss)
            {
                player.TakeHit(effectiveKBA, effectiveKB, result);
                player.impactAudio.PlayOneShot(clipToPlay);
            }
            else
            {
                GameObject parrySpark = Instantiate(Resources.Load("Prefabs/ParrySpark")) as GameObject;
                parrySpark.transform.position = player.gameObject.transform.position;
                parrySpark.GetComponent<SpriteRenderer>().color = RPSX.StateColor(player.currentState);
                if (!player.Dizzy())
                {
                    player.Parry(effectiveKBA);
                    PlayerManager.RecoverDizzy(dizzyDamage, player.playerNum);
                }
                owner.Stagger(effectiveKBA * -1, baseKnockback * 0.5f);
            }
        }
    }
}
