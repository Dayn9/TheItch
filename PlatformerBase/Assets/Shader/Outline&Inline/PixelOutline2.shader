Shader "Unlit/PixelOutline2"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Color ("Outline", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

			fixed4 _Color;
			float4 _MainTex_TexelSize;

            fixed4 frag (v2f i) : SV_Target
            {
				half4 c = tex2D(_MainTex, i.uv);
				c.rgb *= c.a;
				half4 outlineC = _Color;
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
