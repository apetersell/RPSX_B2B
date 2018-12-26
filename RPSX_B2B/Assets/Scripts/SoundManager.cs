using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public static AudioSource auds;
    public AudioClip KOSound;
    public AudioClip WallBounce;
    public AudioClip ParrySound;
    public AudioClip DizzySound;
    public AudioClip StateChangeSound;
    public AudioClip BurstSound;
    public AudioClip NormalOut;
    public AudioClip CoinDink;
    public AudioClip TokenGet;
    public AudioClip DeathSound;
    public AudioClip DodgeSound;

	// Use this for initialization
	void Start () 
    {
        auds = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    public static void PlaySound(AudioClip clip)
    {
        auds.PlayOneShot(clip);
    }
}
