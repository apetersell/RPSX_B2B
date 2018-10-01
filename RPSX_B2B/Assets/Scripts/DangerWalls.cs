using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DangerWalls : MonoBehaviour {

    public RPS_State MyState;
    public RPS_State QueuedState;
    public SpriteRenderer Emblem;
    public Sprite[] RPSEmblems;
    int EmblemIndex;
    public SpriteRenderer body; 
    public SpriteRenderer [] colorChangers;
    Color dark;
    Color bright;
    Color saturated;
    Color lerpingColor;
    Color faded;
    bool Spinning;
    public static float rouletteTimer = 3f;
    float currentRouletteTimer;
    float spinTransitionTime = 5f;
    float spinTransitionTimer;
    public static float flashSpeed = 3f;
    public List<Player> players;

	// Use this for initialization
	void Start () 
    {
        body = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        RouletteHandler();
        Emblem.sprite = RPSEmblems[EmblemIndex];
        ColorHandle();
        CheckForPlayers();
    }

    void ColorHandle()
    {
        lerpingColor = Color.Lerp(bright, dark, Mathf.PingPong(Time.time * flashSpeed, 1));
        if (Spinning)
        {
            if (EmblemIndex == 0)
            {
                foreach (SpriteRenderer sr in colorChangers)
                {
                    sr.color = RPSX.rockColorDark;
                }
                Emblem.color = RPSX.rockColorDark;
                body.color = RPSX.rockColorDark;
            }
            if (EmblemIndex == 1)
            {
                foreach (SpriteRenderer sr in colorChangers)
                {
                    sr.color = RPSX.paperColorDark;
                }
                Emblem.color = RPSX.paperColorDark;
                body.color = RPSX.paperColorDark;
            }
            if (EmblemIndex == 2)
            {
                foreach (SpriteRenderer sr in colorChangers)
                {
                    sr.color = RPSX.scissorsColor;
                }
                Emblem.color = RPSX.scissorsColor;
                body.color = RPSX.scissorsColor;
            }
        }
        else
        {
            if (MyState == RPS_State.Rock)
            {
                dark = RPSX.rockColorDark;
                bright = RPSX.rockColor;
                faded = RPSX.rockColorFaded;
                saturated = Color.blue;
                EmblemIndex = 0;
            }
            else if (MyState == RPS_State.Paper)
            {
                dark = RPSX.paperColorDark;
                bright = RPSX.paperColor;
                faded = RPSX.paperColorFaded;
                saturated = Color.green;
                EmblemIndex = 1;
            }
            else if (MyState == RPS_State.Scissors)
            {
                dark = RPSX.scissorsColorDark;
                bright = RPSX.scissorsColor;
                faded = RPSX.scissorsColorFaded;
                saturated = Color.red;
                EmblemIndex = 2;
            }
            else
            {
                bright = RPSX.normalWalls;
                dark = RPSX.alphadOut;
            }

            if (MyState != RPS_State.Basic)
            {
                foreach (SpriteRenderer sr in colorChangers)
                {
                    sr.color = lerpingColor;
                }
                Emblem.color = lerpingColor;
                body.color = saturated;
            }
        }
    }

    void RouletteHandler ()
    {
        if (Spinning)
        {
            currentRouletteTimer += Time.deltaTime;
            spinTransitionTimer++;
            if (spinTransitionTimer >= spinTransitionTime)
            {
                if (EmblemIndex == 2)
                {
                    EmblemIndex = 0;
                }
                else 
                {
                    EmblemIndex++;
                }
                spinTransitionTimer = 0;
            }

            if (currentRouletteTimer > rouletteTimer)
            {
                currentRouletteTimer = 0;
                spinTransitionTimer = 0;
                Spinning = false;
                MyState = QueuedState;
                QueuedState = RPS_State.Basic;
            }
        }
    }

    public void StartRoutlette (RPS_State state)
    {
        Spinning = true;
        QueuedState = state;
    }

    public void TurnOff()
    {
        MyState = RPS_State.Basic;
        foreach (SpriteRenderer sr in colorChangers)
        {
            sr.DOColor(RPSX.normalWalls, .3f);
        }
        Emblem.color = Color.clear;
        body.DOColor(Color.white, .3f);
    }

    void CheckForPlayers ()
    {
        foreach (Player p in players)
        {
            RPS_Result result = RPSX.determineWinner(MyState, p.currentState);
            if (result == RPS_Result.Win)
            {
                Debug.Log(this.gameObject.name + " destroyed " + p.gameObject.name);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.GetComponent<Player>())
        {
            Player p = coll.gameObject.GetComponent<Player>();
            if (!players.Contains(p))
            {
                players.Add(p);
            }
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.GetComponent<Player>())
        {
            Player p = coll.gameObject.GetComponent<Player>();
            if (players.Contains(p))
            {
                players.Remove(p);
            }
        }
    }
}
