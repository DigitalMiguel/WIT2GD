using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	// key variables
	private bool hasKey;
	private GameObject item;

	// Moving Variables
	private float rotationY = 0f;
	private float rotationX = 0f;
	private float sensitivityX = 15f;
	private float sensitivityY = 15f;
	public float walkSpeed;
	private float defaultSpeed;
	private Vector3 moveDirection;
	private Rigidbody rb;

	// For slowing down over a period
	private IEnumerator slowTimer;
	private IEnumerator itemTimer;


	// Update is called once per frame
	public Camera cam;

	void Start(){
		rb = GetComponent<Rigidbody> ();
		defaultSpeed = walkSpeed;
		hasKey = false;


		Screen.lockCursor = true;
		Cursor.visible = true;
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
		cam.transform.localEulerAngles = new Vector3 (-rotationY, 0, 0);

		// Unlock cursor when escape key is pressed
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Screen.lockCursor = false;
		
		}
			


	}

	void FixedUpdate (){

		Move ();

	}

	void Move(){

		rb.velocity = moveDirection * walkSpeed * Time.deltaTime;

	}
		
	void OnCollisionEnter(Collision other){

		if (other.gameObject.tag == "slowItem") {
			item = other.gameObject;
			item.gameObject.SetActive (false);

		}

		if ( other.gameObject.tag == "speedUp" ) {
			slowTimer = changeSpeed (2.0f, defaultSpeed * 2);
			StartCoroutine (slowTimer);
			other.gameObject.SetActive (false);

		}

		if ( other.gameObject.tag == "slowDown" ) {
			slowTimer = changeSpeed (2.0f, defaultSpeed / 2);
			StartCoroutine (slowTimer);

			itemTimer = deactivateTime (2.0f, other);
			StartCoroutine (itemTimer);

		}

		if (other.gameObject.tag == "freeze") {
			slowTimer = changeSpeed (2.0f, 0f);
			StartCoroutine (slowTimer);

			other.gameObject.SetActive (false);

		}

		if (other.gameObject.tag == "key") {
			hasKey = true;
			other.gameObject.SetActive (false);

		}

		if (other.gameObject.tag == "door" && hasKey) {
			Debug.Log ("door opens");
			other.gameObject.SetActive (false);

		} else {
			Debug.Log ("No key");

		}

	}
		
	private IEnumerator changeSpeed(float waitTime, float newSpeed)
	{
		// Set the new speed
		walkSpeed = newSpeed;
		while (true)
		{
			yield return new WaitForSeconds(waitTime);
			walkSpeed = defaultSpeed;
			StopCoroutine (slowTimer);
			Debug.Log ("Slow down end");

		}
	}

	private IEnumerator deactivateTime(float waitTime, Collision other)
	{
		//Deactivate the items
		other.gameObject.SetActive (false);

		while (true)
		{
			yield return new WaitForSeconds(waitTime);
			other.gameObject.SetActive (true);
			StopCoroutine (slowTimer);

		}
	}
		
}
