using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Hub : Building {
	float range;
	List<GameObject> healTargets = new List<GameObject>();
	float cooldown;
	public float maxCooldown = 2;
	public float healStep = 1;
	// Use this for initialization

	
	// Update is called once per frame
	void Update () {
	
	}

	void Heal(GameObject target){
		target.SendMessage("GetWell", healStep);
	}
}
