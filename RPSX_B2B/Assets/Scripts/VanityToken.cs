using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanityToken : MonoBehaviour {
    SpriteRenderer sr;
    public RPS_State myState;
    public Sprite[] sprites;
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

    public void Shine()
    {
        GameObject parrySpark = Instantiate(Resources.Load("Prefabs/ParrySpark")) as GameObject;
        parrySpark.transform.position = transform.position;
        parrySpark.GetComponent<SpriteRenderer>().color = RPSX.StateColor(myState);
    }
}
