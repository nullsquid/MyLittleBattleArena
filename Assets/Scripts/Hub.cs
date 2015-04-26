using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Hub : Building {
	float range;

	string teamColor;
	List<GameObject> healTargets = new List<GameObject>();
	//public List<Tower> turrets = new List<Tower>();
	float cooldown;
	//bool canBeDamaged;
	public float maxCooldown = 2;
	public float healStep = 1;
	// Use this for initialization

	void Start(){

		//canBeDamaged = false;

	}
	// Update is called once per frame
	void Update () {
		//if(!team.towers.Contains(gameObject)){
		//	canBeDamaged = true;
		//}
	}

	void Heal(GameObject target){
		target.SendMessage("GetWell", healStep);
	}
}
