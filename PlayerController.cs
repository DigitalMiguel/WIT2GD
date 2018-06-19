using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	private float rotationY = 0f;
	private float rotationX = 0f;
	private float sensitivityX = 15f;
	private float sensitivityY = 15f;
	public float walkSpeed;
	private Vector3 moveDirection;
	private Rigidbody rb;
	// Update is called once per frame

	public Camera cam;

	void Start(){
		rb = GetComponent<Rigidbody> ();
	}

	void Update () {

		//Movement
		float horizontalMovement = Input.GetAxisRaw("Horizontal");
		float verticalMovement = Input.GetAxisRaw("Vertical");

		moveDirection = (horizontalMovement * transform.right + verticalMovement * transform.forward).normalized;

		//Rotation
		rotationY += Input.GetAxis ("Mouse Y") * sensitivityY;
		rotationX += Input.GetAxis ("Mouse X") * sensitivityX;

		rotationY = Mathf.Clamp (rotationY, -60f, 60f);

		transform.localEulerAngles = new Vector3 (0, rotationX, 0);
		cam.transform.localEulerAngles = new Vector3 (rotationY, 0, 0);
	}

	void FixedUpdate (){

		Move ();

	}

	void Move(){

		rb.velocity = moveDirection * walkSpeed * Time.deltaTime;

	}
}
