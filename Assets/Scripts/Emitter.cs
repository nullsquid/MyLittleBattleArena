using UnityEngine;
using System.Collections;

public class Emitter : MonoBehaviour {
	void Start(){
		//StartCoroutine(CoolDown(1));
	}
	public float coolDown = 1f;
	public Barrel barrel;
	public bool canShoot = false;
	/*public IEnumerator CoolDown(float cooldown){
		if(canShoot == true){
		while(canShoot){
			Shoot();
			yield return new WaitForSeconds(cooldown);
		}
		}
	}*/

	void Update(){
		if(canShoot){
			coolDown -= Time.deltaTime;
		}
	}
	public void Shoot(){
		if(canShoot){
		

			if(coolDown <= 0){
				GameObject bullet = Instantiate(barrel.projectile, transform.position, transform.rotation) as GameObject;
				bullet.GetComponent<Rigidbody2D>().AddForce(transform.up * barrel.speed);
				coolDown = 1;
			}
		}
		
	}
}
