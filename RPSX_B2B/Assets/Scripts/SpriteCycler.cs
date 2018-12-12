using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteCycler : MonoBehaviour {

    public Sprite[] sprites;
    public int index;
    Animator anim;
    Image img;

	// Use this for initialization
	void Start () 
    {
        anim = GetComponent<Animator>();
        img = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        img.sprite = sprites[index];
        if (anim != null)
        {
            anim.SetInteger("Index", index);
        }
	}
}
