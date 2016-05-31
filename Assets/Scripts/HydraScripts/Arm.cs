using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Arm : MonoBehaviour {


  public GameObject JointPrefab;

  public GameObject Base;

  public Vector3 Direction;
  public bool Main;
  public float id;
  public float life = .5f;
  public float active;


  public float numPoints;

  public float armLength; 
  public float armWidth;
  public float tipSize;

  public float targetArmLength;
  public float targetArmWidth;
  public float targetTipSize;

  public bool created = false;

  public float baseSize;


  //public ComputeBuffer armBuffer;

  /*Struct Arm{

    float id;
    float life;
    float active;
    float attached;
    float jiggleVal;
    float touched;
    float width;
    Vector3[8] points;
    Matrix4x4 tip;

  }*/


  public GameObject[] Points;

	// Use this for initialization
	void Start () {
    	
	}
	
	// Update is called once per frame
	void Update () {

    if( created == true ){
      UpdateSizes();
    }


	}

  void getBaseSize(){
    if( Main == false ){
      baseSize = Base.GetComponent<Arm>().tipSize;
    }else{
      baseSize = 0;
    }
  }

  void UpdateSizes(){

    SpringJoint sj = GetComponent<SpringJoint>();
    //sj.connectedBody = Points[(int)numPoints].GetComponent<Rigidbody>();
    sj.anchor= new Vector3( 0 , -.5f, 0);
    sj.connectedAnchor = new Vector3( 0 , .5f, 0);

    armLength = targetArmLength * life;
    armWidth = targetArmWidth * life;
    tipSize = targetTipSize * life;

    getBaseSize();


    
    float length = armLength;
    length -= baseSize;
    length -= tipSize;  

    for( int i  = 0; i <numPoints; i++ ){
      GameObject capsule = Points[i+1];
      capsule.transform.localScale = new Vector3( armWidth , length / numPoints , armWidth );
    
      sj = capsule.GetComponent<SpringJoint>();

      if( i == 0){
        sj.anchor = new Vector3(0,-.5f,0);
        if( Main == true ){
          sj.connectedAnchor = new Vector3( 0,0,0 );
        }else{
          sj.connectedAnchor = Direction * .5f;
        }
        sj.connectedBody = Base.GetComponent<Rigidbody>();
      }else{
        sj.connectedBody = Points[i].GetComponent<Rigidbody>();
      }

    }

    transform.localScale = new Vector3( tipSize , tipSize , tipSize );

  }

  public void Create(){
    

    life =.5f;
    
    created = true;

    armLength = targetArmLength * life;
    armWidth = targetArmWidth * life;
    tipSize = targetTipSize * life;



    
    
    //Tip = Instantiate( TipPrefab ,  Base.transform.position + Direction * armLength * life , new Quaternion() ) as GameObject;
    transform.position = Base.transform.position + Direction * armLength * life;
    Points = new GameObject[(int)numPoints+2];
    Points[0] = Base;
    getBaseSize();
    
    Vector3 dif = transform.position - Base.transform.position;
    float length = dif.magnitude;

    
    length = armLength;
    length -= baseSize;
    length -= tipSize;  

    dif.Normalize();

    List<AudioClip> Clips = new List<AudioClip>();

    Clips.Add( Resources.Load("Audio/hydra/ArmStroke1") as AudioClip );
    Clips.Add( Resources.Load("Audio/hydra/ArmStroke2") as AudioClip );
    Clips.Add( Resources.Load("Audio/hydra/ArmStroke3") as AudioClip );

    SpringJoint sj;
    for( int i  = 0; i <numPoints; i++ ){

      GameObject capsule = Instantiate( JointPrefab , new Vector3(0,0,0) , new Quaternion() ) as GameObject;
      
      capsule.transform.localScale = new Vector3( 1 , length / numPoints , 1 );

      capsule.transform.position = Base.transform.position + baseSize * dif + length * dif * ( ((float)i + 0.5f) / (float)numPoints );
      capsule.transform.rotation = Quaternion.FromToRotation(Vector3.up, dif);

      //capsule.GetComponent<Renderer>().enabled = false;

     // PlayRandomTouch pt =  capsule.GetComponent<PlayRandomTouch>();
     // pt.pitch = 1.0f * Mathf.Floor( 3.0f * (float)i / (float)numPoints );
     // pt.time = Random.Range( 0 , 10 );
     // pt.Clips = Clips;
     // pt.volume = 0.6f;
    
      sj = capsule.GetComponent<SpringJoint>();

      if( i == 0){
        sj.anchor = new Vector3(0,-.5f,0);
        if( Main == true ){
          sj.connectedAnchor = new Vector3( 0,0,0 );
        }else{
          sj.connectedAnchor = Direction * .5f;
        }
        sj.connectedBody = Base.GetComponent<Rigidbody>();
      }else{
        sj.connectedBody = Points[i].GetComponent<Rigidbody>();
      }

      Points[i+1] = capsule;

    }

    sj = GetComponent<SpringJoint>();
    sj.connectedBody = Points[(int)numPoints].GetComponent<Rigidbody>();
    sj.anchor= new Vector3( 0 , -.5f, 0);
    sj.connectedAnchor = new Vector3( 0 , .5f, 0);
    Points[9] = transform.gameObject;


    UpdateSizes();



  }
}
