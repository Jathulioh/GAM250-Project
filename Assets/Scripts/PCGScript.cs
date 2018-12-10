using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCGScript : MonoBehaviour {

	public int seed;

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.S))
		{
			Random.InitState(seed);

			PrintRandom("Step 1");
			PrintRandom("Step 2");

			Random.State oldState = Random.state;

			PrintRandom("Step 3");
			PrintRandom("Step 4");

			Random.state = oldState;

			PrintRandom("Step 5");
			PrintRandom("Step 6");

			Random.InitState(seed);

			PrintRandom("Step 7");
			PrintRandom("Step 8");
		}
	}

	void PrintRandom(string label)
	{
		Debug.Log(string.Format("{0} - RandomValue {1}", label, Random.Range(1, 100)));
	}
}
