using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AfterImage : MonoBehaviour {

	public float lifespan;
	public GameObject skeleton;
	public List<Transform> bones = new List<Transform>(); 

	// Use this for initialization
	void Start () 
	{
		for (int i = 0; i < skeleton.transform.childCount; i++) 
		{
			Transform newbone = skeleton.transform.GetChild (i);
			bones.Add (newbone);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		GetComponent<SpriteRenderer> ().DOFade (0, lifespan).OnComplete(killSelf);
	}

	void killSelf()
	{
		Destroy (this.gameObject);
	}
}
