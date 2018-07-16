// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Marker" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_Transparency ("Transparency", Range(0, 1)) = 1
	}
	SubShader {
		Tags { "RenderType" = "Transparent"
			   "Queue" = "Transparent"
			 }
		LOD 200
		
		Pass {
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			// Physically based Standard lighting model, and enable shadows on all light types
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			
			fixed4 _Color;
			float _Transparency;
			
			struct v2f {
				float4 pos : SV_POSITION;
			};
			
			v2f vert(appdata_base input) {
				v2f output;
			
				output.pos = UnityObjectToClipPos(input.vertex);		
			
				return output;
			}
			
			float4 frag (v2f input) : COLOR {
				fixed4 output = _Color;
			
				output.a = _Transparency;
			
				return output;
			} 
			
			ENDCG
		}
	}
	FallBack "Diffuse"
}
