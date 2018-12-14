using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class DrunkardsMapGen : MonoBehaviour {

	public GameObject[] mapTile;
	public GameObject[] Objects;
	public int mapSize;
	int[,] mapLayout;
	public bool useSeed = false;
	public int seed;
	int currentCoordRow;
	int currentCoordCol;
	public int maxSteps = 100;
	[SerializeField]
	int currentStep = 0;
	public int stepCountdown;
	Transform parent;
	[SerializeField]
	private float linearChance;
	public float linearChanceDefault;
	public float linearChanceChange;
	int col;
	int row;


	public GameObject cursor;

	int previousDirection = -1;
	int nextTile = -1;

	bool isGenerating = false;

	[Header("Debugging")]
	public bool useSpeed;
	public float mapSpeed = 0.065f;

	void Start()
	{
		
	}

	//User Input
	//Initiates Map Generation
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.F) && !isGenerating)
		{
			MapCleanUp();
			currentStep = 0;
			mapLayout = new int[mapSize, mapSize];
			if (!useSeed)
				seed = DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + DateTime.Now.Millisecond * 2344;

			UnityEngine.Random.InitState(seed);

			if (!useSpeed)
				MapGeneration();
			else
				StartCoroutine(MapGenerator());
		}

	}

	//Generates the map in a coroutine (For debug and fun)
	IEnumerator MapGenerator()
	{

		if (GameObject.Find("LEVEL_DATA"))
			Destroy(GameObject.Find("LEVEL_DATA"));

		parent = new GameObject("LEVEL_DATA").transform;
		isGenerating = true;
		currentCoordRow = mapSize / 2;
		currentCoordCol = mapSize / 2;
		mapLayout[currentCoordRow, currentCoordCol] = 1;

		
		//Does the map generation and spawns the floor tiles
		while(currentStep < maxSteps)
		{
			yield return new WaitForSeconds(mapSpeed);
			if (Move(UnityEngine.Random.Range(0, 4)))
			{
				mapLayout[currentCoordRow, currentCoordCol] = 1;
				GameObject go = Instantiate(mapTile[1], new Vector3(currentCoordCol, 0, currentCoordRow + 1), transform.rotation);
				cursor.transform.position = new Vector3(go.transform.position.x, 3, go.transform.position.z);
				if (go != null)
					go.transform.SetParent(parent);

				currentStep++;
			}
		}

		//MapSpawn();
		isGenerating = false;
		Debug.Log("Finished Generation");
		SecondPass();
	}

	//Generates the map
	void MapGeneration()
	{
		if (GameObject.Find("LEVEL_DATA"))
			Destroy(GameObject.Find("LEVEL_DATA"));

		parent = new GameObject("LEVEL_DATA").transform;
		isGenerating = true;
		currentCoordRow = mapSize / 2;
		currentCoordCol = mapSize / 2;
		mapLayout[currentCoordRow, currentCoordCol] = 1;



		while (currentStep < maxSteps)
		{
			if (Move(UnityEngine.Random.Range(0, 4)))
			{
				mapLayout[currentCoordRow, currentCoordCol] = 1;
				GameObject go = Instantiate(mapTile[1], new Vector3(currentCoordCol, 0, currentCoordRow + 1), transform.rotation);
				cursor.transform.position = new Vector3(go.transform.position.x, 3, go.transform.position.z);
				if (go != null)
					go.transform.SetParent(parent);

				currentStep++;
			}
		}

		//MapSpawn();
		isGenerating = false;
		Debug.Log("Finished Generation");
		SecondPass();
	}

	//After the Map Generation has run, this will check the map for patterns and add collectibles
	void SecondPass()
	{

		//Loops through the map array and selects 3x3 sections to compare to a pattern
		for (int i = 0; i < mapSize-3; i++)
		{
			for (int j = 0; j < mapSize-3; j++)
			{

				int[,] testArray = new int[3, 3];

				//Debug.Log(testArray.Length + " | " + mapLayout.Length);

				for (int k = 0; k < 3; k++)
				{
					for (int l = 0; l < 3; l++)
					{
						testArray[k, l] = mapLayout[i + k, j + l];
						col = i + k;
						row = j + l;
						//Debug.Log("Test Array: " + k + ", " + l);
						//Debug.Log("Map: " + i + ", " + j);
					}
				}

				int[,] pattern1 = new int[3, 3]
				{
					{ 0, 0, 0},
					{ 0, 1, 0},
					{ 0, 1, 0},
				};

				int[,] pattern2 = new int[3, 3]
				{
					{ 0, 1, 0},
					{ 0, 0, 0},
					{ 0, 0, 0},
				};

				for (int a = 0; a < 3; a++)
				{
					for (int b = 0; b < 3; b++)
					{
						if (CompareArray(testArray, pattern1, col, row))
						{
							Debug.LogWarning("Pattern Found");
							
							ReplacePattern(col, row);
						}
						else if (CompareArray(testArray, pattern2, col, row))
						{
							Debug.LogWarning("Pattern Found");
							
							ReplacePattern(col, row);
						}
						else
						{
							//Debug.LogWarning("No Pattern Occourance");
						}
					}
				}
			}
		}

		gameObject.GetComponent<SpawnPlayer>().Spawn(cursor.transform.position);

	}

	//Finds the location of pattern that was found and puts collectibles there
	void ReplacePattern(int patternCol, int patternRow)
	{
		for (int i = patternCol; i > patternCol - 3; i--)
		{ 
			for (int j = patternRow; j > patternRow - 3; j--)
			{
				if (mapLayout[i, j] == 1)
				{
					mapLayout[i, j] = 2;
					Instantiate(Objects[0], new Vector3(j, 1, i + 1), transform.rotation);
				}
			}
		}
	}

	//Checks if two arrays match
	public bool CompareArray(int[,] array1, int[,] array2, int patternCol, int patternRow)
	{
		if (array1.Length != array2.Length)
			return false;

		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				if (array1[i, j] != array2[i, j])
					return false;
			}
		}

		return true;
	}

	/* Old Map Spawning Code
	void MapSpawn()
	{
		Vector3 spawnPosition = Vector3.zero;
		for (int i = 0; i < mapSize; i++)
		{
			spawnPosition.z += 1;
			spawnPosition.x = 0;
			for (int j = 0; j < mapSize; j++)
			{
				GameObject go = null;
				if (mapLayout[i, j] == 0)
				{
					go = Instantiate(mapTile[0], spawnPosition, transform.rotation);
				}
				//else if (mapLayout[i, j] == 1)
				//{
				//	go = Instantiate(mapTile[1], spawnPosition, transform.rotation);
				//}
				else
				{
					//go = Instantiate(mapTile[3], spawnPosition, transform.rotation);
				}
				spawnPosition.x += 1;

				if (go != null)
					go.transform.SetParent(parent);
			}
		}
	}
	*/

	//Handles the rules for map generation
	bool Move(int Direction)
	{
		//Checks the chance of moving in the same direction and sets it
			if (previousDirection >= 0)
			{
				if (Direction != previousDirection && UnityEngine.Random.value <= linearChance)
				{
					Direction = previousDirection;
				}
				else
				{
					previousDirection = Direction;
				}
			}
			else
			{
				previousDirection = Direction;
			}

			//Adds a rule which means if there is a floor tile after the next empty one, don't move towards it
			switch (Direction)
			{
				case 0: // north
				try
				{
					if (mapLayout[currentCoordRow - 2, currentCoordCol] == 1 && mapLayout[currentCoordRow - 1, currentCoordCol] == 0 || mapLayout[currentCoordRow - 1, currentCoordCol - 1] == 1 && mapLayout[currentCoordRow - 1, currentCoordCol + 1] == 1 && mapLayout[currentCoordRow - 1, currentCoordCol] == 0)
					{																												 
						//Debug.Log("Blocked North");																					 
						break;																										 
					}																												 
				}																													 
				catch { }																											 
				currentCoordRow--;																									 
					break;																											 
				case 1: // east																										 
				try																													 
				{																													 
					if (mapLayout[currentCoordRow, currentCoordCol + 2] == 1 && mapLayout[currentCoordRow, currentCoordCol + 1] == 0 || mapLayout[currentCoordRow - 1, currentCoordCol + 1] == 1 && mapLayout[currentCoordRow + 1, currentCoordCol + 1] == 1 && mapLayout[currentCoordRow, currentCoordCol + 1] == 0)
					{
						//Debug.Log("Blocked East");
						break;
					}
				}catch{ }
					currentCoordCol++;
					break;
				case 2: // south
				try
				{
					if (mapLayout[currentCoordRow + 2, currentCoordCol] == 1 && mapLayout[currentCoordRow + 1, currentCoordCol] == 0 || mapLayout[currentCoordRow + 1, currentCoordCol - 1] == 1 && mapLayout[currentCoordRow + 1, currentCoordCol + 1] == 1 && mapLayout[currentCoordRow + 1, currentCoordCol] == 0)
					{
						//Debug.Log("Blocked South");
						break;
					}
				}
				catch { }
				currentCoordRow++;
					break;	
				case 3: // west
				try
				{
					if (mapLayout[currentCoordRow, currentCoordCol - 2] == 1 && mapLayout[currentCoordRow, currentCoordCol - 1] == 0 || mapLayout[currentCoordRow + 1, currentCoordCol - 1] == 1 && mapLayout[currentCoordRow - 1, currentCoordCol + 1] == 1 && mapLayout[currentCoordRow, currentCoordCol - 1] == 0)
					{
						//Debug.Log("Blocked West");
						break;
					}
				}
				catch { }
				currentCoordCol--;
					break;
		}
			//Makes sure we don't try and go outside of the array
			if (currentCoordRow < 0)
			{
				currentCoordRow = 0;
				return false;
			}
			if (currentCoordCol < 0)
			{
				currentCoordCol = 0;
				return false;
			}
			if (currentCoordRow >= mapSize)
			{
				currentCoordRow = mapSize - 1;
				return false;
			}
			if (currentCoordCol >= mapSize)
			{
				currentCoordCol = mapSize - 1;
				return false;
			}
			return true;
	}

	void MapCleanUp()
	{
		GameObject[] objects;

		objects = GameObject.FindGameObjectsWithTag("Collectable");

		foreach (GameObject obj in objects)
		{
			Destroy(obj);
		}

	}

}
