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
	public float range = 2.5f;
	public float projectileSpeed = 2.5f;
	float rotateSpeed;
	//Vector2 relativePos;
	public float baseCoolDown = 1f;
	private float coolDown = 1f;
	//public bool canShoot = true;
	// Use this for initialization
	void Start () {
	
	}
	void Update(){
		//if(canShoot){
			coolDown -= Time.deltaTime;
		//}
		if(coolDown <= 0){
			coolDown = baseCoolDown;
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
		}
	}
	void FixedUpdate () {
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
}
