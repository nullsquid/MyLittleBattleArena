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
	Dictionary<Vector2,Transform> activeBullets = new Dictionary<Vector2,Transform>();
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


		inRange = Physics2D.OverlapCircleAll(gameObject.transform.position, range, Layer.Player.ToMask());
		if(inRange != null && inRange.Length > 0){
			for(int i = 0; i < inRange.Length; i++){
				if (inRange[i] != null){
					RaycastHit2D hit = Physics2D.Raycast(transform.position, inRange[i].transform.position - transform.position, range);
					if (hit.collider.gameObject.layer == Layer.Player.ToIndex()){
						GameObject bullet = Instantiate(projectile, transform.position, transform.rotation) as GameObject;
						activeBullets.Add((Vector2)hit.transform.position, bullet.transform);
					}
				}
			}
		}
		if (activeBullets.Count > 0){
			foreach (KeyValuePair<Vector2, Transform> kvp in activeBullets){
				if (Vector2.Distance(kvp.Value.position, kvp.Key) < 0.1f){
					activeBullets.Remove(kvp.Key);
					Destroy(kvp.Value);
					break;
				}else{
					kvp.Value.position = Vector3.MoveTowards(kvp.Value.position, kvp.Key, Time.deltaTime);
				}
			}
		}
		
		//Debug.Log(inRange.Length);
		//if(inRange.
		//Debug.Log(inRange[2]);

	}


	
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

}
