using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scoreboard : MonoBehaviour
{
    public int numberOfPlayers;
    public Sprite[] emblems;
    Image[] emblemDisplays;
    Image[] stateTimers;
    Image[] portraits;
    Image[] hearts;
    public Image [] dizzyMeters;
    Image arrow;
    Text clock;
    Text[] healthDisplays;
   public Text[] dizzyDisplays;
    Color[] lerpingColors;
    Color[] dark;
    Color[] bright;
    Color[] normal;
    Player[] players;
    public float UIFlashSpeed;

    // Use this for initialization
    void Start()
    {
        emblemDisplays = new Image[numberOfPlayers];
        stateTimers = new Image[numberOfPlayers];
        portraits = new Image[numberOfPlayers];
        hearts = new Image[numberOfPlayers];
        dizzyMeters = new Image[numberOfPlayers];
        healthDisplays = new Text[numberOfPlayers];
        dizzyDisplays = new Text[numberOfPlayers];
        lerpingColors = new Color[numberOfPlayers];
        dark = new Color[numberOfPlayers]; 
        bright = new Color[numberOfPlayers]; 
        normal = new Color[numberOfPlayers];
        players = new Player [numberOfPlayers];
        GetReferences();
    }

    // Update is called once per frame
    void Update()
    {
        KeepTrackOfStates(0);
        KeepTrackOfStates(1);
        ArrowPoint();
    }

    void GetReferences()
    {
        for (int i = 0; i < numberOfPlayers; i++)
        {
            players[i] = GameObject.Find("ARMS_P" + (i + 1)).GetComponent<Player>();
            emblemDisplays[i] = GameObject.Find("Emblem_P" + (i + 1)).GetComponent<Image>();
            stateTimers[i] = GameObject.Find("StateTimer_P" + (i + 1)).GetComponent<Image>();
            portraits[i] = GameObject.Find("Portrait_P" + (i + 1)).GetComponent<Image>();
            hearts[i] = GameObject.Find("Heart_P" + (i + 1)).GetComponent<Image>();
            dizzyMeters[i] = GameObject.Find("DizzyMeter_P" + (i + 1)).GetComponent<Image>();
            healthDisplays[i] = GameObject.Find("HealthDisplay_P" + (i + 1)).GetComponent<Text>();
            dizzyDisplays[i] = GameObject.Find("DizzyDisplay_P" + (i + 1)).GetComponent<Text>();
        }
        clock = GameObject.Find("Clock").GetComponent<Text>();
        arrow = GameObject.Find("Arrow").GetComponent<Image>();
    }

    void KeepTrackOfStates(int playerNum)
    {
        Player player = players[playerNum];
        Player opponent = null;
        RPS_Result result = RPS_Result.Tie;
        Image emblem = emblemDisplays[playerNum];
        Image stateTimer = stateTimers[playerNum];
        Image portrait = portraits[playerNum];
        Image heart = hearts[playerNum];

        if (playerNum == 0)
        {
            if (numberOfPlayers > 1)
            {
                opponent = players[1];
            }
        }
        else 
        {
            opponent = players[0];
        }

        result = RPSX.determineWinner(player.currentState, opponent.currentState);

        if (player.currentState == RPS_State.Rock)
        {
            emblem.sprite = emblems[1];
            normal[playerNum] = RPSX.rockColor;
            bright[playerNum] = Color.blue;
            dark[playerNum] = RPSX.rockColorDark;
        }
        else if (player.currentState == RPS_State.Paper)
        {
            emblem.sprite = emblems[2];
            normal[playerNum] = RPSX.paperColor;
            bright[playerNum] = Color.green;
            dark[playerNum] = RPSX.paperColorDark;
        }
        else if (player.currentState == RPS_State.Scissors)
        {
            emblem.sprite = emblems[3];
            normal[playerNum] = RPSX.scissorsColor;
            bright[playerNum] = Color.red;
            dark[playerNum] = RPSX.scissorsColorDark;
        }
        else
        {
            emblem.sprite = emblems[0];
            normal[playerNum] = Color.white;
            bright[playerNum] = Color.white;
            dark[playerNum] = Color.white;
        }

        lerpingColors[playerNum] = Color.Lerp(bright[playerNum], dark[playerNum], Mathf.PingPong(Time.time * UIFlashSpeed, 1));

        if (result == RPS_Result.Win)
        {
            stateTimer.color = lerpingColors[playerNum];
        }
        else 
        {
            stateTimer.color = normal[playerNum];
        }
        emblem.color = bright[playerNum];
        portrait.color = normal[playerNum];
        heart.color = normal[playerNum];

        healthDisplays[playerNum].text = PlayerManager.healthTotals[playerNum].ToString();
        dizzyDisplays[playerNum].text = PlayerManager.dizzyTotals[playerNum].ToString();
        float currentFillAmount = PlayerManager.maxStateTime - player.timeInState;
        stateTimer.fillAmount = RPSX.fillAmount(currentFillAmount, PlayerManager.maxStateTime);
    }

    void ArrowPoint()
    {
        RPS_Result result = RPSX.determineWinner(players[0].currentState, players[1].currentState);
        if (result == RPS_Result.Win)
        {
            arrow.transform.localScale = new Vector3(1, arrow.transform.localScale.y, arrow.transform.localScale.z);
            arrow.color = lerpingColors[0];
        }
        else if (result == RPS_Result.Loss)
        {
            arrow.transform.localScale = new Vector3(-1, arrow.transform.localScale.y, arrow.transform.localScale.z);
            arrow.color = lerpingColors[1];
        }
        else 
        {
            arrow.color = Color.clear;
        }
    }
}