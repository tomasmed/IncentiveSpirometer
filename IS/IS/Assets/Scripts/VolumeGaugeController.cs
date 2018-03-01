using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using UnityEngine.UI;


public class VolumeGaugeController : MonoBehaviour
{

    public KeyCode input_b;
    public float vertical_shift = 0.1f;
    private float res = 0;
    private float vol = 0;
    private float flow = 0;

    public InputField infield;
    public Text intext;
    public Button yourButton ;
    public bool readytosample = false;


    private string strtoparse = "";
    public string[] strarray = new string[] { };
    private string[] strsep = new string[] {","};

    public Transform flow_bobbin;
    public Transform vol_bobbbin;

    private static int timer;
    public static VolumeGaugeController S;

    private static string url = "";


    // Use this for initialization
    void Start()
    {
        timer = 0;
        S = this;

        Button btn = yourButton.GetComponent<Button>();
        btn.onClick.AddListener(changesamplesettings);
    }

    // Update is called once per frame
    void Update()
    {

        //button.gameObject.GetComponent<Button>().onClick(AddListener(Debug.Log("test"))); // onClick(Debug.Log("Button was clicked"));
        if (readytosample)
        {
            timer++;
            if (timer > 30)
            {
                Debug.Log("Starting Coroutine to get Input");
                timer = 0;
                res = 0;
                StartCoroutine(GetText(res));
            }
            flow_bobbin.position = move_bobbin(flow_bobbin, flow);
            vol_bobbbin.position = move_bobbin(vol_bobbbin, vol);
            if (Input.GetKeyDown(input_b))
            {

                //this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + res, this.transform.position.z);
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
            Debug.Log(www.downloadHandler.text);
            strtoparse = www.downloadHandler.text;
            strarray = strtoparse.Split(strsep,StringSplitOptions.None);

            //vol = UnityEngine.Random.Range(0, 100);
            //flow = UnityEngine.Random.Range(0, 100);

            Debug.Log("Flow was assigned to: " + flow + " and volume was assigned to:" + vol);


            vol = float.Parse(strarray[0]);
            flow = float.Parse(strarray[1]);

            
        }
    }


    private Vector3 move_bobbin(Transform bobbin, float change)
    {
        if ((bobbin.position.y / 5 * 100 + 3) < change)
            return new Vector3(bobbin.transform.position.x, bobbin.position.y + (vertical_shift * Time.deltaTime), bobbin.transform.position.z);

        else if ((bobbin.position.y / 5 * 100 - 3) > change)
            return new Vector3(bobbin.transform.position.x, bobbin.position.y - (vertical_shift * Time.deltaTime), bobbin.transform.position.z);
        else return bobbin.transform.position;
    }


    public void changesamplesettings()
    {
       // Debug.Log("you clicked the button! the ip is: " + intext.text);
        url = intext.text;
        readytosample = !readytosample;
    }

}