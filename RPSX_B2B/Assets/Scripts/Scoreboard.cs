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
    Image[] portraitBG;
    SpriteCycler[] faces;
    SpriteCycler[] screenCracks;
    Image[] hearts;
    Image[] StateBG;
    public Image [] dizzyMeters;
    Image arrow;
    Text clock;
    Text[] healthDisplays;
    Color[] lerpingColors;
    Color[] dark;
    Color[] bright;
    Color[] normal;
    Color[] faded;
    Player[] players;
    public float UIFlashSpeed;

    // Use this for initialization
    void Start()
    {
        emblemDisplays = new Image[numberOfPlayers];
        stateTimers = new Image[numberOfPlayers];
        portraits = new Image[numberOfPlayers];
        portraitBG = new Image[numberOfPlayers];
        faces = new SpriteCycler[numberOfPlayers];
        screenCracks = new SpriteCycler[numberOfPlayers];
        hearts = new Image[numberOfPlayers];
        StateBG = new Image[numberOfPlayers];
        dizzyMeters = new Image[numberOfPlayers];
        healthDisplays = new Text[numberOfPlayers];
        lerpingColors = new Color[numberOfPlayers];
        dark = new Color[numberOfPlayers];
        bright = new Color[numberOfPlayers];
        normal = new Color[numberOfPlayers];
        faded = new Color[numberOfPlayers];
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
            portraits[i] = GameObject.Find("PortraitFace_P" + (i + 1)).GetComponent<Image>();
            faces[i] = GameObject.Find("PortraitFace_P" + (i + 1)).GetComponent<SpriteCycler>();
            screenCracks[i] = GameObject.Find("ScreenCrack_P" + (i + 1)).GetComponent<SpriteCycler>();
            portraitBG[i] = GameObject.Find("PortraitBG_P" + (i + 1)).GetComponent<Image>();
            hearts[i] = GameObject.Find("Heart_P" + (i + 1)).GetComponent<Image>();
            StateBG[i] = GameObject.Find("StateBG_P" + (i + 1)).GetComponent<Image>();
            dizzyMeters[i] = GameObject.Find("DizzyMeter_P" + (i + 1)).GetComponent<Image>();
            healthDisplays[i] = GameObject.Find("HealthDisplay_P" + (i + 1)).GetComponent<Text>();
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
        Image stateBG = StateBG[playerNum];
        Image portrait = portraits[playerNum];
        Image heart = hearts[playerNum];
        Image dizzyMeter = dizzyMeters[playerNum];

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
            faded[playerNum] = RPSX.rockColorFaded;
        }
        else if (player.currentState == RPS_State.Paper)
        {
            emblem.sprite = emblems[2];
            normal[playerNum] = RPSX.paperColor;
            bright[playerNum] = Color.green;
            dark[playerNum] = RPSX.paperColorDark;
            faded[playerNum] = RPSX.paperColorFaded;
        }
        else if (player.currentState == RPS_State.Scissors)
        {
            emblem.sprite = emblems[3];
            normal[playerNum] = RPSX.scissorsColor;
            bright[playerNum] = Color.red;
            dark[playerNum] = RPSX.scissorsColorDark;
            faded[playerNum] = RPSX.scissorsColorFaded;
        }
        else
        {
            emblem.sprite = emblems[0];
            normal[playerNum] = Color.white;
            bright[playerNum] = Color.white;
            dark[playerNum] = Color.white;
            faded[playerNum] = Color.white;
        }

        lerpingColors[playerNum] = Color.Lerp(bright[playerNum], dark[playerNum], Mathf.PingPong(Time.time * UIFlashSpeed, 1));

        if (result == RPS_Result.Win)
        {
            emblem.color = lerpingColors[playerNum];
            stateTimer.color = lerpingColors[playerNum];
        }
        else 
        {
            emblem.color = bright[playerNum];
            stateTimer.color = bright[playerNum];
        }
        stateBG.color = faded[playerNum];
        portraitBG[playerNum].color = faded[playerNum];
        stateTimer.color = bright[playerNum];
        portrait.color = normal[playerNum];
        heart.color = normal[playerNum];
        dizzyMeter.color = bright[playerNum];

        healthDisplays[playerNum].text = PlayerManager.healthTotals[playerNum].ToString();
        float currentFillAmount = PlayerManager.maxStateTime - player.timeInState;
        stateTimer.fillAmount = RPSX.fillAmount(currentFillAmount, PlayerManager.maxStateTime);
        if (players[playerNum].Dizzy())
        {
            dizzyMeters[playerNum].fillAmount = PlayerManager.dizzyTimers[playerNum] / PlayerManager.maxDizzyTime;
        }
        else 
        {
            dizzyMeters[playerNum].fillAmount = RPSX.fillAmount (PlayerManager.dizzyTotals[playerNum], players[playerNum].maxDizzyHits);
        }

        if (player.Dizzy())
        {
            faces[playerNum].index = 2;
        }
        else if (player.hit)
        {
            faces[playerNum].index = 1;
        }
        else 
        {
            faces[playerNum].index = 0;
        }

        if (PlayerManager.dizzyTotals[playerNum] > 6)
        {
            screenCracks[playerNum].index = 0;
        }
        else if (PlayerManager.dizzyTotals[playerNum] <= 6 && PlayerManager.dizzyTotals[playerNum] > 4)
        {
            screenCracks[playerNum].index = 1;
        }
        else if (PlayerManager.dizzyTotals[playerNum] <= 4 && PlayerManager.dizzyTotals[playerNum] > 2)
        {
            screenCracks[playerNum].index = 2;
        }
        else if (PlayerManager.dizzyTotals[playerNum] <= 2 && PlayerManager.dizzyTotals[playerNum] > 0)
        {
            screenCracks[playerNum].index = 3;
        }
        else 
        {
            screenCracks[playerNum].index = 4;
        }
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