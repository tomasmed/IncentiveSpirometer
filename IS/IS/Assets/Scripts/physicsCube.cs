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

    public static physicsCube S;


    public bool readytojump = false;
    public bool inflating = false;

    public Animator animator ;

    private void Awake()
    {
        S = this;
    }

    // Use this for initialization
    void Start () {
		rb = this.GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

    }
	
	// Update is called once per frame
	void Update () {
        
        //animator.SetFloat("Strafe", h);
        //animator.SetBool("Fire", fire);

        if (!hasMoved && doneWalking && readytojump)
        {
            hasMoved = true;
            animator.SetFloat("Score", score);
           
            if (score > 80)
            {
                //Play perfect animation
                perfect();
            }
            else if (score > 30)
            {
                //Play good animation

                good();
            }
            else
            {
                //Play fail animation

                fail();
                hasMoved = false;
            }
        }
        else if (!doneWalking)
        {
            if (transform.position.x < targetPosition)
            {
                //Play wlking animation

                rb.velocity = Vector3.right * walkSpeed;
            }
            else
            {
                rb.velocity = Vector3.zero;
                transform.position = new Vector3(targetPosition, transform.position.y, transform.position.z);
                doneWalking = true;
                animator.SetBool("DoneWalking", true);
                physicsCube.S.animator.SetBool("DoneBreathing", true);

            }
        }
        else {
            if (inflating) {
                //play ifnlating
                animator.SetBool("Readytobreathe", true);
            }
            else {
                //Play IDLE
                animator.SetBool("Readytobreathe", false);
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
        //rb.AddForce(new Vector3(0, distance / 1.5f, 0));

    }

    IEnumerator delayCollisionCheck(){
		yield return new WaitForSeconds (1.5f); // Maybe change this to 10 seconds and leave him hanging up there while people hold their breath? 
        //Maybe add randomness in there and change animation loop to run?
		if (hasMoved) {
			doneWalking = false;
            animator.SetBool("DoneWalking", false);
            physicsCube.S.animator.SetBool("DoneBreathing", true);

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
