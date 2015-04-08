using UnityEngine;

/// <summary>
/// Player controller and behavior
/// </summary>
public class PlayerScript : MonoBehaviour
{
	/// <summary>
	/// 1 - The speed of the ship
	/// </summary>
	public float speed = 5.0f;
	public float fix_particalSpeed = 0.05f;
	public float fix_multiplierOfSpeed = 0.5f;

	public Transform shotPref;

	private float camW;
	private float camH;

	private Vector2 v_speed;
	// 2 - Store the movement
	private Vector2 movement;

	void OnDestroy()
	{
		// Game Over.
		// Add the script to the parent because the current game
		// object is likely going to be destroyed immediately.
		transform.parent.gameObject.AddComponent<GameOverScript>();
	}

	void Start(){
		camW = Camera.main.pixelWidth;
		camH = Camera.main.pixelHeight;
	}

	void Update()
	{
		camW = Camera.main.pixelWidth;
		camH = Camera.main.pixelHeight;

		moveToKeyboard();
		rotateToCursore();

		// 5 - Shooting
		bool shoot = Input.GetButtonDown("Fire1");
		shoot |= Input.GetButtonDown("Fire2");
		// Careful: For Mac users, ctrl + arrow is a bad idea
		if (shoot)
		{
			Shoot();

			//
			SoundEffectsHelper.Instance.MakePlayerShotSound();
		}

		// 6 - Make sure we are not outside the camera bounds
		var dist = (transform.position - Camera.main.transform.position).z;
		
		var leftBorder = Camera.main.ViewportToWorldPoint(
			new Vector3(0, 0, dist)
			).x;
		
		var rightBorder = Camera.main.ViewportToWorldPoint(
			new Vector3(1, 0, dist)
			).x;
		
		var topBorder = Camera.main.ViewportToWorldPoint(
			new Vector3(0, 0, dist)
			).y;
		
		var bottomBorder = Camera.main.ViewportToWorldPoint(
			new Vector3(0, 1, dist)
			).y;
		
		transform.position = new Vector3(
			Mathf.Clamp(transform.position.x, leftBorder, rightBorder),
			Mathf.Clamp(transform.position.y, topBorder, bottomBorder),
			transform.position.z
			);
		
		// End of the update method
	}
	
	void FixedUpdate()
	{
		// 5 - Move the game object
		GetComponent<Rigidbody2D>().velocity = movement;
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		bool damagePlayer = false;
		
		// Collision with enemy
		EnemyScript enemy = collision.gameObject.GetComponent<EnemyScript>();
		if (enemy != null)
		{
			// Kill the enemy
			HealthScript enemyHealth = enemy.GetComponent<HealthScript>();
			if (enemyHealth != null) enemyHealth.Damage(enemyHealth.hp);
			
			damagePlayer = true;
		}
		
		// Damage the player
		if (damagePlayer)
		{
			HealthScript playerHealth = this.GetComponent<HealthScript>();
			if (playerHealth != null) playerHealth.Damage(1);
		}
	}

	void moveToKeyboard(){
		/*
		// 3 - Retrieve axis information
		float inputX = Input.GetAxis("Horizontal");
		float inputY = Input.GetAxis("Vertical");
		
		// 4 - Movement per direction
		movement = new Vector2(
			v_speed.x * inputX,
			v_speed.y * inputY);*/

		v_speed = new Vector2(speed, speed);

		// 3 - Retrieve axis information
		float inputX = Input.GetAxis("Horizontal");
		float inputY = Input.GetAxis("Vertical");
		
		// 4 - Movement per direction
		movement = new Vector2(
			v_speed.x * inputX,
			v_speed.y * inputY);
		

	}

	void rotateToCursore(){
		
		float m_x = Input.mousePosition.x>camW?camW:(Input.mousePosition.x<0?0:(Input.mousePosition.x));
		float m_y = Input.mousePosition.y>camH?camH:(Input.mousePosition.y<0?0:(Input.mousePosition.y));
		float m_z = Input.mousePosition.z;
		
		var trf_inWorldPoint = this.transform.position;
		var mpos_inWorldPoint = Camera.main.ScreenToWorldPoint(new Vector3(m_x,m_y,m_z));
		
		float Angle = returnDegFromRad(mpos_inWorldPoint.y-trf_inWorldPoint.y,mpos_inWorldPoint.x-trf_inWorldPoint.x);
		
		//Debug.Log(Angle);
		
		transform.GetComponent<Rigidbody2D>().MoveRotation(Angle);
	} 

	void Shoot(){

		//player shot
		float m_x = Input.mousePosition.x>camW?camW:(Input.mousePosition.x<0?0:(Input.mousePosition.x));
		float m_y = Input.mousePosition.y>camH?camH:(Input.mousePosition.y<0?0:(Input.mousePosition.y));
		float m_z = Input.mousePosition.z;
		
		var trf_inWorldPoint = this.transform.position;
		var mpos_inWorldPoint = Camera.main.ScreenToWorldPoint(new Vector3(m_x,m_y,m_z));
		
		//
		float x_offset = 0f;
		float y_offset = 0;

		float cursoreAngle = returnDegFromRad(mpos_inWorldPoint.y-trf_inWorldPoint.y,mpos_inWorldPoint.x-trf_inWorldPoint.x);

		// Create a new shot
		Transform shotT = Instantiate(shotPref) as Transform;
		
		shotT.position = new Vector3(transform.position.x + x_offset
		                                     ,transform.position.y + y_offset
		                                     ,0);
		
		shotT.GetComponent<Rigidbody2D>().MoveRotation(cursoreAngle);
		
		// The is enemy property
		ShotScript shot = shotT.gameObject.GetComponent<ShotScript>();
		if (shot != null)
		{
			shot.isEnemyShot = false;
		}
		
		// Make the weapon shot always towards it
		MoveScript move = shotT.gameObject.GetComponent<MoveScript>();
		if (move != null)
		{	
			move.x_direction = this.transform.right.x;
			move.y_direction = this.transform.right.y;
			
			move.SendMessage("Update");	
		}

	}

	float returnDegFromRad(float y,float x){
		float rad_a = 0f;
		float deg_a = 0f;
		
		rad_a = x==0?0:Mathf.Abs(Mathf.Atan(y/x));
		if(x==0 && y ==0){
			deg_a =0;
		}
		else if(y==0 && x>0){
			//I 
			deg_a = 0;
		}
		else if(y>0 && x==0){
			//I 
			deg_a = 90;
		}
		else if(y==0 && x<0){
			//I 
			deg_a = 180;
		}
		else if(y<0 && x==0){
			//I 
			deg_a = 270;
		}
		else if(y>0 && x>0){
			//I 
			deg_a = rad_a*Mathf.Rad2Deg;
		}
		else if(y<0 && x<0){
			//III
			deg_a = rad_a*Mathf.Rad2Deg + 180;
		}
		else if(y>0 && x<0){
			//II
			deg_a = 180 - rad_a*Mathf.Rad2Deg;
		}
		else if(y<0 && x>0){
			//IV
			deg_a = 360 - rad_a*Mathf.Rad2Deg;
		}
		
		return deg_a;
	}
}