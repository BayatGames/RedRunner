// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/UI_BGBlur"
{
	Properties
	{
		_Size("Size", float) = 1
	}
	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100

		GrabPass {Tags{"LightMode" = "Always"}}

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 uvgrab : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _GrabTexture;
			float4 _GrabTexture_TexelSize;
			float _Size;

			v2f vert(appdata_base v) {
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				#if UNITY_UV_STARTS_AT_TOP
				float scale = -1.0;
				#else
				float scale = 1.0;
				#endif
				o.uvgrab.xy = (float2(o.vertex.x, o.vertex.y*scale) + o.vertex.w) * 0.5;
				o.uvgrab.zw = o.vertex.zw;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				// sample the texture
				// half4 col = tex2Dproj( _GrabTexture, UNITY_PROJ_COORD(i.uvgrab));
				// return col;

				half4 sum = half4(0,0,0,0);

				#define GRABPIXEL(weight,kernelx) tex2Dproj( _GrabTexture, UNITY_PROJ_COORD(float4(i.uvgrab.x + _GrabTexture_TexelSize.x * kernelx*_Size, i.uvgrab.y, i.uvgrab.z, i.uvgrab.w))) * weight

				sum += GRABPIXEL(0.05, -4.0);
				sum += GRABPIXEL(0.09, -3.0);
				sum += GRABPIXEL(0.12, -2.0);
				sum += GRABPIXEL(0.15, -1.0);
				sum += GRABPIXEL(0.18, 0.0);
				sum += GRABPIXEL(0.15, +1.0);
				sum += GRABPIXEL(0.12, +2.0);
				sum += GRABPIXEL(0.09, +3.0);
				sum += GRABPIXEL(0.05, +4.0);

				return sum;
			}
			ENDCG
		}

		GrabPass{ Tags{ "LightMode" = "Always" } }

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 uvgrab : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _GrabTexture;
			float4 _GrabTexture_TexelSize;
			float _Size;

			v2f vert(appdata_base v) {
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				#if UNITY_UV_STARTS_AT_TOP
				float scale = -1.0;
				#else
				float scale = 1.0;
				#endif
				o.uvgrab.xy = (float2(o.vertex.x, o.vertex.y*scale) + o.vertex.w) * 0.5;
				o.uvgrab.zw = o.vertex.zw;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				// sample the texture
				// half4 col = tex2Dproj( _GrabTexture, UNITY_PROJ_COORD(i.uvgrab));
				// return col;

				half4 sum = half4(0,0,0,0);

				#define GRABPIXEL(weight,kernely) tex2Dproj( _GrabTexture, UNITY_PROJ_COORD(float4(i.uvgrab.x, i.uvgrab.y + _GrabTexture_TexelSize.y * kernely*_Size, i.uvgrab.z, i.uvgrab.w))) * weight

				sum += GRABPIXEL(0.05, -4.0);
				sum += GRABPIXEL(0.09, -3.0);
				sum += GRABPIXEL(0.12, -2.0);
				sum += GRABPIXEL(0.15, -1.0);
				sum += GRABPIXEL(0.18, 0.0);
				sum += GRABPIXEL(0.15, +1.0);
				sum += GRABPIXEL(0.12, +2.0);
				sum += GRABPIXEL(0.09, +3.0);
				sum += GRABPIXEL(0.05, +4.0);

				return sum;
			}
			ENDCG
		}
	}
	Fallback "UI/Unlit/Transparent"
}