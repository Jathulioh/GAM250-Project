using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneration : MonoBehaviour {

	public GameObject[] mapTile;
	public int mapSize;
	int[,] mapLayout;
	public int seed;

	// Use this for initialization
	void Start () {

		Random.InitState(seed);

		mapLayout = new int[mapSize, mapSize];

		foreach(int element in mapLayout)
		{
			Debug.Log(element);
		}
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown(KeyCode.S))
		{
			Random.InitState(seed);
			Random.State oldState = Random.state;
			MapGenerator();
			Random.state = oldState;

		}

	}

	void MapGenerator()
	{
		for (int i = 0; i < mapSize; i++)
		{
			for (int j = 0; j < mapSize; j++)
			{
				mapLayout[j, i] = Random.Range(0, 2);
			}
		}
		MapSpawn();
	}

	void MapSpawn()
	{
		if (GameObject.Find("LEVEL_DATA"))
			Destroy(GameObject.Find("LEVEL_DATA"));

		Transform parent = new GameObject("LEVEL_DATA").transform;
		Vector3 spawnPosition = Vector3.zero;
		for (int i = 0; i < mapSize; i++)
		{
			spawnPosition.z += 1;
			spawnPosition.x = 0;
			for (int j = 0; j < mapSize; j++)
			{
				GameObject go = null;
				if(mapLayout[i,j] == 0)
				{
					go = Instantiate(mapTile[0], spawnPosition, transform.rotation);
				}else if(mapLayout[i,j] == 1)
				{
					go = Instantiate(mapTile[1], spawnPosition, transform.rotation);
				}
				else
				{
					go = Instantiate(mapTile[3], spawnPosition, transform.rotation);
				}
				spawnPosition.x += 1;

				if (go != null)
					go.transform.SetParent(parent);
			}
		}
	}
}
