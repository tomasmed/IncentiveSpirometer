using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class physicsCube : MonoBehaviour {

	public float score = 80;
	public float distance = 1000f;

	public bool hasMoved = false;
	public bool doneWalking = true;
	public float walkDistance = 5f;
	public float walkSpeed = 2f;

	public int obstaclesCleared = 0;

	public float targetPosition;
	bool currentlyWalking = false;
	Rigidbody rb;

	// Use this for initialization
	void Start () {
		rb = this.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		if (!hasMoved && doneWalking) {
			hasMoved = true;
			if (score > 80) {
				perfect ();
			} else if (score > 30) {
				good ();
			} else {
				fail ();
			}
		}
		else if (!doneWalking) {
			if (transform.position.x < targetPosition) {
				rb.velocity = Vector3.right * walkSpeed;
			} else {
				rb.velocity = Vector3.zero;
				transform.position = new Vector3 (targetPosition, transform.position.y, transform.position.z);
				doneWalking = true; 
			}
		}
	}

	void perfect(){
		rb.AddForce (new Vector3 (distance/4, distance/1.5f, 0));
	}

	void good(){
		rb.AddForce (new Vector3 (distance/5.25f, distance/2, 0));
		StartCoroutine (goodStutter ());
	}

	IEnumerator goodStutter(){
		yield return new WaitForSeconds (1.9f);
		rb.AddForce (new Vector3 (0, distance / 3, 0));
		yield return new WaitForSeconds (.7f);
		rb.AddForce (new Vector3 (0, distance / 4, 0));
		yield return new WaitForSeconds (.7f);
		rb.AddForce (new Vector3 (0, distance / 4, 0));
	}

	void fail(){
	}

	IEnumerator delayCollisionCheck(){
		yield return new WaitForSeconds (.5f);
		if (hasMoved) {
			doneWalking = false;
			hasMoved = false;
		}
	}
		

	void OnCollisionEnter(Collision other){
		if (hasMoved) {
			StartCoroutine(delayCollisionCheck());
		}
		targetPosition = Mathf.RoundToInt(transform.position.x + walkDistance);
	}

	void OnTriggerEnter(Collider collider){
		obstaclesCleared++;
	}
}
