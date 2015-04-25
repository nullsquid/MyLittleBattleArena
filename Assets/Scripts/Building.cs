using UnityEngine;
using System.Collections;

public enum Team{
	Red,
	Blue
};
public abstract class Building : MonoBehaviour {
	public float sight;
	public float health;
	//colliders?
	//UI elements?
	public void IsDamaged(int damage){
		health -= damage;
		if(health<=0){
			IsDestroyed();
		}
	}
	public void IsDestroyed(){

	}
}
