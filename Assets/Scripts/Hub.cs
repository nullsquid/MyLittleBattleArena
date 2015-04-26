using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Hub : Building {
	float range;
	PlayerTeam team;
	string teamColor;
	List<GameObject> healTargets = new List<GameObject>();
	public List<GameObject> turrets = new List<GameObject>();
	float cooldown;
	bool canBeDamaged;
	public float maxCooldown = 2;
	public float healStep = 1;
	// Use this for initialization

	void Start(){
		for(int i = 0; i <= team.buildings.Count; i++){
			if(team.buildings[i].GetType() == typeof(Tower)){
				turrets.Add(team.buildings[i].gameObject);
			}
		}
	}
	// Update is called once per frame
	void Update () {
		
	}

	void Heal(GameObject target){
		target.SendMessage("GetWell", healStep);
	}
}
