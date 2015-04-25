using UnityEngine;
using System.Collections;

public class Barrel : MonoBehaviour {

	public GameObject projectile;
	public Tower tower;
	public GameObject target;
	float cooldown; 
	// Use this for initialization
	void Start () {
		cooldown = tower.cooldown;
	}
	
	// Update is called once per frame
	void Update () {
		//if(tower.target!=null){
		//	target = tower.target;
		//}
		if(tower.targets!=null){
			target = tower.targets[0];
			Track ();
		}

	}
	IEnumerator FireRate(float cooldown){

		Instantiate(projectile, transform.position, transform.rotation);
		yield return new WaitForSeconds(cooldown);
	}
	void Track(){
		Vector3 dir = target.transform.position - transform.position;
		float angle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		//Shoot ();
	}
}
