using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour 
{
	public enum AttackState
	{
		NONE,
		RANGED_ATTACK1,
		RANGED_ATTACK2,
		MELEE_ATTACK,
		PATROL,
	}

	public float speed = 3.0f;
	public bool isMoveRight = true;

	public AttackState prevAttackState;
	public AttackState curAttackState;

	public GameObject target;
	bool isMeleeDone = false;
	float meleeCheckOffset = 1.5f;

	float shootTimer = 0.0f;
	float shootDelay = 0.2f;
	int shootAmount = 0;

	public float patrolTimer = 0.0f;
	public float patrolDuration = 3.0f;

	// Use this for initialization
	void Start () 
	{
		target = GameObject.FindGameObjectWithTag("Player");
		curAttackState = AttackState.PATROL;
		//GetComponent<BoxCollider2D>().enabled = false;
	}

	// Update is called once per frame
	void Update () 
	{
		if(curAttackState == AttackState.PATROL)
		{
			if(isMoveRight)
			{
				transform.Translate(Vector3.right * speed * Time.deltaTime);
				if(transform.position.x > 7.5)
				{
					isMoveRight = false;
				}
			}
			else
			{
				transform.Translate(Vector3.left * speed * Time.deltaTime);
				if(transform.position.x < -7.5)
				{
					isMoveRight = true;
				}
			}
			patrolTimer += Time.deltaTime;
			if(patrolTimer >= patrolDuration)
			{
				patrolTimer = 0.0f;
				UpdateAttackState(AttackState.NONE);
			}
		}
		else if(curAttackState == AttackState.MELEE_ATTACK)
		{
			//! Melee Attack Behavior
			if(!isMeleeDone)
			{
				transform.Translate(Vector3.down * 6.0f * Time.deltaTime);
				if(transform.position.y < -3.5f)
				{
					transform.position = new Vector3(transform.position.x, -3.5f, transform.position.z);
					isMeleeDone = true;
				}
			}
			else
			{
				transform.Translate(Vector3.up * 2.0f * Time.deltaTime);
				if(transform.position.y >= 3.0f)
				{
					//prevAttackState = curAttackState;
					//curAttackState = AttackState.RANGED_ATTACK1;
					UpdateAttackState(AttackState.NONE);
				}
			}
		}
		else if(curAttackState == AttackState.RANGED_ATTACK1)
		{
			//! Ranged 1 Attack Behavior
			shootTimer += Time.deltaTime;
			if(shootTimer >= shootDelay)
			{
				Shoot(EnemyBaseScript.Type.ENEMY_01);
				shootAmount++;
				shootTimer = 0.0f;
			}

			if(shootAmount >= 10)
			{
				shootAmount = 0;
				//prevAttackState = curAttackState;
				//curAttackState = AttackState.RANGED_ATTACK2;
				UpdateAttackState(AttackState.NONE);
			}
		}
		else if(curAttackState == AttackState.RANGED_ATTACK2)
		{
			//! Ranged 2 Attack Behavior
			shootTimer += Time.deltaTime;
			if(shootTimer >= shootDelay)
			{
				Shoot(EnemyBaseScript.Type.ENEMY_02);
				Shoot(EnemyBaseScript.Type.ENEMY_03);
				shootAmount++;
				shootTimer = 0.0f;
			}

			if(shootAmount == 4)
			{
				//prevAttackState = curAttackState;
				//curAttackState = AttackState.PATROL;
				shootAmount = 0;
				UpdateAttackState(AttackState.NONE);
			}

		}
		else if(curAttackState == AttackState.NONE)
		{
			isMeleeDone = false;
			if(target.transform.position.x < transform.position.x + meleeCheckOffset && 
				target.transform.position.x > transform.position.x - meleeCheckOffset)
			{
				if(prevAttackState != AttackState.MELEE_ATTACK && prevAttackState != AttackState.RANGED_ATTACK1)
				{
					//curAttackState = AttackState.MELEE_ATTACK;
					UpdateAttackState(AttackState.MELEE_ATTACK);
				}
				else if(prevAttackState != AttackState.RANGED_ATTACK1)
				{
					//curAttackState = AttackState.RANGED_ATTACK1;
					UpdateAttackState(AttackState.RANGED_ATTACK1);
				}
				else
				{
					//curAttackState = AttackState.PATROL;
					UpdateAttackState(AttackState.PATROL);
				}
			}
			else if(prevAttackState != AttackState.RANGED_ATTACK2)
			{
				//curAttackState = AttackState.RANGED_ATTACK2;
				UpdateAttackState(AttackState.RANGED_ATTACK2);
			}
			else
			{
				//curAttackState = AttackState.PATROL;
				UpdateAttackState(AttackState.PATROL);
			}
		}
	}

	private void UpdateAttackState(AttackState state)
	{
		prevAttackState = curAttackState;
		curAttackState = state;
	}

	GameObject Shoot(EnemyBaseScript.Type type)
	{
		GameObject newEnemy = null;

		if(type == EnemyBaseScript.Type.ENEMY_01)
		{
			newEnemy = ObjectPoolManagerScript.Instance.GetObject("Enemy01");
		}
		else if(type == EnemyBaseScript.Type.ENEMY_02)
		{
			newEnemy = ObjectPoolManagerScript.Instance.GetObject("Enemy02");
		}
		else if(type == EnemyBaseScript.Type.ENEMY_03)
		{
			newEnemy = ObjectPoolManagerScript.Instance.GetObject("Enemy03");
		}
		else
		{
			Debug.LogError("UNKNOWN TYPE IN SPAWN FUNCTION");
		}

		newEnemy.transform.position = transform.position;
		SpawnManagerScript.Instance.enemyList.Add(newEnemy.GetComponent<EnemyBaseScript>());

		return newEnemy;
	}
}
