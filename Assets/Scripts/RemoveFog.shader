Shader "Custom/ObjectFog" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _FogColor("Fog Color", Color) = (0, 0, 0, 1)
        _FogDensity ("Fog Density", Range(0,1)) = 0.1
        _FogOffset ("Fog Offset", Range(0,1)) = 0.0
    }

    SubShader {
        Tags { "Queue" = "Overlay" }
        LOD 100

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma exclude_renderers gles xbox360 ps3

            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float4 pos : POSITION;
                float4 projPos : TEXCOORD0;
            };

            float4 _FogColor;
            float _FogDensity, _FogOffset;

            v2f vert(appdata v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.projPos = ComputeGrabScreenPos(o.pos);
                return o;
            }

            fixed4 frag(v2f i) : COLOR {
                return _FogColor;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
