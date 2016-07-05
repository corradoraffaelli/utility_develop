Shader "Custom/NewSurfaceShader" {
	Properties {
//		_Color ("Color", Color) = (1,1,1,1)
//		_MainTex ("Albedo (RGB)", 2D) = "white" {}
//		_Glossiness ("Smoothness", Range(0,1)) = 0.5
//		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Lighting Off
		Pass
		{
			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			
			struct a2v
			{
				float4 vertex : POSITION;
				float4 color: COLOR;
			};
			
			struct v2f
			{
				float4 pos : SV_POSITION;
				float4 color : COLOR0;
			};
			
			v2f vert (a2v input)
			{
				v2f o;
				o.pos = mul( UNITY_MATRIX_MVP, input.vertex);
				o.color = input.color;
//				o.a = input.a;
			
				return o;
			}
			
			half4 frag( v2f i ) : COLOR
			{
//				i.a = a;
				return i.color;
			}
			
			ENDCG
		}
//		Tags { "RenderType"="Opaque" }
//		LOD 200
//		
//		CGPROGRAM
//		// Physically based Standard lighting model, and enable shadows on all light types
//		#pragma surface surf Standard fullforwardshadows
//
//		// Use shader model 3.0 target, to get nicer looking lighting
//		#pragma target 3.0
//
//		sampler2D _MainTex;
//
//		struct Input {
//			float2 uv_MainTex;
//		};
//
//		half _Glossiness;
//		half _Metallic;
//		fixed4 _Color;
//
//		void surf (Input IN, inout SurfaceOutputStandard o) {
//			// Albedo comes from a texture tinted by color
//			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
//			o.Albedo = c.rgb;
//			// Metallic and smoothness come from slider variables
//			o.Metallic = _Metallic;
//			o.Smoothness = _Glossiness;
//			o.Alpha = c.a;
//		}
//		ENDCG
	} 
//	FallBack "Diffuse"
}
