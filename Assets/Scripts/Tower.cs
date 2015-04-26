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
	Collider2D[] inRange;
	float projectileSpeed;
	public float range = 2.5f;
	float rotateSpeed;
	//Vector2 relativePos;
	public List<GameObject> targets = new List<GameObject>();
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		//	hitColliders = Physics2D.OverlapCircle(transform.position, 1);
		if(targets==null){

		}


		inRange = Physics2D.OverlapCircleAll(gameObject.transform.position, range);
		/*if(inRange.Length > 0){
		for(int i = 0; i<=inRange.Length;i++){
				Debug.Log(inRange[i].gameObject.GetComponent<PlayerTeam>().teamName);
			

			}
		}*/
		
		//Debug.Log(inRange.Length);
		//if(inRange.
		//Debug.Log(inRange[2]);

	}
	void OnTriggerStay2D(Collider2D other){

		if(other.gameObject.GetComponent<PlayerTeam>().teamName == this.gameObject.GetComponent<PlayerTeam>().teamName){
			targets.Add(other.gameObject);
		}
		//if(other.gameObject.team != gameObject.team){
		//if(other.gameObject == team.gameObject){
			
		//	targets.Add(other.gameObject);
		//}


		//}
	}
	/*void OnTriggerExit2D(Collider2D other){
		if(other.gameObject == targets[0]){
			targets.RemoveAt(0);
		}
		//else if (){
		
		//}
		
	}*/
	
	public void Idle(){
		//transform.rotation = Quaternion.Slerp(
		return;




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
