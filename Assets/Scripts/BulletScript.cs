using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {


	public float Lifetime;
	// Use this for initialization
	void Awake () {
		Invoke ("Fizzle", Lifetime);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void FixedUpdate(){
		//Physics.IgnoreLayerCollision(12, 11);
	}

	void Fizzle(){
		Destroy(gameObject);
	}

	void OnCollisionEnter2D(Collision2D coll) {
		//TAGS

		Destroy(gameObject);

	}
	/*void OnTriggerEnter2D(Collider2D other){

		SendMessage("DealDamage", 1);
		Destroy(gameObject);
	}*/
}