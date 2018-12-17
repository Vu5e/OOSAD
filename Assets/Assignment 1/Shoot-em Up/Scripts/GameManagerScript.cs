using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
	private static GameManagerScript mInstance = null;

	public static GameManagerScript Instance
	{
		get
		{
			//! singleton implementation for objects that can dynamically be created (does not have reference to hierarchy objects)
			if(mInstance == null)
			{
				GameObject tempObject = GameObject.FindWithTag("GameManager");

				if(tempObject == null)
				{
					tempObject = Instantiate(PrefabManagerScript.Instance.gameManagerPrefab, Vector3.zero, Quaternion.identity);
				}
				mInstance = tempObject.GetComponent<GameManagerScript>();
			}
			return mInstance;
		}
	}

	public Text scoreText;
	public int score;

	public GameObject gameManagerPrefab;

	// Use this for initialization
	void Start ()
	{
		SoundManagerScript.Instance.PlayBGM(AudioClipID.BGM_GAMEPLAY);

		ObjectPoolManagerScript.Instance.CreatePool(SpawnManagerScript.Instance.enemyPrefabList[0], 5, 15);
		ObjectPoolManagerScript.Instance.CreatePool(SpawnManagerScript.Instance.enemyPrefabList[1], 5, 15);
		ObjectPoolManagerScript.Instance.CreatePool(SpawnManagerScript.Instance.enemyPrefabList[2], 5, 15);
	}
	
	// Update is called once per frame
	void Update ()
	{
		scoreText.text = "Score : " + score;
	}
}
