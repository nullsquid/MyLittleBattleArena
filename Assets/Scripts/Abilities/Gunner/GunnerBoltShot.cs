using UnityEngine;
using System.Collections;

public class GunnerBoltShot : CharacterAbility {

	public GameObject bullet;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public virtual void BoltShot(){
		GameObject newShot = Instantiate(bullet, transform.position, Quaternion.identity) as GameObject; 
		//newShot.GetComponent<BulletScript>();
		//Will instantiate a linear shot towards target

	}
	
}
