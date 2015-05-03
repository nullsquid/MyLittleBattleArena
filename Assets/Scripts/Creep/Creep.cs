using UnityEngine;
using System.Collections;
public enum Lane{
	Top,
	Mid,
	Bot
}
public class Creep : MonoBehaviour {
	public string team;
	public float speed = 2.0f;
	public Transform target;
	NavMeshAgent agent;
	// Use this for initialization
	void Start () {
		//agent = GetComponent<NavMeshAgent>();
		if(gameObject.tag == "blue"){
		Vector3 dir = target.transform.position - transform.position;
		float angle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) -90;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		}
	}
	
	// Update is called once per frame
	void Update () {
		//Vector3 dir = target.transform.position - transform.position;
		//float angle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) +180;
		//transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

		transform.Translate(Vector2.up * Time.deltaTime*speed);
		//agent.SetDestination(target.position);
	}
	void OnCollisionEnter2D(Collision2D other){
		if(other.gameObject.tag != this.gameObject.tag){
			//other.SendMessage("DealDamage", 1);
			//Destroy(this.gameObject);
			//Debug.Log("damage");
		}
	}
}
