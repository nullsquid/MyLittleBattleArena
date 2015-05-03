using UnityEngine;
using System.Collections;
public class CharacterAbility : MonoBehaviour {
	private float coolDownTime = 0f;
	private float range = 0f;
	private float duration = 0f;
	private float startTime = 0f;
	public bool unlocked = true;	//TODO: false at first
	public ParticleSystem normalFX;
	//public ParticleSystem aoeFX;
	//public ParticleSystem buffFX;
	public virtual void OnActivate(){}
	public virtual void OnDeactivate(){}
	//Ranged - range, projectile
	//Aoe - radius
	//Buff - duration
	//Movement - teleport, dash, speed
}
