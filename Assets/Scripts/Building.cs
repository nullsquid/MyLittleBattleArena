using UnityEngine;
using System.Collections;
public abstract class Building : MonoBehaviour {
	public float sight;
	public float health;
	public PlayerTeam team;
	public bool canBeDamaged;
	public bool isDestroyed = false;
	public void DealDamage(int damage){
		if(canBeDamaged == true){
			health -= damage;
			if(health<=0){
				IsDestroyed();
			}
		}
	}
	public void IsDestroyed(){
		isDestroyed = true;
		this.gameObject.SetActive(false);
		//Destroy(this.gameObject);

	}
}
