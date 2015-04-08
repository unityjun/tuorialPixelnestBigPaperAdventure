using System.Collections;
using UnityEngine;

/// <summary>
/// Simply moves the current game object
/// </summary>
public class MoveScript : MonoBehaviour
{
	// 1 - Designer variables
	
	/// <summary>
	/// Object speed
	/// </summary>
	public float speed = 10f;
	private Vector2 v_speed;
	
	/// <summary>
	/// Moving direction
	/// </summary>

	public float x_direction;
	public float y_direction;
	private Vector2 direction;
	
	private Vector2 movement;
	
	void Update()
	{
		//Update vector speed
		//v_speed = new Vector2(speed, speed);

		//update direction
		//direction = new Vector2(x_direction, y_direction);

		// 2 - Movement
		movement = new Vector2(
			//v_speed.x 
			speed * x_direction, //direction.x,
			//v_speed.y 
			speed * y_direction  //direction.y
			);
	}
	
	void FixedUpdate()
	{
		// Apply movement to the rigidbody
		GetComponent<Rigidbody2D>().velocity = movement;
	}
}