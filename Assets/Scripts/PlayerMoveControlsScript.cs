using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMoveControlsScript : MonoBehaviour {

	Rigidbody rigidBody;
	Vector3 movement;
	public float moveSpeed;
	public float jumpSpeed;
	public float gravity;
	public float fallSpeed;

	Ray DistToGround;
	RaycastHit DistToGroundHit;
	bool isGrounded;

	void Start () {

		rigidBody = gameObject.GetComponent<Rigidbody> ();

	}

	void Update () {
		isGroundedCheck();
		Movement ();
	}

	void Movement() {

		if (Input.GetButtonDown("Jump")) {
			Debug.Log(isGrounded);
			if (isGrounded == true) {
				rigidBody.AddForce(Vector3.up * jumpSpeed, ForceMode.VelocityChange);
			}
		}

		if (isGrounded == true)
		{
			//Handles forward and backward
			movement = Input.GetAxis("Vertical") * transform.forward * moveSpeed;
			//Handles left and right
			movement += Input.GetAxis("Horizontal") * transform.right * moveSpeed;
		}
		//Gravity
		if(isGrounded == false) {
			fallSpeed += gravity * Time.deltaTime;
			movement -= transform.up * fallSpeed;
		}
		else
		{
			fallSpeed = 0f;
		}

		//Executes the above
		rigidBody.velocity = movement * Time.deltaTime;

	}

	void isGroundedCheck(){

		DistToGround = new Ray (transform.position, -transform.up);

		if (Physics.Raycast (DistToGround, out DistToGroundHit)) {

			if (DistToGroundHit.distance <= 1.1) {
				isGrounded = true;
			} else {
				isGrounded = false;
			}

		}
	}

}
