using UnityEngine;
 
//This game object invokes PlaneComputeShader (when attached via drag'n drop in the editor) using the PlaneBufferShader (also attached in the editor)
//to display a grid of points moving back and forth along the z axis.
public class Dough : MonoBehaviour
{

    public Shader shader;
    public Shader prismShader;
    public ComputeShader computeShader;

    public const int numDisformers = 20;
    public GameObject DisformerPrefab;
    

    private Material material;
    private Material prismMaterial;

    public GameObject prism;
    public GameObject pedestal;
    public GameObject handBufferInfo; 
    
    private ComputeBuffer _vertBuffer;
    private ComputeBuffer _transBuffer;
    private ComputeBuffer _disformBuffer;

    private Transform miniTransform;



    private const int threadX = 8;
    private const int threadY = 8;
    private const int threadZ = 4;

    private const int strideX = 8 ;
    private const int strideY = 8 ;
    private const int strideZ = 8 ;

    public float tubeRadius = .6f;
    public float shellRadius = .8f;

    public int ribbonWidth = 256;

    private int gridX { get { return threadX * strideX; } }
    private int gridY { get { return threadY * strideY; } }
    private int gridZ { get { return threadZ * strideZ; } }

    private int vertexCount { get { return gridX * gridY * gridZ; } }

    private int ribbonLength { get { return (int)Mathf.Floor( (float)vertexCount / ribbonWidth ); } }

    private int _kernel;


    private Vector3 p1;
    private Vector3 p2;

    private Texture2D audioTexture;

    private float[] transValues = new float[32];
    private float[] disformValues = new float[3 * numDisformers];
    private float[] handValues;

    private GameObject[] Disformers; 

    //We initialize the buffers and the material used to draw.
    void Start (){



      //prism = transform.parent.transform.Find("Prism").gameObject;
      //pedestal = GameObject.FindGameObjectWithTag("Pedestal"); 
      //handBufferInfo = GameObject.FindGameObjectWithTag("Buffers"); 


      GameObject o = new GameObject();

      miniTransform = o.transform; //new Transform();
      miniTransform.position = prism.transform.position;
      miniTransform.rotation = prism.transform.rotation;
      miniTransform.localScale = prism.transform.localScale;
      miniTransform.localScale *= .2f;

      miniTransform.parent = prism.transform;


      createDisformers();
      createBuffers();
      createMaterial();

      _kernel = computeShader.FindKernel("CSMain");

      Camera.onPostRender += Render;

      // Hit it w/ a dispatch first time
      Dispatch();


    }

   
 
    //When this GameObject is disabled we must release the buffers or else Unity complains.
    private void OnDisable(){
      Camera.onPostRender -= Render;
      ReleaseBuffer();
    }


