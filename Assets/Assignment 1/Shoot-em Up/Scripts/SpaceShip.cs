using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShip : MonoBehaviour 
{
	public GameObject bullet;
	public Vector3 velocity;
	public Vector3 gamePadPos;
	public float speed = 1f;
	public CrossHair crossHair;
	public Vector3 direction;
	public float rotSpeed = 15f;
	public int ControlScheme = 0;

	void Update () 
	{
		Orientation();
		if (ControlScheme == 0)
		{
			// Movement
			if (Input.GetKey (KeyCode.W))
			{
				transform.Translate (Vector3.up * Time.deltaTime * speed);
			}
			if (Input.GetKey (KeyCode.A))
			{
				transform.Translate (Vector3.left * Time.deltaTime * speed);
			}
			if (Input.GetKey (KeyCode.S))
			{
				transform.Translate (Vector3.down * Time.deltaTime * speed);
			}
			if (Input.GetKey (KeyCode.D))
			{
				transform.Translate (Vector3.right * Time.deltaTime * speed);
			}

			// Fire bullet
			this.transform.transform.Translate (velocity * Time.deltaTime * speed, Space.World);
			if (Input.GetButtonDown ("Fire1") || Input.GetKeyDown (KeyCode.Space)) 
			{
				Instantiate (bullet, this.transform.position, this.transform.rotation);
			}
		}	

		if (ControlScheme == 1) 
		{
			gamePadPos.x = Input.GetAxis ("Horizontal");
			gamePadPos.y = Input.GetAxis ("Vertical");
			transform.position = gamePadPos + transform.position;

			if (Input.GetKeyDown (KeyCode.Joystick1Button0)) 
			{
				Instantiate (bullet, this.transform.position, this.transform.rotation);
			}
		}

		if (Input.GetKeyDown (KeyCode.Return)) 
		{
			if (ControlScheme == 0)
			{
				ControlScheme = 1;
			}
			else if (ControlScheme == 1)
			{
				ControlScheme = 0;
			}
		}
	}

	void Orientation()
	{
		direction = crossHair.transform.position - this.transform.position;
		direction.Normalize ();
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		this.transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.Euler(0, 0, angle - 90) , Time.deltaTime * rotSpeed);
	}
}

