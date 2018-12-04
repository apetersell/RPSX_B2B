using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMoveset : MonoBehaviour {

    GameObject hitBoxes;
    Attack[] attacks;
    Player player;

	// Use this for initialization
	void Start () 
    {
        player = GetComponent<Player>();
        hitBoxes = transform.GetChild(4).gameObject;
        attacks = new Attack[hitBoxes.transform.childCount];
        for (int i = 0; i < attacks.Length; i++)
        {
            Attack attack = hitBoxes.transform.GetChild(i).gameObject.GetComponent<Attack>();
            attacks[i] = attack;
        }
        player.burstComponent = attacks[11];
    }
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    public Attack GetAttack (string sent)
    {
        Attack attack = null;

        switch (sent)
        {
            case "GroundForward":
                attack = attacks[0];
                break;
            case "StrongGroundForward":
                attack = attacks[1];
                break;
            case "AirForward":
                attack = attacks[2];
                break;
            case "StrongAirForward":
                attack = attacks[3];
                break;
            case "GroundUp":
                attack = attacks[4];
                break;
            case "StrongGroundUp":
                attack = attacks[5];
                break;
            case "AirUp":
                attack = attacks[6];
                break;
            case "StrongAirUp":
                attack = attacks[7];
                break;
            case "DashAttack":
                attack = attacks[8];
                break;
            case "AirDown":
                attack = attacks[9];
                break;
            case "GroundDown":
                attack = attacks[10];
                break;
        }

        return attack;
    }
}
