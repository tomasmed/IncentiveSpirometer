using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour {

	public Text clock;

	int minutes = 60;
	int seconds = 0;

	// Use this for initialization
	void Start () {
		StartCoroutine (HourCountdown());	
	}

	void Update(){
		if (seconds < 10) {
			clock.text = minutes + ":0" + seconds;
		} else {
			clock.text = minutes + ":" + seconds;
		}

	}

	IEnumerator HourCountdown(){
		while (minutes >= 0) {
			yield return new WaitForSeconds (1f);
			seconds--;
			if (seconds < 0) {
				minutes--;
				seconds = 59;
			}
		}
		Application.LoadLevel (2);
	}
}
