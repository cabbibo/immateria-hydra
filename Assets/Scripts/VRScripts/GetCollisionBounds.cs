using UnityEngine;
using System.Collections;
using Valve.VR;

public class GetCollisionBounds : MonoBehaviour {

	// Use this for initialization
	void Start () {

   /* var error = EVRInitError.None;


    var pChaperone = OpenVR.GetGenericInterface(OpenVR.IVRChaperone_Version, ref error);
    if (pChaperone == System.IntPtr.Zero || error != EVRInitError.None)
    {
     // if (!SteamVR.active)
     //   OpenVR.Shutdown();

      return;
    }

  
    var chaperone = new CVRChaperone(pChaperone);
    var chaperoneSetup = new CVRChaperoneSetup(pChaperone);

    //var  pQuadsBuffer = new HmdQuad_t[];

    HmdQuad_t[] pQuadsBuffer = null;


   // //pQuadsBuffer= new HmdQuad_t[punQuadsCount];
//
   // int punQuadsCount = 10;
   // HmdQuad_t[] pqd = new HmdQuad_t[[punQuadsCount];
    bool hasBounds = chaperoneSetup.GetLiveCollisionBoundsInfo( out pQuadsBuffer );

    if( hasBounds == true ){
      print("YAYA");
      print( pQuadsBuffer );
    }else{
      print( "NOOO");
    }
    //print( chaperone );
    //print( chaperoneSetup );
//    print( rect );
*/


	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