    private void FixedUpdate(){

      if( prism.GetComponent<Prism>().activated == true ){
        Dispatch();
      }

    }
    //After all rendering is complete we dispatch the compute shader and then set the material before drawing with DrawProcedural
    //this just draws the "mesh" as a set of points
    private void Render(Camera camera) {


      float aVal = prism.GetComponent<Prism>().activeVal * prism.GetComponent<Prism>().activeVal;
      int numVertsTotal = (int)((float)ribbonWidth * aVal * aVal)  * 3 * 2 * (ribbonLength);
      

      // Only computes physics and does big render
      // if the object is activated!!!
      if( prism.GetComponent<Prism>().activated == true ){     
        
        
        
        material.SetPass(0);

        material.SetBuffer("buf_Points", _vertBuffer);
        material.SetInt("_Mini"   , 1 );


        material.SetInt("_RibbonWidth" , ribbonWidth);
        material.SetInt("_RibbonLength" , ribbonLength);
        material.SetInt("_TotalVerts" ,vertexCount);
        material.SetTexture("_AudioMap", audioTexture);
        material.SetTexture( "_NormalMap" , prism.GetComponent<Prism>().info._NormalMap );
        material.SetTexture("_CubeMap" , prism.GetComponent<Prism>().info._CubeMap ); 

        material.SetMatrix("worldMat", transform.localToWorldMatrix);
        material.SetMatrix("invWorldMat", transform.worldToLocalMatrix);
        prismMaterial.SetMatrix("miniMat", miniTransform.localToWorldMatrix );

        prism.GetComponent<Prism>().uniformInterface.SetRenderUniforms( material );



        Graphics.DrawProcedural(MeshTopology.Triangles, numVertsTotal);



        // Draws prism lines only when activated
        prismMaterial.SetPass(0);

        prismMaterial.SetBuffer("buf_Points", _vertBuffer);

        prismMaterial.SetInt("_RibbonWidth" , ribbonWidth);
        prismMaterial.SetInt("_RibbonLength" , ribbonLength);
        prismMaterial.SetInt("_TotalVerts"   , vertexCount);

        prismMaterial.SetTexture("_AudioMap", audioTexture);


        prismMaterial.SetMatrix("worldMat", transform.localToWorldMatrix);
        prismMaterial.SetMatrix("invWorldMat", transform.worldToLocalMatrix);
        prismMaterial.SetMatrix("miniMat", miniTransform.localToWorldMatrix );

        prismMaterial.SetVector( "_PrismPosition",prism.transform.position);

        prismMaterial.SetFloat( "_Active" , prism.GetComponent<Prism>().activeVal );       
 
        prismMaterial.SetFloat( "_NoiseSize" , prism.GetComponent<Prism>()._NoiseSize );


        //Graphics.DrawProcedural( MeshTopology.Lines, numVertsTotal / 42 );


      
        // Draws minature version all the time
        material.SetPass(0);

        material.SetBuffer("buf_Points", _vertBuffer);

        material.SetInt("_Mini"   , 0);

        material.SetInt("_RibbonWidth" , ribbonWidth);
        material.SetInt("_RibbonLength" , ribbonLength);
        material.SetInt("_TotalVerts"   , vertexCount);
        
        material.SetTexture("_AudioMap", audioTexture);
        material.SetTexture( "_NormalMap" , prism.GetComponent<Prism>().info._NormalMap );
        material.SetTexture("_CubeMap" , prism.GetComponent<Prism>().info._CubeMap ); 

        material.SetMatrix("worldMat", transform.localToWorldMatrix);
        material.SetMatrix("invWorldMat", transform.worldToLocalMatrix);
        material.SetMatrix("miniMat", miniTransform.localToWorldMatrix );


        prism.GetComponent<Prism>().uniformInterface.SetRenderUniforms(  material );

        //Graphics.DrawProcedural( MeshTopology.Triangles, numVertsTotal / 256 );
        
    }





      



    }

    private Vector3 getVertPosition( float uvX , float uvY  ){

     float u = uvY * 2.0f * Mathf.PI;
     float v = uvX * 2.0f * Mathf.PI;

     float largeMovement = Mathf.Sin( uvY * 10.0f ) * .3f;
     float smallMovement = Mathf.Sin( uvY * 100.0f )  * ( uvY * uvY * .03f);
     float tubeRad = tubeRadius; //tubeRadius * Mathf.Pow( uvY - .01f , .3f)  * ( 1.0f + largeMovement + smallMovement ) ;
     float slideRad = shellRadius;//shellRadius / 2.0f + uvY;

     float xV = (slideRad + tubeRad * Mathf.Cos(v)) * Mathf.Cos(u) ;
     float zV = (slideRad + tubeRad * Mathf.Cos(v)) * Mathf.Sin(u) ;
     float yV = (tubeRad) * Mathf.Sin(v) + tubeRad;

     //print( xV );
     return new Vector3( xV , yV , zV );

   }


    // Creates the objects that we will use to disform the body of the tube
    private void createDisformers(){

      Disformers = new GameObject[numDisformers];

      _disformBuffer = new ComputeBuffer( numDisformers ,  3 * sizeof(float));
      float[] disValues = new float[ 3 * numDisformers];
      for( int i = 0; i < numDisformers; i++ ){

        float x = Random.Range( 0.01f , .99f );
        float y = Random.Range( 0.01f , .99f );

        Vector3 fPos = getVertPosition( x , y );
        fPos *= 1.0f;

   
        Vector3 pos = transform.TransformPoint( fPos );
        Disformers[i] = (GameObject) Instantiate( DisformerPrefab, pos , new Quaternion());

        Disformers[i].GetComponent<MeshRenderer>().material.SetFloat("_audioID" , (float)i/numDisformers);
        Disformers[i].GetComponent<MeshRenderer>().material.SetFloat("_NumDisformers" , numDisformers);
       
        Disformers[i].transform.parent = transform;

     
       // Disformers[i].GetComponent<MeshRenderer>().enabled = false;

      }



    }




