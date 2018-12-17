using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy02Script : EnemyBaseScript
{
	public float speed;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		base.Update();
		transform.Translate(new Vector3(-0.5f, -1.0f, 0.0f) * speed * Time.deltaTime);
	}
}
