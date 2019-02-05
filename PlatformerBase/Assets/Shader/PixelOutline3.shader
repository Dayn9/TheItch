Shader "Hidden/PixelOutline3"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		_Outline("Outline", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
	}
		SubShader
		{
			Tags
			{
				"Queue" = "Transparent"
				"IgnoreProjector" = "True"
				"RenderType" = "Transparent"
				"PreviewType" = "Plane"
				"CanUseSpriteAtlas" = "True"
			}

			Cull Off
			Lighting Off
			ZWrite Off
			Blend One OneMinusSrcAlpha

			Pass
			{
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile _ PIXELSNAP_ON
				#include "UnityCG.cginc"

				struct appdata_t
				{
					float4 vertex   : POSITION;
					float4 color    : COLOR;
					float2 texcoord : TEXCOORD0;
				};

				struct v2f
				{
					float4 vertex   : SV_POSITION;
					fixed4 color : COLOR;
					float2 texcoord  : TEXCOORD0;
				};

				fixed4 _Color;
				fixed4 _Outline;

				v2f vert(appdata_t IN)
				{
					v2f OUT;
					OUT.vertex = UnityObjectToClipPos(IN.vertex);
					OUT.texcoord = IN.texcoord;
					OUT.color = IN.color * _Color;
					#ifdef PIXELSNAP_ON
					OUT.vertex = UnityPixelSnap(OUT.vertex);
					#endif

					return OUT;
				}

				sampler2D _MainTex;
				sampler2D _AlphaTex;
				float _AlphaSplitEnabled;
				float4 _MainTex_TexelSize;

				fixed4 SampleSpriteTexture(float2 uv)
				{
					fixed4 color = tex2D(_MainTex, uv);

	#if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
					if (_AlphaSplitEnabled)
						color.a = tex2D(_AlphaTex, uv).r;
	#endif //UNITY_TEXTURE_ALPHASPLIT_ALLOWED

					return color;
				}

				fixed4 frag(v2f IN) : SV_Target
				{
					fixed4 c = SampleSpriteTexture(IN.texcoord) * IN.color;
					c.rgb *= c.a;
					
					half4 outlineC = _Outline;
					//outlineC.a *= ceil(c.a);
					outlineC.rgb *= outlineC.a;
					
					fixed myAlpha = c.a;
					fixed upAlpha = tex2D(_MainTex, IN.texcoord + fixed2(0, _MainTex_TexelSize.y)).a;
					fixed downAlpha = tex2D(_MainTex, IN.texcoord - fixed2(0, _MainTex_TexelSize.y)).a;
					fixed rightAlpha = tex2D(_MainTex, IN.texcoord + fixed2(_MainTex_TexelSize.x, 0)).a;
					fixed leftAlpha = tex2D(_MainTex, IN.texcoord - fixed2(_MainTex_TexelSize.x, 0)).a;

					return lerp(c, outlineC, ceil( clamp(downAlpha + upAlpha + leftAlpha + rightAlpha, 0, 1) ) - ceil(myAlpha));
				}
			ENDCG
			}
		}
}
