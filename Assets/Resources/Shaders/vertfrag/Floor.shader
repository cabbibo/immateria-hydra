Shader "Custom/Floor" {

    Properties {


    _Hand1( "Hand Position 1" , Vector ) = ( .1 , .4 , .4 )
    _Hand2( "Hand Position 2" , Vector ) = ( .1 , .4 , .4 )


    }

    SubShader{
//        Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
        Cull Back
        Pass{

            Blend SrcAlpha OneMinusSrcAlpha // Alpha blending
 
            CGPROGRAM
            #pragma target 5.0
 
            #pragma vertex vert
            #pragma fragment frag
 
            #include "UnityCG.cginc"
             #include "Assets/Shaders/vertfrag/Resources/Chunks/uvNormalMap.cginc"
 

            struct Vert {

						    float3 pos;
						    float3 vel;
						    float3 nor;
						    float2 uv;
						    float  ribbonID;
						    float  life; 
						    float3 debug;

						};
            
            struct Pos{
                            float3 pos;
                        };

            StructuredBuffer<Vert> buf_Points;
            StructuredBuffer<Pos> og_Points;

            uniform float3 _Hand1;
            uniform float3 _Hand2;
            uniform int _RibbonWidth;
            uniform float4x4 _World;

            uniform sampler2D _NormalMap;
            uniform sampler2D _Audio;
            uniform samplerCUBE _CubeMap;
 
             //A simple input struct for our pixel shader step containing a position.
             struct varyings {
                float4 pos      : SV_POSITION;
                float3 worldPos : TEXCOORD1;
                float3 nor      : TEXCOORD0;
                float3 eye      : TEXCOORD2;
                float3 debug    : TEXCOORD3;
                float2 uv       : TEXCOORD4;
            };
            uint3 getID( uint id  ){

            	uint base = floor( id / 6 );
            	uint tri  = id % 6;
            	uint row = floor( base / _RibbonWidth );
            	uint col = base % _RibbonWidth;

            	uint rDoID = row * _RibbonWidth;
            	uint rUpID = (row + 1) * _RibbonWidth;
            	uint cDoID = col;
            	uint cUpID = col + 1;

                  uint fID = 0;
                  uint tri1 = 0;
                  uint tri2 = 0;


            	if( tri == 0 ){
            		fID = rDoID + cDoID;
                    tri1 = rUpID + cDoID;
                    tri2 = rUpID + cUpID;
            	}else if( tri == 1 ){
            		fID = rUpID + cDoID;
                    tri1 = rUpID + cUpID;
                    tri2 = rDoID + cDoID;
            	}else if( tri == 2 ){
            		fID = rUpID + cUpID;
                    tri1 = rDoID + cDoID;
                    tri2 = rUpID + cDoID;
            	}else if( tri == 3 ){
            		fID = rDoID + cDoID;
                    tri1 = rUpID + cUpID;
                    tri2 = rDoID + cUpID;
            	}else if( tri == 4 ){
            		fID = rUpID + cUpID;
                    tri1 = rDoID + cUpID;
                    tri2 = rDoID + cDoID;
            	}else if( tri == 5 ){
            		fID = rDoID + cUpID;
                    tri1 = rDoID + cDoID;
                    tri2 = rUpID + cUpID;
            	}else{
            		fID = 0;
            	}

                //if( fID >=32768 ){ fID -= 32768; }
                //if( tri1 >=32768 ){ tri1 -= 32768; }
                //if( tri2 >=32768 ){ tri2 -= 32768; }
                return uint3( fID , tri1 , tri2 );

            }
 
            //Our vertex function simply fetches a point from the buffer corresponding to the vertex index
            //which we transform with the view-projection matrix before passing to the pixel program.
            varyings vert (uint id : SV_VertexID){

                varyings o;

                uint3 fID = getID( id );
                Vert v = buf_Points[fID.x ];
                float3 tri1 = buf_Points[fID.y ].pos;
                float3 tri2 = buf_Points[fID.z ].pos;

                Pos og = og_Points[fID.x];
                float3 ogPos = mul(_World , float4( og.pos , 1.));

                float3 nor = normalize(cross( normalize(v.pos - tri1) , normalize(v.pos - tri2)));
                float3 worldPos = v.pos;
                 o.worldPos = v.pos;

                o.pos = mul (UNITY_MATRIX_VP, float4(o.worldPos,1.0f));

                o.debug = o.worldPos - ogPos;

                o.eye = _WorldSpaceCameraPos - o.worldPos;
                o.uv = v.uv;

                o.nor = nor; 


                return o;
            }
 
            //Pixel function returns a solid color for each point.
            float4 frag (varyings i) : COLOR {

                 float3 fNorm = i.nor; //uvNormalMap( _NormalMap , i.pos ,  i.uv  * float2( 1. , .2), i.nor , 4.1 ,1.0);

                float radius = length( i.uv - float2( .5 , .5 ));

                 if( length( i.uv - float2( .5 , .5 )) > .5 ){ discard;}
                float3 fRefl = normalize(reflect( -i.eye , fNorm ));
                float match = dot( fRefl , fNorm );
                float4 aCol = tex2D(_Audio, float2( length( i.debug) * .1 ,0));
                float3 cubeCol = texCUBE(_CubeMap,fRefl ).rgb;

                float multiplier = smoothstep( .5 , .3 , radius );

              //  multiplier *= abs(sin( i.uv.x * 200. )) + abs(sin( i.uv.y * 200.);


                float3 fCol = lerp( float3(0,0,0 ) , float3( 1. , .1 , .4 ) * cubeCol- min( aCol.xyz * 20. , float3( 1,1,1) ) * .1  , multiplier);
                return float4( fCol , 1.);//.1 * float4(1,0.5f,0.0f,1.0) / ( i.dToPoint * i.dToPoint  * i.dToPoint );
            }
 
            ENDCG
 
        }
    }
 
    Fallback Off
	
}
