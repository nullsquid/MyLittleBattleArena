using UnityEngine;
using System.Collections;

public class ClassGunner : CharacterClass {

	// Use this for initialization
	void Start () {
		ability1 = new GunnerBoltShot();
		ability2 = new GunnerBulletHail();

		ability1.unlocked = true;
		ability2.unlocked = false;
		ability3.unlocked = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}


}
