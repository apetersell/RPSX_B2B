using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

    public RPS_State myState;
    public Sprite[] sprites;
    public TokenManager manager;
    SpriteRenderer sr;
    Animator anim;
    SoundManager sm;
    bool taken;
    float takenTimer;
    float timeAfterTaken = 1.5f;
    public int mySlot;


	// Use this for initialization
	void Start () 
    {
        sm = GameObject.Find("Manager").GetComponent<SoundManager>();
        anim = GetComponent<Animator>();
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
        if(taken)
        {
            takenTimer += Time.deltaTime;
            if (takenTimer >= timeAfterTaken)
            {
                Die();
            }
        }
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.GetComponent<Player>() != null)
        {
            Player player = coll.gameObject.GetComponent<Player>();
            if (!player.Dizzy() && !player.Graced())
            {
                player.ChangeRPSState(myState);
                anim.SetTrigger("Get");
                taken = true;
                SoundManager.PlaySound(sm.TokenGet);

            }
        }
    }

    void Die ()
    {
        manager.SpawnToken(myState, mySlot);
        Destroy(this.gameObject);
    }

    public void Shine ()
    {
        GameObject parrySpark = Instantiate(Resources.Load("Prefabs/ParrySpark")) as GameObject;
        parrySpark.transform.position = transform.position;
        parrySpark.GetComponent<SpriteRenderer>().color = RPSX.StateColor(myState);
        SoundManager sm = GameObject.Find("Manager").GetComponent<SoundManager>();
        SoundManager.PlaySound(sm.CoinDink);
    }
}
