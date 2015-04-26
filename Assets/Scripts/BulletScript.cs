using UnityEngine;
using System.Collections;


/*TODO:  Ignore Collisions with Own player
 * Set trail renderer to color of team.
 * 
 * 
 * 
 */


public class BulletScript : MonoBehaviour {


	public float Lifetime;
	// Use this for initialization
	void Awake () {
		Invoke ("Fizzle", Lifetime);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Fizzle(){
		Destroy(gameObject);
	}

	void OnCollisionEnter2D(Collision2D coll) {
		//TAGS
		Destroy(gameObject);

	}
}