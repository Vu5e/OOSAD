using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCollision : MonoBehaviour 
{
	public SpriteRenderer spr;
	public CustomCollision other;
	public bool isPixelCollision = false;
	bool isColliding = false;
	public bool debugMode = false;

	void Start ()
	{
		this.spr = this.GetComponent<SpriteRenderer> ();
	}

	void Update ()
	{
		DetectCollision ();
		DebugRender ();

	}

	void DetectCollision ()
	{
		if (!isPixelCollision) 
		{
			if (this.spr.bounds.Intersects (other.spr.bounds)) 
			{
				isColliding = true;
			} 
			else 
			{
				isColliding = false;
			}
		} 
		else 
		{
			if (this.spr.bounds.Intersects (other.spr.bounds)) 
			{
				// R1
				Bounds2D r1 = new Bounds2D () 
				{
					minX = this.spr.bounds.min.x,
					maxX = this.spr.bounds.max.x,
					minY = this.spr.bounds.min.y,
					maxY = this.spr.bounds.max.y
				};

				//Debug.Log(string.Format("{0},{1},{2},{3}", r1.minX, r1.maxX, r1.minY, r1.maxY));

				// R2
				Bounds2D r2 = new Bounds2D () 
				{
					minX = other.spr.bounds.min.x,
					maxX = other.spr.bounds.max.x,
					minY = other.spr.bounds.min.y,
					maxY = other.spr.bounds.max.y
				};
					
				// All Section points / coordinates ( Max of mins and min of maxes )
				float x1 = Mathf.Min (r1.maxX, r2.maxX);
				float x2 = Mathf.Max (r1.minX, r2.minX);
				float y1 = Mathf.Min (r1.maxY, r2.maxY);
				float y2 = Mathf.Max (r1.minY, r2.minY);

				// Get section global coordinates and size
				Rect section = new Rect (
					              Mathf.Min (x1, x2), 
					              Mathf.Min (y1, y2), 
					              Mathf.Abs (x1 - x2),
					              Mathf.Abs (y1 - y2));

				// Convert Section to local texture space
				Rect r1Local = new Rect (
					              section.x - r1.minX,
					              section.y - r1.minY,
					              section.width,
					              section.height);

				Rect r2Local = new Rect (
					              section.x - r2.minX,
					              section.y - r2.minY,
					              section.width,
					              section.height);

				//if (debugMode) Debug.Log(string.Format("x: {0}    y: {1}", r1Local.x, r1Local.y));

				// Get color information within local section
				Color[] r1Colors = this.spr.sprite.texture.GetPixels (
					                  (int)r1Local.min.x,
					                  (int)r1Local.min.y,
					                  (int)(r1Local.width * spr.sprite.pixelsPerUnit),
					                  (int)(r1Local.height * spr.sprite.pixelsPerUnit));

				Color[] r2Colors = other.spr.sprite.texture.GetPixels (
					                  (int)r2Local.min.x,
					                  (int)r2Local.min.y,
					                  (int)(r2Local.width * spr.sprite.pixelsPerUnit),
					                  (int)(r2Local.height * spr.sprite.pixelsPerUnit));

				// Compare both sprite color information
				for (int i = 0; i < r1Colors.Length; i++) 
				{
					// Return colliding if both pixels are NOT TRANSPARENT (color.a == 1f)
					if (r1Colors [i].a == 1f && r2Colors [i].a == 1f) 
					{
						isColliding = true;
						return;
					}
				}
				isColliding = false;
			}
		}
	}

	// Render sprite red if collision is detected
	void DebugRender()
	{
		if (debugMode) 
		{
			if (isColliding) 
			{
				this.spr.color = Color.red;
			} 
			else 
			{
				this.spr.color = Color.white;
			}
		}
	}
}
