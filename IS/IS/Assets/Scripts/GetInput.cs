using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GetInput : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //StartCoroutine(GetText());
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.G))
        { StartCoroutine(GetText());}
	}

    IEnumerator GetText()
    {
        UnityWebRequest www = UnityWebRequest.Get("https://www.random.org/integers/?num=1&min=1&max=99&col=1&base=10&format=plain&rnd=new");

        yield return www.Send();
        //yield return www.SendWebRequest();

        //if (www.isNetworkError)
        //{
        //    Debug.Log(www.error);
        //}
        //else
        {
            // Show results as text
            Debug.Log(www.downloadHandler.text);

            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
        }
    }




}
