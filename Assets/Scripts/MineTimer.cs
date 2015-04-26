using UnityEngine;
using System.Collections;

public class MineTimer : MonoBehaviour {

	//use some sort of timer, with countdown animations

	public float Lifetime;
	public string teamColor;//this should be an enum!!!
	public GameObject mineExplosionPrefab ;
	public AudioSource pingSound;
	public GameObject pingAnimationPrefab;


	// Use this for initialization
	void Awake () {
		Invoke ("Fizzle", Lifetime);
		Invoke ("RedAlert", Lifetime - Lifetime/10.0f);//this is a terrible way to do effects
		InvokeRepeating("MinePing", 1,1);
	}
	
	// Update is called once per frame
	void Update () {
		//tick tock bleep bloop
	}

	void RedAlert(){
		//cancel Ping?  or set a bool for mine ping or something.
	}

	void MinePing(){
		//PING
	//	this.GetComponent<AudioSource>().Play();  //this sound could get annoying?
	//	Instantiate(pingAnimationPrefab,transform.position,transform.rotation);
		//might want to pass the color too?
	}

	void Fizzle(){
		Destroy(gameObject);
	}
	
	void OnCollisionEnter2D(Collision2D coll) {
		//????
		
	}

	//When hit by a bullet:
	void EXPLODE(){
		print ("BOOM");


		GameObject mineExplosion = Instantiate(mineExplosionPrefab,transform.position, Quaternion.identity) as GameObject;
			//pass color, radius, and speed out, and speed in.
		Destroy(gameObject);

	}
}
