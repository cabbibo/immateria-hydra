using UnityEngine;
using System.Collections;

public class Slider : MonoBehaviour {

  GameObject Rod;
  GameObject Marker;
  GameObject Grabber;
  GameObject Text;

  public float Value;

  private Vector3 localPos;

	// Use this for initialization
	void Start () {

    SetGameObjects();
   
	
	}
	
	// Update is called once per frame
	void Update () {

    localPos = transform.worldToLocalMatrix.MultiplyPoint( Grabber.transform.position );
    localPos.Scale( Vector3.forward );
    if(localPos.magnitude > 1 ){ localPos.Normalize(); }
    Marker.transform.localPosition = localPos;

    Value = (localPos.z + 1.0f) / 2.0f;

    if(Grabber.GetComponent<MoveByController>().moving == false ){
      Grabber.transform.position = Marker.transform.position;
    }
	}

  public void SetValue(float v){

    Value = v;

    if( Marker == null ){
      SetGameObjects();
    };

    print( v );

    Marker.transform.localPosition = new Vector3( 0, 0, (v * 2.0f) - 1);
    Grabber.transform.position = Marker.transform.position;


  }

  public void SetSliderName(string s){

    if( Text == null ){
      SetGameObjects();
    };
    
    Text.GetComponent<TextMesh>().text = s;
  }

  private void SetGameObjects(){

    foreach (Transform child in transform){
      if( child.tag == "Marker" ){ Marker = child.gameObject; }
      if( child.tag == "Rod" ){ Rod = child.gameObject; }
      if( child.tag == "Grabber" ){ Grabber = child.gameObject; }
      if( child.tag == "Text" ){ Text = child.gameObject; }
    }

  }
  
}
