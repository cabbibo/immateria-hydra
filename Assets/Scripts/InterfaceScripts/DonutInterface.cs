using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class DonutInterface: MonoBehaviour {


  // TODO:
  // Figure out articulate ways to define 'META INTERFACE'
  // Which bundles certain values of interface together to make
  // much more power with single slider

  // be able to define shape of curve as part of uniform
  // be able to set to osscilate in certain ways?


  // TODO: Be able to create 'presets'
  // Be able to save presets from inside VR
  // Be able to reload these presets, and add them to a a 'presets interface'
  // Be able to select audio

  // To Think about: Be able to choose a 'render mode ', and have interface for that mode dynamically update

  public struct FloatUniform{
    public float value;
    public float low;
    public float high;
    public float og;
  }

  public struct TextureUniform{
    public Texture2D value;
    public Texture2D og;
  }

  public struct CubeUniform{
    public Cubemap value;
    public Cubemap og;
  }

  public Texture2D defaultNormalMap;
  public Cubemap defaultCubeMap;
  //public Dictionary Sliders<string,GameObject>;

  public Hashtable ComputeSliders;
  public Hashtable RenderSliders;

  public GameObject Slider1;
  public GameObject Slider2;
  public GameObject Slider3;
  public GameObject Slider4;


  public class ComputeUniforms{

    public FloatUniform _LengthOfConnectionSprings;
    public FloatUniform _ConnectionSpringStrength;
    public FloatUniform _MaxVel;
    public FloatUniform _MaxForce;
    public FloatUniform _ForceMultiplier;
    
    public FloatUniform _Dampening;
    public FloatUniform _HandRepelRadius;
    public FloatUniform _HandRepelStrength;

    public FloatUniform _ReturnSpringStrength;

    public FloatUniform _DistanceToAudioHit;
    public FloatUniform _AudioHitMultiplier;

  };


  public class RenderUniforms{

    public FloatUniform _NormalizedVelocityValue;
    public FloatUniform _NormalizedVertexValue;
    public FloatUniform _NormalizedOriginalValue;

    public FloatUniform _AudioValue;
    public FloatUniform _AudioSampleSize;
    public FloatUniform _ReflectionValue;
    public FloatUniform _NormalMapSize;
    public FloatUniform _NormalMapDepth;

    
    public CubeUniform     _CubeMap;
    public TextureUniform _NormalMap;


  };


  private ComputeUniforms computeUniforms;
  private RenderUniforms renderUniforms;




  void Awake(){

    computeUniforms = new ComputeUniforms();

    computeUniforms._LengthOfConnectionSprings.value  = 1.1f ;
    computeUniforms._LengthOfConnectionSprings.og     = 1.1f ;
    computeUniforms._LengthOfConnectionSprings.low    = 0f   ;
    computeUniforms._LengthOfConnectionSprings.high   = 100.5f  ;

    computeUniforms._ConnectionSpringStrength.value   = 10.9f  ;
    computeUniforms._ConnectionSpringStrength.og      = 10.9f  ;
    computeUniforms._ConnectionSpringStrength.low     = 1.9f    ;
    computeUniforms._ConnectionSpringStrength.high    = 20.9f ;

    computeUniforms._MaxVel.value                     = 30.5f  ;
    computeUniforms._MaxVel.value                     = 30.5f  ;
    computeUniforms._MaxVel.low                       =   .1f  ;
    computeUniforms._MaxVel.high                      = 100.5f ;

    computeUniforms._MaxForce.value                   = 30.2f  ;
    computeUniforms._MaxForce.value                   = 30.2f  ;
    computeUniforms._MaxForce.low                     =   .2f  ;
    computeUniforms._MaxForce.high                    = 60.2f  ;


    computeUniforms._ForceMultiplier.value            = .01f   ;
    computeUniforms._ForceMultiplier.og               = .01f   ;
    computeUniforms._ForceMultiplier.low              = .0001f  ;
    computeUniforms._ForceMultiplier.high             = .03f    ;


    computeUniforms._Dampening.value                  = .98f   ;
    computeUniforms._Dampening.og                     = .98f   ;
    computeUniforms._Dampening.low                    = .4f    ;
    computeUniforms._Dampening.high                   = .99f   ;

    computeUniforms._HandRepelRadius.value            = 1.0f   ;
    computeUniforms._HandRepelRadius.og               = 1.0f   ;
    computeUniforms._HandRepelRadius.low              = .01f   ;
    computeUniforms._HandRepelRadius.high             = 10.0f   ;


    computeUniforms._HandRepelStrength.value          = .5f   ;
    computeUniforms._HandRepelStrength.og             = .5f   ;
    computeUniforms._HandRepelStrength.low            = .001f  ;
    computeUniforms._HandRepelStrength.high           = 1.0f   ;


    computeUniforms._ReturnSpringStrength.value       = 4.1f   ;
    computeUniforms._ReturnSpringStrength.og          = 4.1f   ;
    computeUniforms._ReturnSpringStrength.low         = .01f   ;
    computeUniforms._ReturnSpringStrength.high        = 10.1f  ;


    computeUniforms._DistanceToAudioHit.value         = 5.8f   ;
    computeUniforms._DistanceToAudioHit.og            = 5.8f   ;
    computeUniforms._DistanceToAudioHit.low           = .3f    ;
    computeUniforms._DistanceToAudioHit.high          = 50.8f  ;

    computeUniforms._AudioHitMultiplier.value         = .01f   ;
    computeUniforms._AudioHitMultiplier.og            = .01f   ;
    computeUniforms._AudioHitMultiplier.low           = .001f  ;
    computeUniforms._AudioHitMultiplier.high          = .06f   ;


    renderUniforms = new RenderUniforms();

    renderUniforms._NormalizedOriginalValue.value  = 1f ;
    renderUniforms._NormalizedOriginalValue.og     = 1f ;
    renderUniforms._NormalizedOriginalValue.low    = 0f ;
    renderUniforms._NormalizedOriginalValue.high   = 1f ;

    renderUniforms._NormalizedVertexValue.value  = 1f ;
    renderUniforms._NormalizedVertexValue.og     = 1f ;
    renderUniforms._NormalizedVertexValue.low    = 0f ;
    renderUniforms._NormalizedVertexValue.high   = 1f ;

    renderUniforms._NormalizedVelocityValue.value  = 1f ;
    renderUniforms._NormalizedVelocityValue.og     = 1f ;
    renderUniforms._NormalizedVelocityValue.low    = 0f ;
    renderUniforms._NormalizedVelocityValue.high   = 1f ;

    renderUniforms._AudioValue.value   = 0f ;
    renderUniforms._AudioValue.og      = 0f ;
    renderUniforms._AudioValue.low     = 0f ;
    renderUniforms._AudioValue.high    = 1f ;

    renderUniforms._AudioSampleSize.value   = 0.1f ;
    renderUniforms._AudioSampleSize.og      = 0.1f ;
    renderUniforms._AudioSampleSize.low     = 0.1f ;
    renderUniforms._AudioSampleSize.high    = 1f ;

    renderUniforms._ReflectionValue.value   = 0f  ;
    renderUniforms._ReflectionValue.value   = 0f  ;
    renderUniforms._ReflectionValue.low     = 0f  ;
    renderUniforms._ReflectionValue.high    = 1f ;

    renderUniforms._NormalMapSize.value   = 2f ;
    renderUniforms._NormalMapSize.og      = 2f ;
    renderUniforms._NormalMapSize.low     = 0f ;
    renderUniforms._NormalMapSize.high    = 10f ;

    renderUniforms._NormalMapDepth.value   = 0f  ;
    renderUniforms._NormalMapDepth.value   = 0f  ;
    renderUniforms._NormalMapDepth.low     = 0f  ;
    renderUniforms._NormalMapDepth.high    = 20f ;

    renderUniforms._NormalMap.value = defaultNormalMap;
    renderUniforms._NormalMap.og = defaultNormalMap;

    renderUniforms._CubeMap.value = defaultCubeMap;
    renderUniforms._CubeMap.og = defaultCubeMap;




  }

  public void SetComputeUniforms( ComputeShader Mat ){

    //print(computeUniforms._Dampening.value);

   // Mat.SetFloat( "_LengthOfConnectionSprings"    , u._LengthOfConnectionSprings);

    foreach (var field in typeof(ComputeUniforms).GetFields()){

      FloatUniform u = (FloatUniform)field.GetValue( computeUniforms );

      Mat.SetFloat( field.Name , u.value );

    }
   
    //Mat.SetFloat( "_MaxVel"                       , computeUniforms._MaxVel.value                    );
    //Mat.SetFloat( "_MaxForce"                     , computeUniforms._MaxForce.value                  );
    //Mat.SetFloat( "_ForceMultiplier"              , computeUniforms._ForceMultiplier.value           );
    //Mat.SetFloat( "_Dampening"                    , computeUniforms._Dampening.value                 );
    //Mat.SetFloat( "_HandRepelRadius"              , computeUniforms._HandRepelRadius.value           );
    //Mat.SetFloat( "_HandRepelStrength"            , computeUniforms._HandRepelStrength.value         );
    //Mat.SetFloat( "_ReturnSpringStrength"         , computeUniforms._ReturnSpringStrength.value      );
    //Mat.SetFloat( "_DistanceToAudioHit"           , computeUniforms._DistanceToAudioHit.value        );
    //Mat.SetFloat( "_AudioHitMultiplier"           , computeUniforms._AudioHitMultiplier.value        );

  }

  public void SetRenderUniforms( Material Mat ){

    foreach (var field in typeof(RenderUniforms).GetFields()){

      if( field.Name == "_NormalMap"){

        TextureUniform u = (TextureUniform)field.GetValue( renderUniforms );
        Mat.SetTexture(field.Name ,  u.value ); 

      }else if( field.Name == "_CubeMap"){

        CubeUniform u = (CubeUniform)field.GetValue( renderUniforms );
        Mat.SetTexture(field.Name ,  u.value ); 

      }else{

        FloatUniform u = (FloatUniform)field.GetValue( renderUniforms );
        Mat.SetFloat( field.Name , u.value );

      }
    }
   
  }

  public void SetRenderUniformsUsingDonutInfo( DonutInfo di , Material Mat ){

    foreach (var field in typeof(RenderUniforms).GetFields()){

      if( field.Name == "_NormalMap"){

        //object  o = GetPropValue( di , field.Name );
       // print( u );
        //Texture2D u = (Texture2D)di.GetType().GetProperty(field.Name).GetValue(di, null);//o.value; //GetPropValue( di , field.Name );//di.GetValue<Texture2D>(field.Name); // (Texture2D)field.GetValue( di );
       // Mat.SetTexture(field.Name ,  u); 

        System.Type T = di.GetType();

        System.Reflection.FieldInfo FI = T.GetField(field.Name );

        Texture2D tmp = (Texture2D)FI.GetValue(di);

        Mat.SetTexture(field.Name , tmp ); 

      }else if( field.Name == "_CubeMap"){

       //Cubemap u = (Cubemap)di.GetType().GetProperty(field.Name).GetValue(di, null);//GetPropValue( di , field.Name );//di.GetValue<Cubemap>(field.Name); //(Cubemap)field.GetValue( di );
       //Mat.SetTexture(field.Name , u ); 
        System.Type T = di.GetType();

        System.Reflection.FieldInfo FI = T.GetField(field.Name );

        Cubemap tmp = (Cubemap)FI.GetValue(di);

        Mat.SetTexture(field.Name , tmp ); 

      }else{

        System.Type T = di.GetType();

        System.Reflection.FieldInfo FI = T.GetField(field.Name );

        float tmp = (float)FI.GetValue(di);

        //float u = (float)di.GetType().GetProperty(field.Name).GetValue(di, null); //GetPropValue( di , field.Name ); //di.GetValue<float>(field.Name);// (float)field.GetValue( di );
        Mat.SetFloat( field.Name , tmp );

      }
    }


  }

  public static object GetPropValue(object src, string propName)
   {
       return src.GetType().GetProperty(propName).GetValue(src, null);
   }

  public void resetRenderUniforms(){

    foreach (var field in typeof(RenderUniforms).GetFields()){

      FloatUniform u = (FloatUniform)field.GetValue( renderUniforms );
      u.value = u.og;

      field.SetValue( renderUniforms , u );

    }

  }

  public void resetComputeUniforms(){


    foreach (var field in typeof(ComputeUniforms).GetFields()){

      FloatUniform u = (FloatUniform)field.GetValue( computeUniforms );
      u.value = u.og;

      field.SetValue( renderUniforms , u );

    }
   

  }





  public void SetDonutInfo( GameObject donut ){


    SetDonutUniforms( donut );



  }

  void SetDonutUniforms( GameObject donut ){
    
    DonutInfo d = donut.GetComponent<DonutInfo>();

    FieldInfo[] fields = d.GetType().GetFields();

    foreach (var field in typeof(ComputeUniforms).GetFields()){

      FloatUniform u = (FloatUniform)field.GetValue( computeUniforms );

      for( int i = 0; i < fields.Length; i++ ){
        if( fields[i].Name == field.Name ){
          u.value = (float)fields[i].GetValue( d );
        }
      }

      field.SetValue( computeUniforms , u );

    }







    foreach (var field in typeof(RenderUniforms).GetFields()){


      if( field.Name == "_NormalMap"){

        TextureUniform u = (TextureUniform)field.GetValue( renderUniforms );

        for( int i = 0; i < fields.Length; i++ ){
          if( fields[i].Name == field.Name ){
            u.value = (Texture2D)fields[i].GetValue( d );
          }
        }

        field.SetValue( renderUniforms , u );


      }else if( field.Name == "_CubeMap"){

        CubeUniform u = (CubeUniform)field.GetValue( renderUniforms );

        for( int i = 0; i < fields.Length; i++ ){
          if( fields[i].Name == field.Name ){
            u.value = (Cubemap)fields[i].GetValue( d );
          }
        }

        field.SetValue( renderUniforms , u );
        

      }else{

        FloatUniform u = (FloatUniform)field.GetValue( renderUniforms );

        for( int i = 0; i < fields.Length; i++ ){
          if( fields[i].Name == field.Name ){
            u.value = (float)fields[i].GetValue( d );
          }
        }

        field.SetValue( renderUniforms , u );

      }

    }


  }


public static T GetReference<T>(object inObj, string fieldName) where T : class
{
    return GetField(inObj, fieldName) as T;
}


public static T GetValue<T>(object inObj, string fieldName) where T : struct
{
    return (T)GetField(inObj, fieldName);
}


public static void SetField(object inObj, string fieldName, object newValue)
{
    FieldInfo info = inObj.GetType().GetField(fieldName);
    if (info != null)
        info.SetValue(inObj, newValue);
}


private static object GetField(object inObj, string fieldName)
{
    object ret = null;
    FieldInfo info = inObj.GetType().GetField(fieldName);
    if (info != null)
        ret = info.GetValue(inObj);
    return ret;
}


	// Update is called once per frame
	void Update () {
	
	}


}
