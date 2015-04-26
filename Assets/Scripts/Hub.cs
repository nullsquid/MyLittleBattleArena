using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Hub : Building {
	float range;
	List<GameObject> healTargets = new List<GameObject>();
	float cooldown;
	float maxCooldown = 2;
	float healStep;
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
		target.SendMessage("GetWell", 1);
	}
}
