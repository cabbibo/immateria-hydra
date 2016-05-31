using UnityEngine;
using System.Collections;

public class scaleupdown : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

    float s = .7f + Mathf.Sin( Time.time * .5f ) * .3f;
    transform.localScale = new Vector3( s,s,s);
	
	}
}
