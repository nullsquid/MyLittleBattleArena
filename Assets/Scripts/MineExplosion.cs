using UnityEngine;
using System.Collections;

public class MineExplosion : MonoBehaviour {

	public float speed = 1.0F;
	private float lifetime = 3.0f;
	private float maxRadius = 10.0f;
	private Vector3 maxRadiusVec;
	private float startTime;
	public string teamColor;
	// Use this for initialization
	void Start () {
		Invoke ("Fizzle", lifetime);
		maxRadiusVec = new Vector3(maxRadius, maxRadius, maxRadius);  //no comment
		//Should starttime be time.time?
		startTime = Time.time;
		this.tag = teamColor;

		ColorManager.setColorByTag(this.tag,this.transform.GetChild(0).gameObject);

	}
	
	void Update () {
		float beforeTime = (Time.time - startTime) * speed;
		float lerpTime = beforeTime / lifetime;
		transform.localScale = Vector3.Lerp(Vector3.zero, maxRadiusVec, lerpTime);
	
	}

	void OnTriggerEnter2D(Collider2D other) {  //Mine explosions must collide with other mine explosions 	//Maybe Use Physics2D.OverlapCircleAll
		if(other.gameObject.GetComponent<MineTimer>() !=  null){
			other.gameObject.BroadcastMessage("EXPLODE");
		}

		other.gameObject.BroadcastMessage("Damage",1,SendMessageOptions.DontRequireReceiver);
	}

	void OnColliderEnter2D(Collider2D other) {  
		other.gameObject.BroadcastMessage("Damage",1,SendMessageOptions.DontRequireReceiver);
	}

	//Maybe do damage based on a timer.?

	void Fizzle(){
		Destroy(gameObject);
	}


	//Maybe Use Physics2D.OverlapCircleAll
}
