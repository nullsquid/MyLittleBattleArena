using UnityEngine;
using System.Collections;
public abstract class Building : MonoBehaviour {
	public float sight;
	public float health;
	public PlayerTeam team;
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
