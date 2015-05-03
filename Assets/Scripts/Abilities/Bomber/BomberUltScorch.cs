using UnityEngine;
using System.Collections;

public class BomberUltScorch : CharacterAbility {

	private float chargeTimeMaximum = 5.0f;
	private float chargeStep = 1.0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public virtual void Scorch(){
		//Charge shot
		//Channeled
		//Explodes in radius when it reaches its destination
		//Triggers all nearby bombs
		//try with colliders and with ignoring colliders

	}
	IEnumerator ChargeUp(){

		yield return new WaitForSeconds(chargeStep);
	}
	public override void OnActivate(){
		Scorch();
	}
}
