using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallManager : MonoBehaviour {

    List<List<DangerWalls>> Configurations = new List<List<DangerWalls>>();
    List<DangerWalls> CurrentConfig;
    public DangerWalls[] allWalls;
    public List<DangerWalls> Config_0 = new List<DangerWalls>();
    public List<DangerWalls> Config_1 = new List<DangerWalls>();
    public List<DangerWalls> Config_2 = new List<DangerWalls>();
    public List<DangerWalls> Config_3 = new List<DangerWalls>();
    public List<DangerWalls> Config_4 = new List<DangerWalls>();
    int Index;
    float activeTimer;
    public float activeTime;
    float downTimer;
    public float downTime;
    bool active;

    // Use this for initialization
    void Start () 
    {
        active = false;
        Configurations.Add(Config_0);
        Configurations.Add(Config_1);
        Configurations.Add(Config_2);
        Configurations.Add(Config_3);
        Configurations.Add(Config_4);
        allWalls = FindObjectsOfType<DangerWalls>();
    }
	
	// Update is called once per frame
	void Update () 
    {
        ManageUpTime();
        CurrentConfig = Configurations[Index];
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            ChangeConfigurations(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeConfigurations(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeConfigurations(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChangeConfigurations(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ChangeConfigurations(4);
        }
    }

    public void ChangeConfigurations(int sent)
    {
        Index = sent;
        int Rocks = 2;
        int Papers = 2;
        int Scissors = 2;
        for (int i = 0; i < CurrentConfig.Count; i++)
        {
            DangerWalls dw = CurrentConfig[i];
            int rando = Random.Range(0, 2);
            if (rando == 0)
            {
                if (Rocks > 0)
                { 
                    dw.StartRoutlette(RPS_State.Rock);
                    Rocks--;
                }
                else if (Papers > 0)
                {
                    dw.StartRoutlette(RPS_State.Paper);
                    Papers--;
                }
                else 
                {
                    dw.StartRoutlette(RPS_State.Scissors);
                    Scissors--;
                }
            }
            else if (rando == 1)
            {
                if (Papers > 0)
                {
                    dw.StartRoutlette(RPS_State.Paper);
                    Papers--;
                }
                else if (Scissors > 0)
                {
                    dw.StartRoutlette(RPS_State.Scissors);
                    Scissors--;
                }
                else
                {
                    dw.StartRoutlette(RPS_State.Rock);
                    Rocks--;
                }
            }
            else 
            {
                if (Scissors > 0)
                {
                    dw.StartRoutlette(RPS_State.Scissors);
                    Scissors--;
                }
                else if (Rocks > 0)
                {
                    dw.StartRoutlette(RPS_State.Rock);
                    Rocks--;
                }
                else
                {
                    dw.StartRoutlette(RPS_State.Paper);
                    Papers--;
                }
            }
        }
    }

    void ManageUpTime()
    {
        if (active)
        {
            activeTimer += Time.deltaTime;
        }
        else 
        {
            downTimer += Time.deltaTime;
        }

        if (activeTimer > activeTime)
        {
            activeTimer = 0;
            active = false; 
            foreach (DangerWalls wall in allWalls)
            {
                wall.TurnOff();
            }
        }

        if (downTimer > downTime)
        {
            downTimer = 0;
            int rando = Random.Range(0, Configurations.Count);
            ChangeConfigurations(rando);
            active = true;
        }
    }
}
