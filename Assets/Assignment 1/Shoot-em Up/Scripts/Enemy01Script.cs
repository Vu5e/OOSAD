using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy01Script : EnemyBaseScript
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
		transform.Translate(Vector3.down * speed * Time.deltaTime);
	}
}
