using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour {

	public GameObject player;

	public void Spawn(Vector3 pos)
	{
		Instantiate(player, pos, transform.rotation);
	}
}
