Shader "Custom/FloorTileShader" {
	Properties {
		_MainTex ("Emmisive (RGB)", 2D) = "white" {}
		_EmissionColor ("EmissionColor", Color) = (1,1,1,1)
		_EmissionIntensity ("Emission Intensity", float) = 0
		_TexTiling ("Texture Tiling", float) = 20
		_index ("Start Index", float) = 0

		_GridTex ("Grid Texture (bit mask)", 2D) = "" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0


		sampler2D _MainTex;
		sampler2D _GridTex;
		fixed4 _EmissionColor;
		float _EmissionIntensity;
		float _TexTiling;
		int _index;
		 
		struct Input {
			float2 uv_MainTex;
			float2 uv_GridTex;
		};
		//basic emission + intensity
//		void surf (Input IN, inout SurfaceOutputStandard o) {
//			// Albedo comes from a texture tinted by color
//			fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
//
//			//Output
//   			o.Emission = c.rgb * _EmissionColor * _EmissionIntensity;
//
//			o.Alpha = c.a;
//		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
			fixed4 c2 = tex2D (_GridTex, IN.uv_GridTex);

			o.Emission = c.rgb * _EmissionIntensity;
			o.Emission *= c2.rgb;
		}

		ENDCG
	}
	FallBack "Standard"
}
