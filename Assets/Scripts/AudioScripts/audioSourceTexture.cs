﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class audioSourceTexture : MonoBehaviour
{


private AudioSource audioSource;

private int width; // texture width 
private int height; // texture height 
public Color backgroundColor = Color.black; 
public Color waveformColor = Color.green; 
public int size = 1024; // size of sound segment displayed in texture

private Color[] blank; // blank image array 
public Texture2D AudioTexture; 
private float[] samples; // audio samples array
public float[] lowRes;
public int lowResSize;// = 256;

void Awake ()
{ 
    width = size;
    height = 1;

    // create the samples array 
    samples = new float[size*8]; 
    lowRes  = new float[64];
    lowResSize = 64;
    print( lowRes );

    audioSource = GetComponent<AudioSource>();

    // create the AudioTexture and assign to the guiTexture: 
    AudioTexture = new Texture2D (width, height);


    // create a 'blank screen' image 
    blank = new Color[width * height]; 

    for (int i = 0; i < blank.Length; i++) { 
         blank [i] = backgroundColor; 
    } 

 // refresh the display each 100mS 

}

void Update(){
GetCurWave();
}

    void GetCurWave (){ 
        // clear the AudioTexture 
        AudioTexture.SetPixels (blank, 0); 

        // get samples from channel 0 (left) 
        //GetComponent<AudioSource>().GetOutputData (samples, 0); 

        audioSource.GetSpectrumData(samples, 0, FFTWindow.Triangle);
        audioSource.GetSpectrumData(lowRes, 0, FFTWindow.Triangle);
        //print( lowRes[ 0] );

        Color c;
        float r , g, b, a;
        // draw the waveform 
        for (int i = 0; i < size; i++) { 

          Color og = AudioTexture.GetPixel((int)(width * i / size), (int)(1 * (samples [i])) - 1 );

          r = og.r * .8f + samples[ (int)(i * 4)  + 0 ] * 128;
          g = og.g * .8f + samples[ (int)(i * 4)  + 1 ] * 128;
          b = og.b * .8f + samples[ (int)(i * 4)  + 2 ] * 128;
          a = og.a * .8f + samples[ (int)(i * 4)  + 3 ] * 128;

//
          c = new Color( r , g, b, a);

          AudioTexture.SetPixel((int)(width * i / size), (int)(1 * (samples [i])) - 1, c );
        } // upload to the graphics card 


        AudioTexture.Apply (); 
    } 
}