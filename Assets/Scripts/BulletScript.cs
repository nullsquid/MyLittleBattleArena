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
	private string color = "grey";
	// Use this for initialization
	void Awake () {
		Invoke ("Fizzle", Lifetime);
		//this.transform.tag = 
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Fizzle(){
		Destroy(gameObject);
	}

	void OnCollisionEnter2D(Collision2D coll) {  //trigger instead?
		//TAGS
		//print (coll.transform.name);
		if(coll.transform.tag == color && coll.gameObject.GetComponent<MineTimer>() !=  null){  //Use something better than name, get object types
			coll.gameObject.BroadcastMessage("EXPLODE");
		}
		Destroy(gameObject);

	}
}