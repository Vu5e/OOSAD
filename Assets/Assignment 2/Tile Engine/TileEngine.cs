using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TileEngine : MonoBehaviour
{
	// Y = Rows // X = Columns
	public int mapWidth = 1;
	public int mapHeight = 1;
	public Sprite[] tileSprites;
	GameObject[,] tileRefs = new GameObject[0,0];
	int[,] tileMap = new int[0, 0];
	Vector3 offset;

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Alpha1)) 
		{
			RandomizeTiles ();
		}
		if (Input.GetKeyDown (KeyCode.Alpha2))
		{
			HardcodeTiles ();
		}
		if (Input.GetKeyDown (KeyCode.Alpha3)) 
		{
			LoadTilesFromText ();
		}
	}

	void ResetTileMap ()
	{
		tileMap = new int[0, 0];

		for (int row = 0; row < tileRefs.GetLength (1); row++) 
		{
			for (int col = 0; col < tileRefs.GetLength (0); col++) 
			{
				Destroy (tileRefs [col, row]);
			}
		}
		Debug.Log ("All Tiles Deleted");
	}

	void RecalculateOffsets ()
	{
		offset = Vector3.zero;
		offset.x = -(float)tileMap.GetLength(0) / 2f + 0.5f;
		offset.y = (float)tileMap.GetLength(1) / 2f - 0.5f;
	}

	[ContextMenu("Randomize Tiles")]
	void RandomizeTiles ()
	{
		ResetTileMap (); // Reset tilemap and detroy all the tile reference generated from previous actions

		tileMap = new int[mapWidth, mapHeight]; // Define size

		// Assign random sprite index for each cell in the TileMap
		for (int row = 0; row < tileMap.GetLength (1); row++) 
		{
			for (int col = 0; col < tileMap.GetLength (0); col++) 
			{
				tileMap [col, row] = Random.Range (0, tileSprites.Length);
			}
		}

		RenderTiles (); // Render map based on tileMap

		Debug.Log ("Randomize Tiles");
	}

	[ContextMenu("Hardcore Tiles")]
	void HardcodeTiles ()
	{
		ResetTileMap (); // Deletes all tiles

		tileMap = new int[3, 3] 
		{
			{ 0, 11, 22 },
			{ 1, 12, 23 },
			{ 2, 13, 24 }
		};

		mapWidth = tileMap.GetLength (0);
		mapHeight = tileMap.GetLength (1);

		RenderTiles ();
	}

	[ContextMenu("Load Tiles From Text")]
	void LoadTilesFromText ()
	{
		ResetTileMap ();

		// Read from text
		TextAsset textFile = Resources.Load("TileMap") as TextAsset;
		string data = textFile.text;

		// Get width
		//data = data.Substring (data.IndexOf ("width=") + 6);
		//mapWidth = int.Parse (data.Substring (0, data.IndexOf ("\n")));
		mapWidth = int.Parse(FindData(data, "width"));

		// Get height
		// Cut out the string to find height
		// Assign mapHeight value
		//data = data.Substring (data.IndexOf ("height=") + 6);
		//mapHeight = int.Parse (data.Substring (0, data.IndexOf ("\n")));
		mapHeight = int.Parse(FindData(data, "height"));

		tileMap = new int [mapWidth, mapHeight];

		// Get data
		Debug.Log(data.Substring(data.IndexOf("data") + 7));
		string[] mapDataRow = data.Substring(data.IndexOf("data") + 7).Split('\n');

		for(int row = 0; row < mapHeight; row++)
		{
			Debug.Log(mapDataRow[row]);
			string[] mapDataCol = mapDataRow[row].Split(',');

			for(int col = 0; col < mapWidth; col++)
			{
				Debug.Log(mapDataCol[col]);
				// populate tilemap with data
				tileMap[col, row] = int.Parse(mapDataCol[col]) - 1;
			}
		}

		// Cut out the string to find "data="
		// Get an array of string[] using Split
		//string[] allData = data.Split (data);

		// populate tilemap with data
		// Use Array, do magic 

		Debug.Log (data);

		RenderTiles ();
	}

	string FindData(string data, string findingData)
	{
		data = data.Substring(data.IndexOf(findingData) + findingData.Length+1);
		data = data.Substring(0,data.IndexOf("\n"));

		return data;
	}

	void RenderTiles ()
	{	
		// Determine size of map
		tileRefs = new GameObject [tileMap.GetLength (0), tileMap.GetLength (1)];

		RecalculateOffsets ();

		GameObject go = Resources.Load ("Renderers/Tile") as GameObject;

		// Load tiles
		for (int row = 0; row < tileMap.GetLength(1); row++) 
		{
			for (int col = 0; col < tileMap.GetLength(0); col++) 
			{
				GameObject tile = Instantiate(go);
				tile.GetComponent<Tile> ().spriteRenderer.sprite = tileSprites [tileMap [col, row]];
				tileRefs [col, row] = tile;
				tile.transform.position = new Vector3 (col, -row, 0f) + offset;
			}
		}
	}
}