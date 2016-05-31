using UnityEngine;
using System.Collections;

public class AddRays : MonoBehaviour {

  private Texture2D audioTexture;
  public Texture2D normalMap;
  public Cubemap cubeMap;

  public GameObject audioObj;

	// Use this for initialization
	void Start () {
   
    audioTexture = audioObj.GetComponent<audioListenerTexture>().AudioTexture;
    for( int i = 0; i < 40; i++ ){
      GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
      go.transform.localScale = new Vector3( 0.5f , 2 , 0.5f );

      go.GetComponent<Renderer>().material.color = new Color(0,0,0,1);
      
      Shader s = Shader.Find("Custom/Pillars");
      Material m = new Material( s );

      m.SetTexture("_AudioMap", audioTexture);
      m.SetTexture( "_NormalMap" , normalMap );
      m.SetTexture("_CubeMap" , cubeMap ); 

  
      go.GetComponent<Renderer>().material = m;
      //go.transform.position = transform.position;
      //Vector3 lookVec = new Vector3( 0 , -1 , 0 );
      //lookVec.x += Random.Range( -.1f, .1f);
      //lookVec.z += Random.Range( -.1f, .1f);
      //lookVec.Normalize();
      //go.transform.rotation = Quaternion.LookRotation( lookVec);
      float angle = (float)i/40 * 2 * Mathf.PI;
      go.transform.position = new Vector3( Mathf.Sin(angle ) * 6 , 0 , Mathf.Cos( angle ) * 6 );

    }
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
