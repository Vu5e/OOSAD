using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHair : MonoBehaviour 
{
	public Vector3 rawMousePos;
	public Vector3 worldMousePos;

	// Update is called once per frame
	void Update () 
	{
		rawMousePos = Input.mousePosition;

		worldMousePos = Camera.main.ScreenToWorldPoint (rawMousePos);
		worldMousePos.z = 0f;
		this.transform.position = worldMousePos;
		Cursor.visible = false;

	}
}
