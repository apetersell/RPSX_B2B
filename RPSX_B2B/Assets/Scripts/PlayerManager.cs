using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour {

    //Universal respawn values.
    public static int numberOfPlayers = 2;
    public static float respawnTime = 3f;
    public static float respawnInvincibility;
    public static Vector3 deadzone = new Vector3(15f, 15f, 0f);

    //Player values
    public int maxHealth = 10;
    public static float maxStateTime = 10;
    public static int maxDizzyTime = 8;
    public static int[] healthTotals;
    public static int[] dizzyTotals;
    public static float[] dizzyTimers;
    public static float[] stateTimerTotals;
    public static Player[] players;

    // Use this for initialization
	void Start () 
    {
        healthTotals = new int[numberOfPlayers];
        dizzyTotals = new int[numberOfPlayers];
        dizzyTimers = new float[numberOfPlayers];
        players = new Player[numberOfPlayers];
        for (int i = 0; i < numberOfPlayers; i++)
        {
            players[i] = GameObject.Find("ARMS_P" + (i + 1)).GetComponent<Player>();
        }
        for (int i = 0; i < numberOfPlayers; i++)
        {
            healthTotals[i] = maxHealth;
            dizzyTotals[i] = players[i].maxDizzyHits;
        }
    }
	
	// Update is called once per frame
	void Update () 
    {
        foreach (Player p in players)
        {
            int playerNum = p.playerNum - 1;
            if (p.Dizzy())
            {
                dizzyTimers[playerNum] += Time.deltaTime;
                if (dizzyTimers[playerNum] >= maxDizzyTime)
                {
                    dizzyTotals[playerNum] = players[playerNum].maxDizzyHits;
                    dizzyTimers[playerNum] = 0;
                }
            }
        }
    }

    public static void TakeDamage (int playerNum)
    {
        healthTotals[playerNum]--;
    }

    public static void TakeDizzy (int damage, int playerNum)
    {
        if (dizzyTotals[playerNum - 1] - damage >= 0)
        {
            dizzyTotals[playerNum - 1] -= damage;
        }
        else 
        {
            dizzyTotals[playerNum -1] = 0;
        }
    }

    public static void RecoverDizzy (int damage, int playerNum)
    {
        int maxDizzy = players[playerNum - 1].maxDizzyHits;
        if (dizzyTotals[playerNum -1] + damage < maxDizzy)
        {
            dizzyTotals[playerNum - 1] += damage;
        }
        else 
        {
            dizzyTotals[playerNum - 1] = maxDizzy;
        }
    }
}
