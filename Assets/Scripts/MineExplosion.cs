﻿using UnityEngine;
using System.Collections;

public class MineExplosion : MonoBehaviour {

	public float speed = 1.0F;
	private float lifetime = 3.0f;
	private float maxRadius = 5.0f;
	private Vector3 maxRadiusVec;
	private float startTime;
	// Use this for initialization
	void Start () {
		Invoke ("Fizzle", lifetime);
		maxRadiusVec = new Vector3(maxRadius, maxRadius, maxRadius);  //no comment
	}
	
	void Update () {
		float beforeTime = (Time.time - startTime) * speed;
		float lerpTime = beforeTime / lifetime;
		transform.localScale = Vector3.Lerp(Vector3.zero, maxRadiusVec, lerpTime);
	
	}

	void OnTriggerEnter2D(Collider2D other) {  //Mine explosions must collide with other mine explosions 	//Maybe Use Physics2D.OverlapCircleAll

		//Rocks fall, everyone dies
	}

	void Fizzle(){
		Destroy(gameObject);
	}

	//Maybe Use Physics2D.OverlapCircleAll
}
