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
	public GameObject projectile;
	Collider2D[] inRange;
	public float range = 2.5f;
	public float projectileSpeed = 2.5f;
	public float projectileLifetime = 1.5f;
	float rotateSpeed;
	//Vector2 relativePos;
	public float baseCoolDown = 1f;
	private float coolDown = 1f;
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
						PlayerMovement player = inRange[i].GetComponent<PlayerMovement>();
						if (player != null && player.thisCharacterData.team != team){
							RaycastHit2D hit = Physics2D.Raycast(transform.position, inRange[i].transform.position - transform.position, range, ~Layer.Buildings.ToMask());
							if (hit.collider.gameObject.layer == Layer.Player.ToIndex()){
								GameObject bullet = Instantiate(projectile, transform.position, transform.rotation) as GameObject;
								BulletScript bulletScr = bullet.GetComponent<BulletScript>();
								if (bulletScr != null){
									bulletScr.Lifetime = projectileLifetime;
								}
								Rigidbody2D bulletRigid = bullet.GetComponent<Rigidbody2D>();
								if (bulletRigid != null){
									bulletRigid.AddForce ((hit.transform.position - transform.position) * 100);
								}
								break;
							}
						}
					}
				}
			}
		}
	}
}
