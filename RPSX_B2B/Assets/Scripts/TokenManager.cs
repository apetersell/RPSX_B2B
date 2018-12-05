using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenManager : MonoBehaviour {

    public float maxX;
    public float minX;
    public float maxY;
    public float minY;

	// Use this for initialization
	void Start () 
    {
        SpawnToken(RPS_State.Rock);
        SpawnToken(RPS_State.Paper);
        SpawnToken(RPS_State.Scissors);
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    public void SpawnToken(RPS_State state)
    {
        GameObject newToken = Instantiate(Resources.Load("Prefabs/Token")) as GameObject;
        Coin coin = newToken.GetComponent<Coin>();
        coin.myState = state;
        coin.manager = this;
        float posX = Random.Range(minX, maxX);
        float posY = Random.Range(minY, maxY);
        Vector2 startingLocation = new Vector2(posX, posY);
        newToken.transform.position = startingLocation; 
    }
}
