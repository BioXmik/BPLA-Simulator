Shader "Wire/Animated Shader" 
{
	Properties 
	{
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0

        [Normal] _BumpMap("Normal Map", 2D) = "bump" {}

        _Speed ("Speed Y", Float) = 1.0

        //_Wavelength ("Wavelength", Float) = 1.0
        _Frequency ("Frequency Y", Float) = 1.0 // _Frequency = 1/_Wavelength
        _Amplitude ("Amplitude Y", Float) = 1.0

        _Speed2 ("Speed X,Z", Float) = 1.0
        _Amplitude2 ("Amplitude X,Z", Float) = 1.0
        _WindDirection ("Wind Direction", Vector) = (0,0,0,0)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard vertex:vert addshadow

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _BumpMap;

		struct Input 
		{
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		//float _Wavelength;
		float _Frequency;
        float _Amplitude;

		float _Speed; 

		float _Speed2; 
		float _Amplitude2;

		float4 _WindDirection;

        void vert (inout appdata_full v)
        {
            float X = v.texcoord.x;
            float weight = 1 - pow(X*2 -1 ,2);

            v.vertex.y += sin((_Time.y * _Speed - X) * _Frequency) * _Amplitude * weight;

            v.vertex.x += sin(_Time.y * _Speed2) * _Amplitude2 * weight * _WindDirection.x;
            v.vertex.z += sin(_Time.y * _Speed2) * _Amplitude2 * weight * _WindDirection.z;
        }
 

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
	
			
			float2 uv = IN.uv_MainTex;
			fixed4 c = tex2D (_MainTex, uv) * _Color;
			o.Albedo = c.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;

			o.Normal = UnpackNormal (tex2D (_BumpMap, uv));

		}
		ENDCG
	}
	FallBack "Diffuse"
}
