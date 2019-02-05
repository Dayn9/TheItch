Shader "Unlit/PixelOutline"
{
    Properties
    {
		//sprite sheet treated as main texture
        _MainTex ("Texture", 2D) = "white" {}
		_OutlineColor ("Outline Color", Color) = (1, 1, 1, 1)
    }

    SubShader
    {
		//turn off culling
		Cull Off
		Blend One OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vertFunc
            #pragma fragment fragFunc
            #include "UnityCG.cginc"

            sampler2D _MainTex;

			struct v2f {
				float4 pos : SV_POSITION;
				half2 uv : TEXTCOORD0;
			};

			fixed4 _OutlineColor;

			v2f vertFunc(appdata_base v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;
				return o;
			}

			float4 _MainTex_TexelSize;

			fixed fragFunc(v2f i) : COLOR {
				half4 c = tex2D(_MainTex, i.uv);
				c.rgb *= c.a;
				half4 outlineC = _OutlineColor;
				outlineC.a *= ceil(c.a);
				outlineC.rgb *= outlineC.a;

				fixed upAlpha = tex2D(_MainTex, i.uv + fixed2(0, _MainTex_TexelSize.y)).a;
				fixed downAlpha = tex2D(_MainTex, i.uv - fixed2(0, _MainTex_TexelSize.y)).a;
				fixed rightAlpha = tex2D(_MainTex, i.uv + fixed2(_MainTex_TexelSize.x, 0)).a;
				fixed leftAlpha = tex2D(_MainTex, i.uv - fixed2(_MainTex_TexelSize.x, 0)).a;

				return lerp(outlineC, c, ceil(upAlpha * downAlpha * rightAlpha * leftAlpha));

				return c;
			}

            ENDCG
        }
    }
}
