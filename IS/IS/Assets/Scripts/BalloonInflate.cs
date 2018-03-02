using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using UnityEngine.UI;


public class BalloonInflate : MonoBehaviour
{
    public Transform flow_bobbin;
    public Transform vol_bobbin;

    public float vertical_shift = 5f;
    private float res = 0;
    private float vol = 0;
    private float flow = 0;
    private bool new_vals; // this is going to track whether we read new values from the controller
    private float new_flow = 0; // new values go here, so we can compare to old values
    [SerializeField] private float new_vol = 0; // new values go here, so we can compare to old values

    public bool readytosample = false;


    private string strtoparse = "";
    public string[] strarray = new string[] { };
    private string[] strsep = new string[] { "," };


    // Timers
    private static int timer; // track screen updates & try to get new values every 30th update
    private static int failTimer = 0; //track time with flow meter in fail state
    [SerializeField] private int noChangeTimer = 0; // track time with level in a constant or declining state
    public static BalloonInflate S;

    //These are definitions for thresholds and ranges that will need to be changed eventually.
    public static float flow_limit = 67; //anything higher than this limit is unacceptable flow rate
    public static float bad_flow_frames = 6; // if the flow rate stays bad for this many frames or more, fail
    public static float done_frames = 3; // if the volume stays level or decreases for this many frames, move on
    public float scale_factor = .1f; // This is how much to scale the shape change based on the difference between last & current input

    [SerializeField]private string url = "";
    private static Vector3 origScale; // Original scale of the sprite
    private static float max_vol = 0f; // Maximum volume achieved so far

    public bool increasing = false;
    public bool decr = false;

    // Use this for initialization
    void Start()
    {
        timer = 0;
        S = this;

        //Button btn = yourButton.GetComponent<Button>();
        //btn.onClick.AddListener(changesamplesettings);
        changesamplesettings();
    }

    // Update is called once per frame
    void Update()
    {
 
        //button.gameObject.GetComponent<Button>().onClick(AddListener(Debug.Log("test"))); // onClick(Debug.Log("Button was clicked"));
        if (readytosample)
        {
            flow_bobbin.position = move_bobbin(flow_bobbin, flow);
            vol_bobbin.position = move_bobbin(vol_bobbin, vol);
            timer++;
            if (timer > 30)
            // try to read from the controller ever 30 frames
            {
                //Debug.Log("Starting Coroutine to get Input");
                timer = 0;
                failTimer = 0;
                res = 0;
                StartCoroutine(GetText(res));
            }

            if (new_vals)
            {
                //Debug.Log("Current flow: " + new_flow + " Previous flow: " + flow);
                //Debug.Log("Current volume: " + new_vol + " Previous volume: " + vol);

                // New values: check if the vol has stayed the same or gone down for xxx counts
                if (new_vol <= vol) {
                    noChangeTimer++;
                } else {
                    noChangeTimer = 0;
                }
                if (new_flow >= flow_limit) {
                    // Show the warning animation & increase count of bad frames
                    failTimer++; //Note: this might need to be updated to bad_flow_frames+=timer or something more sophisticated
                } else {
                    failTimer = 0;
                }

                if (failTimer >= bad_flow_frames) {
                    // Go to failure animation
                    // ... or just reset size to normal?
                    //balloon.localScale = origScale;
                    max_vol = 0;
                }
                else if (noChangeTimer >= done_frames) {
                    // Move on to the next step of animation
                    //patient probably stopped breathing and the bobbin is falling evaulate the Score and send it to The movement controller

                    physicsCube.S.animator.SetBool("doneBreathing",true);
                    physicsCube.S.score = max_vol;
                    physicsCube.S.inflating = false;
                    physicsCube.S.readytojump = true;
                    max_vol = 0;

                }

                // Now grow the balloon, if the volume level went up
                if (new_vol > max_vol) {
                    Debug.Log("Max volume attained: " + new_vol);
                    max_vol = new_vol;
                    physicsCube.S.inflating = true;
                    //balloon.localScale = new Vector3(origScale.x * scale_factor * new_vol, origScale.y * scale_factor * new_vol);
                }
                // store these new values as the current values
                vol = new_vol;
                flow = new_flow;
                new_vals = false;
            }
        }
    }




    IEnumerator GetText(float res)
    {
        //"https://www.random.org/integers/?num=2&min=1&max=10&col=1&base=10&format=plain&rnd=new"); 
        //"https://eos.mpogresearch.org/Reports/NG.aspx"
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.Send();
        //yield return www.SendWebRequest();

        if (www.isError)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            // Debug.Log(www.downloadHandler.text);
            strtoparse = www.downloadHandler.text;
            strarray = strtoparse.Split(strsep, StringSplitOptions.None);

            //vol = UnityEngine.Random.Range(0, 100);
            //flow = UnityEngine.Random.Range(0, 100);

            /* Ideally these next three commands should be an atomic unit... 
                I'm not sure how to do that right now, but it's safer to read the new values & not think they're new until the next
                frame than to see that we have new values when we actually don't. */


            //if (increasing) new_vol+= 5f;
            //if (decr && new_vol > 1) new_vol-= 5f;


            //vol is the one that needs to keep getting larger and get as large as possible
            new_vol = float.Parse(strarray[0]);
            //flow needs to stay within the acceptable range
            new_flow = float.Parse(strarray[1]);
            new_vals = true;



        }
    }

    public void changesamplesettings()
    {
        //url = "https://eos.mpogresearch.org/Reports/NG.aspx";
        url = StartGame.S.url;
        readytosample = !readytosample;
        if (readytosample)
        {
            //origScale = balloon.localScale;
        }
    }

    private Vector3 move_bobbin(Transform bobbin, float change)
    {
        if ((bobbin.position.y / 15 * 100 + 3) < change)
            return new Vector3(bobbin.transform.position.x, bobbin.position.y + (vertical_shift * Time.deltaTime), bobbin.transform.position.z);

        else if ((bobbin.position.y / 15 * 100 - 3) > change)
            return new Vector3(bobbin.transform.position.x, bobbin.position.y - (vertical_shift * Time.deltaTime), bobbin.transform.position.z);
        else return bobbin.transform.position;
    }

}