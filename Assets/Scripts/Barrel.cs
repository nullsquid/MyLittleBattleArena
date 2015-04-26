using UnityEngine;
using System.Collections;

public class Barrel : MonoBehaviour {

	public GameObject projectile;
	public Tower tower;
	GameObject target;
	public float speed = 10;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(tower.targets.Count >= 1){
			target = tower.targets[0];
			Track ();
		}
		else if(tower.targets.Count == 0){
			tower.Idle();
			Debug.Log("idle");
		}
	}

	void Track(){
			Vector3 dir = target.transform.position - transform.position;
			float angle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
			transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
			GetComponentInChildren<Emitter>().canShoot = true;
			GetComponentInChildren<Emitter>().Shoot();
			//Invoke(GetComponentInChildren<Emitter>().ShootSequence(), 0);
		}
		//transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		//shouldFire = true;
		//Shoot ();

	void Shoot(){
		
		GameObject bullet = Instantiate(projectile, transform.position, transform.rotation) as GameObject;
		bullet.GetComponent<Rigidbody2D>().AddForce(transform.up * speed);

	}
}
