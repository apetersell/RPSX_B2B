using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenManager : MonoBehaviour {

    public Vector2[] possibleTokenLocations;
    public List<int> availableSlots = new List<int>();


	// Use this for initialization
	void Start () 
    {
        for (int i = 0; i < possibleTokenLocations.Length; i++)
        {
            availableSlots.Add(i);
            Transform t = GameObject.Find("TokenSconce_" + i).gameObject.transform;
            Vector2 pos = new Vector2(t.position.x, t.position.y + 0.6f);
            possibleTokenLocations[i] = pos;
        }
        SpawnToken(RPS_State.Rock, possibleTokenLocations.Length + 1);
        SpawnToken(RPS_State.Paper, possibleTokenLocations.Length + 1);
        SpawnToken(RPS_State.Scissors, possibleTokenLocations.Length + 1);
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    public void SpawnToken(RPS_State state, int previousSlot)
    {
        if (previousSlot < possibleTokenLocations.Length + 1)
        {
            availableSlots.Add(previousSlot);
        }
        GameObject newToken = Instantiate(Resources.Load("Prefabs/Token")) as GameObject;
        Coin coin = newToken.GetComponent<Coin>();
        coin.myState = state;
        coin.manager = this;
        int rando = Random.Range(0, availableSlots.Count);
        availableSlots.Remove(rando);
        newToken.transform.position = possibleTokenLocations[rando]; 
    }
}
