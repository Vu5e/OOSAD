using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseScript : MonoBehaviour {

	public enum Type
	{
		ENEMY_01 = 0,
		ENEMY_02,
		ENEMY_03,
		TOTAL
	}

	public Type type = Type.ENEMY_01;
	public bool isToBeDestroyed = false;

	// this does not get called, as it was overriden by the child class
	void Start () {
	}
	
	// Update is called once per frame
	public void Update ()
	{
		if(transform.position.y < -5.6f)
		{
			isToBeDestroyed = true;
		}

		if(isToBeDestroyed)
		{	
			//gameManager.GetComponent<GameManagerScript>().score += 100;
			GameManagerScript.Instance.score += 100;
			SoundManagerScript.Instance.PlaySFX(AudioClipID.SFX_ATTACK);
			//Destroy(gameObject);
			gameObject.SetActive(false);
			isToBeDestroyed = false;
		}
	}
}
