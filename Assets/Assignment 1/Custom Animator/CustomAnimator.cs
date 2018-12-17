using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimationState
{
	LOOPING,
	REVERSE,
	ONE_SHOT,
	PING_PONG,
	MULTI_ROW,
	NONE,
};

public class CustomAnimator : MonoBehaviour 
{
	[Header("State Condition")]
	public AnimationState curAnimationState;
	public float timer;

	[Header("Sprite Setup")]
	public int rowSize;
	public int colSize;
	public int pixelsPerUnit;

	[Header("Animation Controls")]
	public int currentRow;
	public int currentCol;
	public float framePerSecond;

	private bool isReversing = false;

	// Use this for initialization
	void Start () 
	{
		currentRow = 1;
		framePerSecond = 5;
		timer = 0.0f;
		curAnimationState = AnimationState.NONE;

		//! Slicing & tiling our texture to show only one sprite per quad
		Vector2 tiling = new Vector2 (1f / colSize, 1f / rowSize); // Slicing the texture
		GetComponent<Renderer> ().material.SetTextureScale ("_MainTex", tiling); // Apply tiling

		// Scaling the sprite while keeping the aspect ratio
		float sizeX = GetComponent<Renderer> ().material.mainTexture.width / (float) colSize;
		float sizeY = GetComponent<Renderer> ().material.mainTexture.height / (float) rowSize;
		this.transform.localScale = new Vector3 (sizeX / (float) pixelsPerUnit, sizeY / (float) pixelsPerUnit, 1f);
	}
	
	// Update is called once per frame
	void Update () 
	{
		GetKeyState ();
		if (curAnimationState == AnimationState.LOOPING) 
		{
			Vector2 offset = Vector2.zero;

			// Set row offset to show the current row
			offset.y = (1f / rowSize) * (rowSize - currentRow);

			// Iterate through every column of the row / second
			if (timer < 1f / framePerSecond) 
			{
				timer += Time.deltaTime;
			} 
			else 
			{
				timer = 0.0f;
				currentCol++;
				currentCol %= colSize;
			}

			offset.x = (1f / colSize) * currentCol;

			// Assign offset to sprite
			GetComponent<Renderer> ().material.SetTextureOffset ("_MainTex", offset);
		} 

		else if (curAnimationState == AnimationState.REVERSE) 
		{
			Vector2 offset = Vector2.zero;

			// Set row offset to show the current row
			offset.y = (1f / rowSize) * (rowSize - currentRow);

			// Iterate through every column of the row / second
			if (timer < 1f / framePerSecond) 
			{
				timer += Time.deltaTime;
			}
			else
			{
				timer = 0.0f;
				if (currentCol == 0) 
				{
					currentCol = colSize - 1;
				}
				else if (currentCol <= colSize ) 
				{
					currentCol--;
				}
			}

			offset.x = (1f /colSize) * currentCol;

			// Assign offset to sprite
			GetComponent<Renderer> ().material.SetTextureOffset ("_MainTex", offset);
		}

		else if (curAnimationState == AnimationState.ONE_SHOT)
		{
			Vector2 offset = Vector2.zero;

			// Set row offset to show the current row
			offset.y = (1f / rowSize) * (rowSize - currentRow);

			// Iterate through every column of the row / second
			if (currentCol != colSize - 1) 
			{
				if (timer < 1f / framePerSecond) 
				{
					timer += Time.deltaTime;
				} 
				else 
				{
					timer = 0.0f;
					currentCol++;
					currentCol %= colSize;
				}
			}
			else
			{
				timer = 0.0f;
				curAnimationState = AnimationState.NONE;
			}

			offset.x = (1f /colSize) * currentCol;

			// Assign offset to sprite
			GetComponent<Renderer> ().material.SetTextureOffset ("_MainTex", offset);
		}

		else if (curAnimationState == AnimationState.PING_PONG)
		{
			Vector2 offset = Vector2.zero;

			// Set row offset to show the current row
			offset.y = (1f / rowSize) * (rowSize - currentRow);

			if (timer < 1f / framePerSecond) 
			{
				timer += Time.deltaTime;
			}
			else if(!isReversing)
			{
				timer = 0.0f;
				if(currentCol != colSize - 1)
				{
					currentCol++;
				}
				else
				{
					currentCol = colSize - 2;
					isReversing = true;
				}
			}
			else // Reversing
			{
				timer = 0.0f;
				if (currentCol == 0)
				{
					currentCol = 1;
					isReversing = false;
				}
				else
				{
					currentCol--;
				}
			}

			offset.x = (1f /colSize) * currentCol;

			// Assign offset to sprite
			GetComponent<Renderer> ().material.SetTextureOffset ("_MainTex", offset);
		}

		else if (curAnimationState == AnimationState.MULTI_ROW)
		{
			Vector2 offset = Vector2.zero;

			// Set row offset to show the current row
			offset.y = (1f / rowSize) * (rowSize - currentRow);

			// Iterate through every column of the row / second
			if (timer < 1f / framePerSecond) 
			{
				timer += Time.deltaTime;
			} 
			else 
			{
				timer = 0.0f;
				currentCol++;
				//currentCol %= colSize + 1;

				if (currentCol == colSize) 
				{
					currentRow++;
					currentCol = 0;
				}
			}

			if (currentRow == 3 && currentCol == 2) 
			{
				currentRow = 1;
				currentCol = 0;
			}

			offset.x = (1f / colSize) * currentCol;

			// Assign offset to sprite
			GetComponent<Renderer> ().material.SetTextureOffset ("_MainTex", offset);
		}
	}

	void GetKeyState ()
	{
		if (Input.GetKeyDown (KeyCode.Alpha1)) 
		{
			curAnimationState = AnimationState.LOOPING;
		}
		else if (Input.GetKeyDown (KeyCode.Alpha2)) 
		{
			curAnimationState = AnimationState.REVERSE;
		}
		else if (Input.GetKeyDown (KeyCode.Alpha3)) 
		{
			timer = 0.0f;
			currentCol = 0;
			curAnimationState = AnimationState.ONE_SHOT;
		}
		else if (Input.GetKeyDown (KeyCode.Alpha4)) 
		{
			curAnimationState = AnimationState.PING_PONG;
		}
		else if (Input.GetKeyDown (KeyCode.Alpha5)) 
		{
			curAnimationState = AnimationState.MULTI_ROW;
		}
	}
}
