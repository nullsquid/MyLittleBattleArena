using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public abstract class CharacterAbility : MonoBehaviour {
	private float coolDownTime = 0f;
	private float range = 0f;
	private float duration = 0f;
	private float startTime = 0f;
	public bool unlocked = true;	//TODO: false at first; what will the unlock parameter be?
	public ParticleSystem normalFX;
	public CharacterBase character;
	//public ParticleSystem aoeFX;
	//public ParticleSystem buffFX;
	public virtual void OnActivate(){}
	public virtual void OnDeactivate(){}
	//Ranged - range, projectile
	//Aoe - radius
	//Buff - duration
	//Movement - teleport, dash, speed
}
