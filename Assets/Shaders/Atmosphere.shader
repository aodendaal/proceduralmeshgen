Shader "Custom/Atmosphere" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
	}
	SubShader {
		Tags { "RenderType"="Transparent"
			   "Queue"="Transparent"
			 }
		
		Pass {
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
				#pragma vertex vert
			    #pragma fragment frag

				#include "UnityCG.cginc"

				float4 _Color;

				struct v2f {
			        float4 pos : SV_POSITION;
			        float3 normal : TEXCOORD0;
			        float3 worldvertpos : TEXCOORD1;
			    };

				v2f vert (appdata_base input) {
					v2f output;

					output.pos = mul(UNITY_MATRIX_MVP, input.vertex);
					output.normal = mul((float3x3)unity_ObjectToWorld, input.normal);
					output.worldvertpos = mul(unity_ObjectToWorld, input.vertex);

					return output;
				}

				float4 frag (v2f input) : COLOR {
					
					float4 col;

					col.rgb = saturate(dot(input.normal, _WorldSpaceLightPos0));
					col.a = 0.1;//dot(input.normal, _WorldSpaceCameraPos);
					
					return col; 
				}
				
			ENDCG
		}
	}
	FallBack "Diffuse"
}
