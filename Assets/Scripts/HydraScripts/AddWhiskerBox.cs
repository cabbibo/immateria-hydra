using UnityEngine;
using System.Collections;

public class AddWhiskerBox : MonoBehaviour {

    public Texture2D normalMap;
    public Cubemap cubeMap;
	// Use this for initialization
	void Start () {


//    print( Camera.main );

    GameObject cam = GameObject.Find("Camera (eye)");
    WhiskersBox wb;
    
    if( cam != null ){
      wb = cam.AddComponent<WhiskersBox>();
    }else{
//      print("ss");
      //print(Camera.main.gameObject)
      wb = Camera.main.gameObject.AddComponent<WhiskersBox>();
    }

//    print( wb );
    wb.mainObject = gameObject;
    wb.stalkShader = Shader.Find("Custom/Whiskers");
    wb.tipShader = Shader.Find("Custom/Tips");
    wb.baseShader = Shader.Find("Custom/whiskersBase");
    wb.computeShader = (ComputeShader)Resources.Load("WhiskersTransform");
    wb.handL = GameObject.Find("handL");
    wb.handR = GameObject.Find("handR");
    wb.Select3D = GameObject.Find("Select3D");
    
    wb.audioObj = GameObject.Find("Camera (ears)");
    wb.normalMap = normalMap;
    wb.cubeMap = cubeMap;

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
