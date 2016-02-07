Shader "Custom/FloorTileShader" {
	Properties {
		_MainTex ("Emmisive (RGB)", 2D) = "white" {}
		_EmissionIntensity ("Emission Intensity", float) = 0

		_GridTex ("Grid Texture (bit mask)", 2D) = "" {}

//		_BumpMap ("Bump Map (bit mask)", 2D) = "bump" {}
	}
	SubShader {
        Tags { "RenderType" = "Opaque" "Queue" = "Background"}

        LOD 200
        CGPROGRAM
          #pragma surface surf SimpleLambert
  
          half4 LightingSimpleLambert (SurfaceOutput s, half3 lightDir, half atten) {
              half NdotL = dot (s.Normal, lightDir);
              half4 c;
              c.rgb = _LightColor0.rgb * (NdotL * atten);
              c.a = s.Alpha;
              return c;
          }
  
        struct Input {
            float2 uv_MainTex;
            float2 uv_GridTex;
//            float2 uv_BumpMap;
        };
        
        sampler2D _MainTex;
        sampler2D _GridTex;
//        sampler2D _BumpMap;
        float _EmissionIntensity;
        
        void surf (Input IN, inout SurfaceOutput o) {
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
			fixed4 c2 = tex2D (_GridTex, IN.uv_GridTex);
			o.Emission = c.rgb * c2.rgb * _EmissionIntensity;
			//o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_BumpMap));
        }
        ENDCG
   

//        //Pass 2
//		Blend DstColor Zero
//
//		CGPROGRAM
//		// Physically based Standard lighting model, and enable shadows on all light types
//		#pragma surface surf Standard
//		
//		// Use shader model 3.0 target, to get nicer looking lighting
//		#pragma target 3.0
//
//		struct Input {
//			float2 uv_MainTex;
//			float2 uv_GridTex;
//		};
//		        
//        sampler2D _MainTex;
//		sampler2D _GridTex;
//		float _EmissionIntensity;
//
//		void surf (Input IN, inout SurfaceOutputStandard o) {
//			fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
//			fixed4 c2 = tex2D (_GridTex, IN.uv_GridTex);
//			o.Emission = c.rgb * c2.rgb * _EmissionIntensity;
//		}
//
//		ENDCG
        }
	FallBack "Diffuse"
}
