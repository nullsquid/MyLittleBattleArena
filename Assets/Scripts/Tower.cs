using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Tower : Building {

	float cooldown;
	float distanceToTarget;
	GameObject target;
	GameObject projectile;
	float projectileSpeed;
	float range;
	public List<GameObject> targets = new List<GameObject>();
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void Track(){

		if(Vector2.Distance(gameObject.transform.position, this.transform.position) <= sight&&gameObject.tag == "Targetable"){
			targets.Add(gameObject);
		}
		if(Vector2.Distance(targets[0].transform.position, this.transform.position) > sight){
			targets.RemoveAt(0);
		}
		else if (targets[0] == null){
			Idle();
		}
		if(targets[0] != null){

			Shoot (targets[0]);
		}

	}

	void Shoot(GameObject target){

	}
	void Idle(){

	}
}
