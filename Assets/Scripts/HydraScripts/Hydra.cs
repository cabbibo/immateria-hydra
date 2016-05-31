using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hydra : MonoBehaviour {



  public GameObject Arm;


  public Vector3 startHeight = new Vector3( 0 , 1.25f , 0 );
    
  public float baseTipSize = 0.5f;
  public float centerScale = 0.3f;
  public float tipSize = 0.15f;

  public int   armPoints = 8;

  public float armLength = 0.6f;
  public float armWidth  = 0.05f;

  public float stalkLength = 1.25f;
  public float stalkWidth  = 0.1f;


  private GameObject MainArm;
  private GameObject[] Arms;
	// Use this for initialization

  private Vector3[] directions;


  private PlayTouch pt;


	void Start() {

    Arms = new GameObject[6];

    directions = new Vector3[] {
      new Vector3(  1 , 0 ,  0 ),
      new Vector3( -1 , 0 ,  0 ),
      new Vector3(  0 , 0 ,  1 ),
      new Vector3(  0 , 0 , -1 ),
      new Vector3(  0 , 1 ,  0 )
    };

    MainArm = Instantiate( Arm , new Vector3(0,0,0) , Quaternion.identity ) as GameObject;

    MainArm.GetComponent<Arm>().Base = transform.gameObject;
    MainArm.GetComponent<Arm>().Direction = new Vector3( 0 , 1 , 0 );
    MainArm.GetComponent<Arm>().Main = true;

    MainArm.GetComponent<Arm>().numPoints = 8;
    MainArm.GetComponent<Arm>().id = 0;

    MainArm.GetComponent<Arm>().targetArmLength = stalkLength;
    MainArm.GetComponent<Arm>().targetArmWidth = stalkWidth;
    MainArm.GetComponent<Arm>().targetTipSize = baseTipSize;



    //MainArm.GetComponent<PlayArmTouch>().tipClip = GetComponent<Clips>().baseHit;
    //MainArm.GetComponent<PlayArmTouch>().armClips = GetComponent<Clips>().armHits[i];
    //MainArm.GetComponent<LookForFood>().enabled = true; // Main Arm doesn't look for food!

    //MainArm.GetComponent<Life>()

    //MainArm.GetComponent<Arm>().tubeCubeMap = tubeCubeMap;
    //MainArm.GetComponent<Arm>().tubeNormalMap = tubeNormalMap;

    // Call to start this arm!
    //MainArm.GetComponent<Arm>().Enter();
    
    MainArm.GetComponent<Arm>().Create();

    Arms[0] = MainArm;

    for( int i = 0; i < directions.Length; i++ ){
          
      GameObject arm = Instantiate( Arm , new Vector3(0,0,0) , new Quaternion() ) as GameObject;

      arm.GetComponent<Arm>().Base = MainArm;
      arm.GetComponent<Arm>().Direction = directions[i];
      arm.GetComponent<Arm>().Main = false;

      arm.GetComponent<Arm>().id = 1 + (float)i;

      arm.GetComponent<Arm>().numPoints = 8;

      arm.GetComponent<Arm>().targetArmLength = armLength;
      arm.GetComponent<Arm>().targetArmWidth = armWidth;
      arm.GetComponent<Arm>().targetTipSize = tipSize;

      //arm.GetComponent<PlayArmTouch>().clip = GetComponent<Clips>().tipHits[i];
      //arm.GetComponent<PlayArmTouch>().clips = GetComponent<Clips>().armHits[i];
      //arm.GetComponent<LookForFood>().enabled = false; // Main Arm doesn't look for food!

      //arm.GetComponent<Arm>().tubeCubeMap = tubeCubeMap;
      //arm.GetComponent<Arm>().tubeNormalMap = tubeNormalMap;
      arm.GetComponent<Arm>().Create();

      Arms[i+1] = arm;

    }
	
	}

	// Update is called once per frame
	void Update () {

    Rigidbody rb = MainArm.GetComponent<Rigidbody>();
    rb.AddForce( new Vector3( 0 , .1f , 0) );


	}
}
