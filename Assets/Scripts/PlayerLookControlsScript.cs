using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLookControlsScript : MonoBehaviour {

	GameObject playerCapsule;
	GameObject mainCamera;

	public float verticalLook;
	public float lookSensitivity;

	void Start () {
		
		playerCapsule = gameObject;
		mainCamera = GetComponentInChildren<Camera>().transform.gameObject;

		verticalLook = 0.0f;

		//Sets the cursor locked to the centre of the screen (And make it invisible)
		Cursor.lockState = CursorLockMode.Locked;
	}

	void Update () {


		//Vertical Mouse is different from horizontal to limit the max rotation up and down so you can't just spin

		//Setting Vertical
		verticalLook += -Input.GetAxis ("Mouse Y") * lookSensitivity;
		verticalLook = Mathf.Clamp (verticalLook, -75f, 75f);
		//Horizontal
		playerCapsule.transform.Rotate (0f, Input.GetAxis ("Mouse X") * lookSensitivity, 0f);
		//Vertical
		mainCamera.transform.eulerAngles = new Vector3 (verticalLook, playerCapsule.transform.eulerAngles.y, 0f);



	}

}
