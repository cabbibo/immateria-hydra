using UnityEngine;
using System.Collections;

public class DonutInfo : MonoBehaviour {

  public float _LengthOfConnectionSprings;
  public float _ConnectionSpringStrength;
  public float _MaxVel;
  public float _MaxForce;
  public float _ForceMultiplier;
  
  public float _Dampening;
  public float _HandRepelRadius;
  public float _HandRepelStrength;
  public float _ReturnSpringStrength;
  public float _DistanceToAudioHit;
  public float _AudioHitMultiplier;

  public float _NormalizedVelocityValue;
  public float _NormalizedVertexValue;
  public float _NormalizedOriginalValue;
  public float _AudioValue;
  public float _AudioSampleSize;
  public float _ReflectionValue;
  public float _NormalMapSize;
  public float _NormalMapDepth;
  
  public Cubemap     _CubeMap;
  public Texture2D _NormalMap;

  public AudioClip clip;
  public string songName;


  // Use this for initialization
  void Awake () {
   // clip = Resources.Load(songName) as AudioClip; 
  }
  
  // Update is called once per frame
  void Update () {
  
  }
}