    private void createBuffers() {

      _vertBuffer = new ComputeBuffer( vertexCount ,  AssignStructs.VertC4StructSize * sizeof(float));
      _transBuffer = new ComputeBuffer( 32 ,  sizeof(float));
      

      float[] inValues = new float[ AssignStructs.VertC4StructSize * vertexCount];
      float[] ogValues = new float[ 3         * vertexCount];

      int index = 0;


      for (int z = 0; z < gridZ; z++) {
        for (int y = 0; y < gridY; y++) {
          for (int x = 0; x < gridX; x++) {

            int id = x + y * gridX + z * gridX * gridY; 
            
            float col = (float)(id % ribbonWidth );
            float row = Mathf.Floor( ((float)id+.01f) / ribbonWidth);


            float uvX = col / ribbonWidth;
            float uvY = row / ribbonLength;

            Vector3 fVec = getVertPosition( uvX , uvY );

            AssignStructs.VertC4 vert = new AssignStructs.VertC4();


            vert.pos = fVec * .9f;
            vert.og  = fVec;
            vert.vel = new Vector3( 0 , 0 , 0 );
            vert.nor = new Vector3( 0 , 1 , 0 );
            vert.uv  = new Vector2( uvX , uvY );
            vert.ribbonID = 0;
            vert.life = -1;
            vert.debug = new Vector3( 0 , 1 , 0 );
            vert.row   = row; 
            vert.col   = col; 

            vert.lID = convertToID( col - 1 , row + 0 );
            vert.rID = convertToID( col + 1 , row + 0 );
            vert.uID = convertToID( col + 0 , row + 1 );
            vert.dID = convertToID( col + 0 , row - 1 );

            AssignStructs.AssignVertC4Struct( inValues , index , out index , vert );

          }
        }
      }

      _vertBuffer.SetData(inValues);

    }

    private float convertToID( float col , float row ){

      float id;

      if( col >= ribbonWidth ){ col -= ribbonWidth; }
      if( col < 0 ){ col += ribbonWidth; }

      if( row >= ribbonLength ){ row -= ribbonLength; }
      if( row < 0 ){ row += ribbonLength; }


      id = row * ribbonWidth + col;

      return id;

    }

 
    //For some reason I made this method to create a material from the attached shader.
    private void createMaterial(){

      material = new Material( shader );
      prismMaterial = new Material( prismShader );

    }
 
    //Remember to release buffers and destroy the material when play has been stopped.
    void ReleaseBuffer(){

      _vertBuffer.Release(); 
      _disformBuffer.Release(); 
      _transBuffer.Release(); 
      DestroyImmediate( material );

    }


    private void Dispatch() {


//      print( prism );
      prism.GetComponent<Prism>().uniformInterface.SetComputeUniforms( computeShader );
      audioTexture = prism.GetComponent<audioSourceTexture>().AudioTexture;

      AssignStructs.AssignTransBuffer( transform , transValues , _transBuffer );
      AssignStructs.AssignDisformerBuffer( Disformers , disformValues , _disformBuffer );

      computeShader.SetInt( "_NumDisformers", numDisformers );
      computeShader.SetInt( "_NumberHands", handBufferInfo.GetComponent<HandBuffer>().numberHands );

      computeShader.SetFloat( "_DeltaTime"    , Time.deltaTime );
      computeShader.SetFloat( "_Time"         , Time.time      );
      computeShader.SetInt( "_RibbonWidth"  , ribbonWidth    );
      computeShader.SetInt( "_RibbonLength"  , ribbonLength    );


      computeShader.SetTexture(_kernel,"_Audio", audioTexture);

      computeShader.SetBuffer( _kernel , "transBuffer"  , _transBuffer    );
      computeShader.SetBuffer( _kernel , "vertBuffer"   , _vertBuffer     );
      computeShader.SetBuffer( _kernel , "disformBuffer", _disformBuffer  );
      computeShader.SetBuffer( _kernel , "handBuffer"   , handBufferInfo.GetComponent<HandBuffer>()._handBuffer );

      computeShader.Dispatch(_kernel, strideX , strideY , strideZ );



    }

}