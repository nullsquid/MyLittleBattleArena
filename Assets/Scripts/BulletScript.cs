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
	public PlayerTeam team;
	private float birthTime;
	private string color = "grey";//this was messing shit up for some reason, so now i'm sticking to tags
	// Use this for initialization
	void Awake () {
		birthTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > birthTime + Lifetime){
			Fizzle();
		}
	}
	void FixedUpdate(){
		//Physics.IgnoreLayerCollision(12, 11);
	}

	void Fizzle(){
		Destroy(gameObject);
	}

	void OnCollisionEnter2D(Collision2D coll) {  //trigger instead?
		//TAGS


		Destroy(gameObject);
		coll.gameObject.SendMessage("DealDamage", 1,SendMessageOptions.DontRequireReceiver);

		if( coll.gameObject.GetComponent<MineTimer>() !=  null){  //Use something better than name, get object types
			coll.gameObject.BroadcastMessage("EXPLODE");
		}
		Destroy(gameObject);

	}

	void OnTriggerEnter2D(Collider2D col) {  //trigger instead?

			col.gameObject.BroadcastMessage("EXPLODE",SendMessageOptions.DontRequireReceiver);

			Destroy(gameObject);

	}
	
	/*void OnTriggerEnter2D(Collider2D other){
		//if(other.gameObject.tag == "Building"||other.gameObject.tag == "Actor"){
			SendMessage("DealDamage", 1);
			Destroy(gameObject);
		//}
	}*/
}