using UnityEngine;
using System.Collections;

public class GunnerBulletHail : CharacterAbility {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public virtual void BulletHail(){
		//fires a stream of shots at forward vector
		//channeled (locks character movement)
		//OnActivate ==> character.canmove = false;
		//OnDeactivate ==> character.canmove = true;
	}
	//IEnumerate ShotLength(){

	//}
	public override void OnActivate(){
		BulletHail();
		character.GetComponent<CharacterBase>().CanMove = true;
	}

}
