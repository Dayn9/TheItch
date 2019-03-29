// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Unlit/Fadeoff"
{
    Properties
    {
		[PerRendererData] _MainTex ("Texture", 2D) = "white" {}
		_StartY("Start Y", Float) = 0
		_FadeRate("Faderate", Range(0, 1)) = 0.1
		
    }
    SubShader
    {
        Tags { 
			"RenderType"="Transparent" 
			"Queue" = "Transparent"
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

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
				float yPos : FLOAT;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

			float _FadeRate;
			float _StartY;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.yPos = worldPos.y;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);

				if (i.yPos > _StartY) { return 0; }

				float distFromStartY = abs((i.yPos + i.uv.y) - _StartY);

				col.a *= 1 - distFromStartY * _FadeRate;

				//col.a = _FadeRate;
				//col.rgb += 1-col.a;
				col.rgb *= col.a;
                
				return col;
            }
            ENDCG
        }
    }
}
