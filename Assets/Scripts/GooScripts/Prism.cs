using UnityEngine;
using System.Collections;

public class Prism : MonoBehaviour {

  // Defining some of render values
  public float _NoiseSize;
  public Vector3 _Color1;
  public Vector3 _Color2;

  public GameObject puppy;

  public DonutInfo info;
  public DonutInterface uniformInterface;

  private AudioSource audio;

  public bool activated = false;
  public float activationTime = 0.0f;
  public float activeVal = 0; // 0 -> 1  for fade in / fade out ! 
  public bool deactivating = false;
  public bool activating = false;
  public float deactivationTime = 0;

  public float activatingTime = 1;
  public float deactivatingTime = 1;

  

  // Use this for initialization
  void Awake() {

//    puppy = transform.parent.transform.Find("Puppy").gameObject;
    // Making each rendered object have its own instance


    uniformInterface = GetComponent<DonutInterface>();
    info             = GetComponent<DonutInfo>();

    audio = GetComponent<AudioSource>();
    select();


    GetComponent<Renderer>().material.SetInt( "_WHAT", 1 );
  
  }
  
  // Update is called once per frame
  void Update () {      

    if( activated == true ){

      if( activating == true ){
        activeVal = (Time.time - activationTime)  / activatingTime;
        audio.volume = activeVal;
        //print( activeVal );

        if( activeVal > 1 ){
          activating  = false;
          activeVal = 1;
          audio.volume = 1;
        }
      }

      if( deactivating == true ){
        activeVal = 1.0f - ((Time.time - deactivationTime)  / deactivatingTime);
        audio.volume = activeVal;

        if( activeVal < 0 ){
          deactivating  = false;
          activeVal = 0;
          audio.volume = 0;
          audio.Stop();
          activated = false;
        }
      }

    }

  
  }

  public void select(){

    activated = true;
    activationTime = Time.time;
    activating = true;

    uniformInterface.SetDonutInfo( transform.gameObject );
    
    audio.volume = 0;
    audio.clip = info.clip;
    audio.Play();

  }

  public void deselect(){
    deactivating = true;
    deactivationTime = Time.time;
  }


}
