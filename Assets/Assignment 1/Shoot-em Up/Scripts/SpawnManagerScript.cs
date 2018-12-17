using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManagerScript : MonoBehaviour
{
	private static SpawnManagerScript mInstance = null;

	public static SpawnManagerScript Instance
	{
		get
		{
			//! singleton implementation for objects that can dynamically be created (does not have reference to hierarchy objects)
			if(mInstance == null)
			{
				GameObject tempObject = GameObject.FindWithTag("SpawnManager");

				if(tempObject == null)
				{
					tempObject = Instantiate(PrefabManagerScript.Instance.spawnManagerPrefab, Vector3.zero, Quaternion.identity);
				}
				mInstance = tempObject.GetComponent<SpawnManagerScript>();
			}
			return mInstance;
		}
	}

	public GameObject[] enemyPrefabList;
	public List<EnemyBaseScript> enemyList = new List<EnemyBaseScript>();

	float spawnTimer = 0.0f;
	float spawnDuration = 1.5f;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
//		spawnTimer += Time.deltaTime;
//
//		if(spawnTimer > spawnDuration)
//		{
//			spawnTimer = 0.0f;
//			EnemyBaseScript.Type type;
//
//			if(enemyList.Count < 3)
//			{
//				type = EnemyBaseScript.Type.ENEMY_01;
//			}
//			else
//			{
//				type = (EnemyBaseScript.Type)Random.Range(0, (int)EnemyBaseScript.Type.TOTAL);
//			}
//
//			GameObject newEnemy = Spawn(type);
//
//			//! Fancy Smenchy Spawning Logic
//			/*
//			if(newEnemy.GetComponent<EnemyBaseScript>().type == EnemyBaseScript.Type.ENEMY_03)
//			{
//				Spawn(EnemyBaseScript.Type.ENEMY_01);
//				Spawn(EnemyBaseScript.Type.ENEMY_02);
//			}
//			*/
//		}

//		if(Input.GetKeyDown(KeyCode.Space))
//		{
//			for(int i=0; i<100; i++)
//			{
//				GameObject newEnemy = Spawn((EnemyBaseScript.Type)Random.Range(0, (int)EnemyBaseScript.Type.TOTAL));
//			}
//		}
	}

	//! factory method
	GameObject Spawn(EnemyBaseScript.Type type)
	{
		GameObject newEnemy = null;

		if(type == EnemyBaseScript.Type.ENEMY_01)
		{
			newEnemy = ObjectPoolManagerScript.Instance.GetObject("Enemy01");
			newEnemy.transform.position = new Vector3(0.0f, 5.6f, 0.0f);
		}
		else if(type == EnemyBaseScript.Type.ENEMY_02)
		{
			newEnemy = ObjectPoolManagerScript.Instance.GetObject("Enemy02");
			newEnemy.transform.position = new Vector3(7.0f, 5.6f, 0.0f);
		}
		else if(type == EnemyBaseScript.Type.ENEMY_03)
		{
			newEnemy = ObjectPoolManagerScript.Instance.GetObject("Enemy03");
			newEnemy.transform.position = new Vector3(-7.0f, 5.6f, 0.0f);
		}
		else
		{
			Debug.LogError("UNKNOWN TYPE IN SPAWN FUNCTION");
		}

		enemyList.Add(newEnemy.GetComponent<EnemyBaseScript>());

		return newEnemy;
	}
}
