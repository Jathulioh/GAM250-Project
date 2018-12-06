using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DrunkardsMapGen : MonoBehaviour {

	public GameObject[] mapTile;
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

	public GameObject cursor;

	int previousDirection = -1;
	int nextTile = -1;

	public float mapSpeed = 0.065f;

	bool isGenerating = false;
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.S) && !isGenerating)
		{
			currentStep = 0;
			mapLayout = new int[mapSize, mapSize];
			if (!useSeed)
				seed = DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + 2344;

			UnityEngine.Random.InitState(seed);

		//	UnityEngine.Random.State oldState = UnityEngine.Random.state;
			StartCoroutine(MapGenerator());
		//	UnityEngine.Random.state = oldState;

		}

	}

	IEnumerator MapGenerator()
	{
		if (GameObject.Find("LEVEL_DATA"))
			Destroy(GameObject.Find("LEVEL_DATA"));

		parent = new GameObject("LEVEL_DATA").transform;
		isGenerating = true;
		currentCoordRow = mapSize / 2;
		currentCoordCol = mapSize / 2;
		mapLayout[currentCoordRow, currentCoordCol] = 1;

		

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
	}

	void SecondPass()
	{

		//Find and place spawn
		for (int i = 0; i < mapSize; i++)
		{
			for (int j = 0; j < mapSize; j++)
			{

				if (mapLayout[j, i] == 1 && mapLayout[j + 1,i] == 0 && mapLayout[j - 1, i] == 0 && mapLayout[j, i + 1] == 0)
				{
					int[,] test = new int[3, 4]
					{
						{ 1, 1, 1, 1 },
						{ 1, 3, 0, 0 },
						{ 1, 1, 1, 1 },
					};
				}

			}
		}

	}

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

	bool Move(int Direction)
	{

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

			switch (Direction)
			{
				case 0: // north
				try
				{
					if (mapLayout[currentCoordRow - 2, currentCoordCol] == 1 && mapLayout[currentCoordRow - 1, currentCoordCol] == 0 || mapLayout[currentCoordRow - 1, currentCoordCol - 1] == 1 && mapLayout[currentCoordRow - 1, currentCoordCol + 1] == 1 && mapLayout[currentCoordRow - 1, currentCoordCol] == 0)
					{																												 
						Debug.Log("Blocked North");																					 
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
						Debug.Log("Blocked East");
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
						Debug.Log("Blocked South");
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
						Debug.Log("Blocked West");
						break;
					}
				}
				catch { }
				currentCoordCol--;
					break;
		}
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
}
