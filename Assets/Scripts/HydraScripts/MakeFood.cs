using UnityEngine;
using System.Collections;


public class MakeFood : MonoBehaviour {

  private GameObject go;
  private AudioSource audio;

  public Texture2D normalMap;
  public Cubemap cubeMap;

  void OnEnable(){
    EventManager.OnTriggerDown += OnTriggerDown;

    go = GameObject.CreatePrimitive(PrimitiveType.Cube);
    go.transform.localScale = go.transform.localScale * 0.05f;
    go.tag = "Food";
    go.transform.position = new Vector3( .4f , 1.5f , .4f );
    go.AddComponent<Rigidbody>();

    go.AddComponent<Food>();
    Shader s = Shader.Find("Custom/RaytraceFood");
    Material m = new Material( s );
    m.SetTexture( "_NormalMap" , normalMap );
    m.SetTexture("_CubeMap" , cubeMap );   

    go.GetComponent<Renderer>().material = m;


    go = GameObject.CreatePrimitive(PrimitiveType.Cube);
    go.transform.localScale = go.transform.localScale * 0.05f;
    go.tag = "Food";
    go.transform.position = new Vector3( -.2f , 2.4f , -.4f );
    go.AddComponent<Rigidbody>();

    go.AddComponent<Food>();
    // s = Shader.Find("Custom/RaytraceFood");
    m = new Material( s );
    m.SetTexture( "_NormalMap" , normalMap );
    m.SetTexture("_CubeMap" , cubeMap );   

    go.GetComponent<Renderer>().material = m;


        go = GameObject.CreatePrimitive(PrimitiveType.Cube);
    go.transform.localScale = go.transform.localScale * 0.05f;
    go.tag = "Food";
    go.transform.position = new Vector3( -.9f , 1.4f , -.4f );
    go.AddComponent<Rigidbody>();

    go.AddComponent<Food>();
    // s = Shader.Find("Custom/RaytraceFood");
    m = new Material( s );
    m.SetTexture( "_NormalMap" , normalMap );
    m.SetTexture("_CubeMap" , cubeMap );   

    go.GetComponent<Renderer>().material = m;



  }

  void OnTriggerDown(GameObject o){

    go = GameObject.CreatePrimitive(PrimitiveType.Cube);
    go.transform.localScale = go.transform.localScale * 0.05f;
    go.tag = "Food";
    go.transform.position = o.transform.position;
    go.transform.rotation = o.transform.rotation;
    go.transform.position = go.transform.position + ( go.transform.rotation * (Vector3.forward * .1f) );
    go.AddComponent<Rigidbody>();

    go.AddComponent<Food>();
    Shader s = Shader.Find("Custom/RaytraceFood");
    Material m = new Material( s );
    m.SetTexture( "_NormalMap" , normalMap );
    m.SetTexture("_CubeMap" , cubeMap );   

    go.GetComponent<Renderer>().material = m;

  }

	// Update is called once per frame
	void Update () {
	
	}
}
