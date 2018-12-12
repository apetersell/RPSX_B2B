using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvantageArrow : MonoBehaviour {

    public int playerNum;
    Transform owner;
    Transform opponent;
    Transform GreenRoom;
    SpriteRenderer sr;
    float minDistance = 2f;

	// Use this for initialization
	void Start () 
    {
        GreenRoom = GameObject.Find("GreenRoom").transform;
        sr = GetComponent<SpriteRenderer>();
        opponent = GameObject.Find("ARMS_P2").transform;                                   
	}
	
	// Update is called once per frame
	void Update () 
    {
        FindPlayers();
        Vector3 dir = (opponent.transform.position - transform.position).normalized;
        float dist = Vector3.Distance(opponent.position, owner.position); 
        if (dist > minDistance)
        {
            transform.position = owner.position + dir;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else 
        {
            transform.position = GreenRoom.position;
        }
    }

    void FindPlayers()
    {
        RPS_Result result = RPSX.determineWinner(PlayerManager.players[0].currentState, PlayerManager.players[1].currentState);
        if (result == RPS_Result.Win)
        {
            owner = PlayerManager.players[0].transform;
            opponent = PlayerManager.players[1].transform;
            sr.color = RPSX.StateColor(PlayerManager.players[0].currentState);

        }
        else if (result == RPS_Result.Loss)
        {
            owner = PlayerManager.players[1].transform;
            opponent = PlayerManager.players[0].transform;
            sr.color = RPSX.StateColor(PlayerManager.players[1].currentState);
        }
        else
        {
            owner = GreenRoom;
        }
    }
}
