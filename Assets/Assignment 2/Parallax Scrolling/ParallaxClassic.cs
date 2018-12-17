using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxClassic : MonoBehaviour
{

	public GameObject spriteA;
	GameObject spriteB;
	Vector3 velocity = Vector3.zero;
	public float parallaxFactor = 1f;
	Vector3 threshold;

	// Use this for initialization
	void Start () 
	{
		//Create a copy of sprite A and assign as a reference B
		spriteB = Instantiate(spriteA, this.transform);
		Vector3 newPos = spriteA.transform.position;
		newPos.x = spriteA.GetComponent<SpriteRenderer> ().bounds.max.x;
		newPos.x += spriteA.GetComponent<SpriteRenderer> ().bounds.extents.x;
		spriteB.transform.position = newPos;
	}
	
	// Update is called once per frame
	void Update () 
	{
		ParallaxMovement ();
		ParallaxOffset ();
	}

	void ParallaxMovement ()
	{		
		velocity.x = Input.GetAxis ("Horizontal") * Time.deltaTime * parallaxFactor;
		spriteA.transform.Translate (velocity);
		spriteB.transform.Translate (velocity);

		//Calculate threshold
		threshold = Vector3.zero;
		threshold.x = spriteA.GetComponent<SpriteRenderer> ().bounds.extents.x * 2f;
	}

	void ParallaxOffset ()
	{
		if (spriteA.transform.position.x < -threshold.x) 
		{
			Vector3 newPos = spriteB.transform.position;
			newPos.x += spriteB.GetComponent<SpriteRenderer> ().bounds.extents.x * 2f;
			spriteA.transform.position = newPos;
		}
		else if (spriteA.transform.position.x > threshold.x) 
		{
			Vector3 newPos = spriteB.transform.position;
			newPos.x -= spriteB.GetComponent<SpriteRenderer> ().bounds.extents.x * 2f;
			spriteA.transform.position = newPos;
		}
		else if (spriteB.transform.position.x < -threshold.x) 
		{
			Vector3 newPos = spriteA.transform.position;
			newPos.x += spriteB.GetComponent<SpriteRenderer> ().bounds.extents.x * 2f;
			spriteB.transform.position = newPos;
		}
		else if (spriteB.transform.position.x > threshold.x) 
		{
			Vector3 newPos = spriteA.transform.position;
			newPos.x -= spriteB.GetComponent<SpriteRenderer> ().bounds.extents.x * 2f;
			spriteB.transform.position = newPos;
		}
	}
}
