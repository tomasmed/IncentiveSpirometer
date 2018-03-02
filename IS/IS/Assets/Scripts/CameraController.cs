using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {


    public Transform CameraTransform;
    public Transform target;
    public Transform RightEdge;
    public Transform LeftEdge;

    public float speed; 

    private Transform trans;
    private bool timetomove = false;

	// Use this for initialization
	void Start () {
        trans = this.transform;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (target.position.x >= RightEdge.position.x)
        {
            // LeftEdge.position = new Vector3(RightEdge.position.x + (speed * 1.2f * Time.deltaTime), RightEdge.position.y, RightEdge.position.z);
            timetomove = true;

        }
        if (timetomove)
        {
            CameraTransform.position = new Vector3(CameraTransform.position.x + (1.2f * speed * Time.deltaTime), CameraTransform.position.y, CameraTransform.position.z);

        }
        if (target.position.x <= LeftEdge.position.x)
        {
            timetomove = false;
            //RightEdge.position = new Vector3(RightEdge.position.x + (speed * 1.2f * Time.deltaTime), RightEdge.position.y, RightEdge.position.z);

        }
    }
}
