using UnityEngine;
using System.Collections;


public class GunShootingScript : MonoBehaviour
{
	public int damagePerShot = 20;
	public float timeBetweenBullets = 0.15f;
	public float range = 100f;
	
	public GameObject bulletPrefab;
	public float bulletSpeed = 100f;
	
	float timer;
	Ray shootRay;
	RaycastHit shootHit;
	int shootableMask;
	ParticleSystem gunParticles;
	LineRenderer gunLine;
	AudioSource gunAudio;
	float effectsDisplayTime = 0.2f;
	
	
	void Awake ()
	{
		shootableMask = LayerMask.GetMask ("Shootable");
		gunParticles = GetComponent<ParticleSystem> ();
		gunLine = GetComponent <LineRenderer> ();
		gunAudio = GetComponent<AudioSource> ();
	}
	
	
	void Update ()
	{
		timer += Time.deltaTime;
		

		
		if(timer >= timeBetweenBullets * effectsDisplayTime)
		{
			DisableEffects ();
		}
	}
	
	
	public void DisableEffects ()
	{
		gunLine.enabled = false;
	}
	
	void TryToFire(){
		if( timer >= timeBetweenBullets && Time.timeScale != 0)
		{
			Shoot ();
		}
		

	}

	void Shoot ()
	{
		timer = 0f;
		
		gunAudio.Play ();
		
		//gunLight.enabled = true;
		
		gunParticles.Stop ();
		gunParticles.Play ();
		
	//	gunLine.enabled = true;
		gunLine.SetPosition (0, transform.position);
		
		shootRay.origin = transform.position;
		shootRay.direction = transform.up;
		//RAYCAST don't work in 3d Baby!
		if(Physics.Raycast (shootRay, out shootHit, range, shootableMask))
		{
			
			//CODE HERE TO DISTINGUISH BETWEEN PLAYERS/TOWERS/ETC
			//PlayerHealth enemyHealth = shootHit.collider.GetComponent <PlayerHealth> ();
//			if(enemyHealth != null)
			{
				//                enemyHealth.TakeDamage (damagePerShot);
			}
			//gunLine.SetPosition (1, shootHit.point);
		}
		else
		{
			//gunLine.SetPosition (1, shootRay.origin + shootRay.direction * range);
		}
		
		//OR//
		
		GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject;
		bullet.GetComponent<Rigidbody2D>().AddForce(transform.up * bulletSpeed);
		
		
	}
}
