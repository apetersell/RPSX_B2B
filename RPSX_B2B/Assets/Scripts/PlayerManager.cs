using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour {

    //Universal respawn values.
    public int numberOfPlayers;
    public static float respawnTime = 3f;
    public static float respawnInvincibility;
    public static Vector3 deadzone = new Vector3(15f, 15f, 0f);

    //Player values
    public int maxHealth = 10;
    public static float maxStateTime = 10;
    public static int[] healthTotals;
    public static float[] stateTimerTotals;
    Player[] players;

    // Use this for initialization
	void Start () 
    {
        healthTotals = new int[numberOfPlayers];
        players = new Player[numberOfPlayers];
        for (int i = 0; i < numberOfPlayers; i++)
        {
            players[i] = GameObject.Find("ARMS_P" + (i + 1)).GetComponent<Player>();
        }
        for (int i = 0; i < numberOfPlayers; i++)
        {
            healthTotals[i] = maxHealth;
        }
    }
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    public static void TakeDamage (int playerNum)
    {
        healthTotals[playerNum]--;
    }
}
