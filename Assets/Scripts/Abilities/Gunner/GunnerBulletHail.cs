using UnityEngine;
using System.Collections;

public class GunnerBulletHail : CharacterAbility {
	private float nextBulletStep = .5f;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public virtual void BulletHail(){
		character.GetComponent<CharacterBase>().CanMove = false;
		//fires a stream of shots at forward vector
		//channeled (locks character movement)
		//OnActivate ==> character.canmove = false;
		//OnDeactivate ==> character.canmove = true;
	}
	IEnumerator ShotLength(){

		yield return new WaitForSeconds(nextBulletStep);
	}
	public override void OnActivate(){
		BulletHail();
		character.GetComponent<CharacterBase>().CanMove = true;
	}

}
