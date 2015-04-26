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
	void OnTriggerStay2D(Collider2D other){
		Debug.Log("yes");
		cooldown = maxCooldown;
		if(gameObject.tag == other.gameObject.tag){
			cooldown -= Time.deltaTime;
			if(cooldown <= 0){
				Heal (other.gameObject);
				cooldown = maxCooldown;
			}
		}
	}

	void Heal(GameObject target){
		target.SendMessage("GetWell", healStep);
	}
}
