using UnityEngine;
using System.Collections;

public class SilenceListener : MonoBehaviour {

	// Use this for initialization
	void Start () {

    //AudioListener al = GetComponent<AudioListener>();
    AudioListener.volume = 0;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
