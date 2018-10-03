using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour {

    ParticleSystem ps;
    //public AudioClip poof;
    public Color test;
    public float lifeTime;
    public bool permanant;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!permanant)
        {
            lifeTime -= Time.deltaTime;
            if (lifeTime <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void burst(Color sent, int num)
    {
        ps = GetComponent<ParticleSystem>();
        ParticleSystem.MainModule ma = ps.main;
        ma.startColor = new Color(sent.r, sent.g, sent.b);
        //GetComponent<AudioSource> ().PlayOneShot (poof);
        ps.Emit(num);
    }
}
