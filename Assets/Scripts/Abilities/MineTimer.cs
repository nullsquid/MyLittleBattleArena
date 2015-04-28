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
	void Start () {
		Invoke ("Fizzle", Lifetime);
		Invoke ("RedAlert", Lifetime - Lifetime/10.0f);//this is a terrible way to do effects
		InvokeRepeating("MinePing", 1,1);
		this.tag = teamColor;
	}
	
	// Update is called once per frame
	void Update () {

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
		EXPLODE();
		//Destroy(gameObject);
	}
	
	void OnCollisionEnter2D(Collision2D coll) {
		//????
		
	}

	//When hit by a bullet:


	void EXPLODE(float newerPitch){
		GameObject mineExplosion = Instantiate(mineExplosionPrefab,transform.position, Quaternion.identity) as GameObject;
		//mineExplosion.GetComponent<MineExplosion>().teamColor = this.tag;
		print (newerPitch.ToString() +" PitchUp?");
		mineExplosion.GetComponent<MineExplosion>().newPitch = newerPitch + 4.0f;
		//pass color, radius, and speed out, and speed in.
		Destroy(gameObject);
}
	void EXPLODE(){
		EXPLODE(0.0f);
	}
}