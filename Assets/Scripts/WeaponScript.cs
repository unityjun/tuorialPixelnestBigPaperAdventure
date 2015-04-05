using UnityEngine;
using System.Collections;

public class WeaponScript : MonoBehaviour {

	//--------------------------------
	// 1 - Designer variables
	//--------------------------------
	
	/// <summary>
	/// Projectile prefab for shooting
	/// </summary>
	public Transform shotPrefab;
	
	/// <summary>
	/// Cooldown in seconds between two shots
	/// </summary>
	public float shootingRate = 0.25f;
	
	//--------------------------------
	// 2 - Cooldown
	//--------------------------------
	
	private float shootCooldown;
	
	void Start()
	{
		if (this.name.Equals("poulpi")){
			shootCooldown = Random.Range(1,3)+Random.value;
		}
		else{
			shootCooldown = 0f;
		}
	}
	
	void Update()
	{
		if (shootCooldown > 0)
		{
			shootCooldown -= Time.deltaTime;
		}
	}
	
	//--------------------------------
	// 3 - Shooting from another script
	//--------------------------------
	
	/// <summary>
	/// Create a new projectile if possible
	/// </summary>
	public void Attack(bool isEnemy)
	{
		if (CanAttack)
		{
			shootCooldown = shootingRate;
			string nameObj = this.name;

			//offset shots
			float x_offset = 0f;
			float y_offset = 0f;

			if (nameObj.Equals("poulpi")){

				//random sleep

				//poulpi shot
				x_offset = -0.5f;
				y_offset = -0.55f;

				// Create a new shot
				var shotTransform1 = Instantiate(shotPrefab) as Transform;
				var shotTransform2 = Instantiate(shotPrefab) as Transform;

				shotTransform1.position = new Vector3(transform.position.x + x_offset
				                                     ,transform.position.y + y_offset
				                                     ,0);
				shotTransform2.position = new Vector3(transform.position.x + x_offset
				                                      ,transform.position.y + y_offset
				                                      ,0);

				// The is enemy property
				ShotScript shot1 = shotTransform1.gameObject.GetComponent<ShotScript>();
				if (shot1 != null)
				{
					shot1.isEnemyShot = isEnemy;
				}
				// The is enemy property
				ShotScript shot2 = shotTransform2.gameObject.GetComponent<ShotScript>();
				if (shot2 != null)
				{
					shot2.isEnemyShot = isEnemy;
				}

				MoveScript move1 = shotTransform1.gameObject.GetComponent<MoveScript>();
				MoveScript move2 = shotTransform2.gameObject.GetComponent<MoveScript>();

				float angl = 45f; //Random.Range(40,50);

				move1.y_direction = Mathf.Cos(angl) * move1.speed;
				move1.x_direction = Mathf.Sin(angl) * move1.speed * -1;
				
				move2.y_direction = Mathf.Cos(angl) * move2.speed * -1;
				move2.x_direction = Mathf.Sin(angl) * move2.speed * -1;

				move1.SendMessage("Update");
				move2.SendMessage("Update");
			}
			else{
				//player shoot
			}

		}
	}
	
	/// <summary>
	/// Is the weapon ready to create a new projectile?
	/// </summary>
	public bool CanAttack
	{
		get
		{
			return shootCooldown <= 0f;
		}
	}



}
