using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteCycler : MonoBehaviour {

    public Sprite[] sprites;
    public int index;
    Image img;

	// Use this for initialization
	void Start () 
    {
        img = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        img.sprite = sprites[index];
	}
}
