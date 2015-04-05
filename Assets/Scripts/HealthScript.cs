using UnityEngine;
using System.Collections;

public class HealthScript : MonoBehaviour {

	/// <summary>
	/// Total hitpoints
	/// </summary>
	public int hp = 1;
	
	/// <summary>
	/// Enemy or player?
	/// </summary>
	public bool isEnemy = true;

	//if enemy - destroy 10 second
	void Start(){
		/*
		if(this.name.Equals("poulpi")){
			//timelife = 10seconds
			Destroy(gameObject,10);
		}*/
	}

	/// <summary>
	/// Inflicts damage and check if the object should be destroyed
	/// </summary>
	/// <param name="damageCount"></param>
	public void Damage(int damageCount)
	{
		hp -= damageCount;
		
		if (hp <= 0)
		{
			// 'Splosion!
			SpecialEffectsHelper.Instance.Explosion(transform.position);

			//Spesial sounds effects
			SoundEffectsHelper.Instance.MakeExplosionSound();

			//
			//RandomEnemy.Instance.EnemyDead();

			// Dead!
			Destroy(gameObject);
		}
	}
	
	void OnTriggerEnter2D(Collider2D otherCollider)
	{
		// Is this a shot?
		ShotScript shot = otherCollider.gameObject.GetComponent<ShotScript>();
		if (shot != null)
		{
			// Avoid friendly fire
			if (shot.isEnemyShot != isEnemy)
			{
				Damage(shot.damage);
				
				// Destroy the shot
				Destroy(shot.gameObject); // Remember to always target the game object, otherwise you will just remove the script
			}
		}
	}
}
