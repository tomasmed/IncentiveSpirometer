using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {


    public Transform CameraTransform;
    public Transform target;
    public Transform RightEdge;

    public float speed; 

    private Transform trans;

	// Use this for initialization
	void Start () {
        trans = this.transform;
	}
	
	// Update is called once per frame
	void Update () {
        if (target.position.x >= RightEdge.position.x)
        {
            CameraTransform.position = new Vector3(CameraTransform.position.x + (speed * Time.deltaTime), CameraTransform.position.y, CameraTransform.position.z);
            RightEdge.position = new Vector3(RightEdge.position.x + (speed * Time.deltaTime), RightEdge.position.y, RightEdge.position.z);
        }
	}
}
