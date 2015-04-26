using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Tower : Building {

	
	public float cooldown;
	//float distanceToTarget;
	//public GameObject target;
	//public GameObject projectile;

//	float cooldown;
	float distanceToTarget;
	GameObject target;
	GameObject projectile;

	float projectileSpeed;
	float range;
	float rotateSpeed;
	//Vector2 relativePos;
	public List<GameObject> targets = new List<GameObject>();
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		//	hitColliders = Physics2D.OverlapCircle(transform.position, 1);
		/*if(targets==null){
			Idle ();
		}*/
		
	}
	void OnTriggerEnter2D(Collider2D other){
		if(other.tag == "Targetable"){
			targets.Add(other.gameObject);
		}
	}
	void OnTriggerExit2D(Collider2D other){
		if(other.gameObject == targets[0]){
			targets.RemoveAt(0);
		}
		//else if (){
		
		//}
		
	}
	
	void Idle(){
		//transform.rotation = Quaternion.Slerp(





	}
	void Track(){

		/*if(Vector2.Distance(gameObject.transform.position, this.transform.position) <= sight&&gameObject.tag == "Targetable"){
			targets.Add(gameObject);

		}*/
		if(targets[0]!=null){
			if(Vector2.Distance(targets[0].transform.position, this.transform.position) > sight){
				targets.RemoveAt(0);
			}
		}
		else if (targets[0] == null){
			Idle();
		}
		//if(targets[0] != null){
			
			//Quaternion.LookRotation(relativePos);
			//Shoot (targets[0]);
		//}

	}

	void Shoot(GameObject target){

	}

}
