Shader "Unlit/InlineOptimized"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		_Outline("Outline", Color) = (1,1,1,1)
	}
    SubShader
    { 
			Tags
			{
				"Queue" = "Transparent"
				"IgnoreProjector" = "True"
				"RenderType" = "Transparent"
				"PreviewType" = "Plane"
				"CanUseSpriteAtlas" = "False"
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
				sampler2D _MainTex;
				float4 _MainTex_TexelSize;

				v2f vert(appdata_t IN)
				{
					v2f OUT;
					OUT.vertex = UnityObjectToClipPos(IN.vertex);
					OUT.texcoord = IN.texcoord;
					OUT.color = IN.color * _Color;
					return OUT;
				}

				fixed4 frag(v2f IN) : SV_Target
				{
					fixed4 c = tex2D(_MainTex, IN.texcoord) * IN.color;
					c.rgb *= c.a;

					fixed4 o = _Outline;
					o.rgb *= o.a;

					return lerp(c, o,
						(1 - ceil(tex2D(_MainTex, IN.texcoord - fixed2(0, _MainTex_TexelSize.y)).a *
							tex2D(_MainTex, IN.texcoord + fixed2(0, _MainTex_TexelSize.y)).a *
							tex2D(_MainTex, IN.texcoord - fixed2(_MainTex_TexelSize.x, 0)).a *
							tex2D(_MainTex, IN.texcoord + fixed2(_MainTex_TexelSize.x, 0)).a)) * ceil(c.a)
					);
				}
			ENDCG
		}
    }
}
