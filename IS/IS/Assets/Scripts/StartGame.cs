using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class StartGame : MonoBehaviour {
    public string url;
    public InputField urlinput;

	public void GameStart(){
        //urlinput = GameObject.Find("URLInput").GetComponent<Text>
        url = urlinput.GetComponentInChildren<Text>().text;
        Debug.Log(url);
		Application.LoadLevel (1);
	}
    
}
