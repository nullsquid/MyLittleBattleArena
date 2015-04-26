using UnityEngine;
using System.Collections;

public enum Team{
	Red,
	Blue
};
public abstract class Building : MonoBehaviour {
	public float sight;
	public float health;
	public string team;
	//colliders?
	//UI elements?
	void Start(){
		if(team == "red"){
			gameObject.tag = "red";
		}
		else if (team == "blue"){
			gameObject.tag = "blue";
		}
		else{
			Debug.LogWarning(this.gameObject.name + " does not have a tag");
			this.gameObject.tag = "Untagged";
		}
	}
	public void DealDamage(int damage){
		health -= damage;
		if(health<=0){
			IsDestroyed();
		}
	}
	public void IsDestroyed(){
		Destroy(this.gameObject);
	}
}
