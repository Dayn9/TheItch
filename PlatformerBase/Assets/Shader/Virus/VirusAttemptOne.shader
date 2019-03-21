Shader "Unlit/VirusAttemptOne"
{
    Properties
    {
		_MainTex ("Texture", 2D) = "white" {}
		_TransitionTex("Transition Texture", 2D) = "white" {}
		_Color("Screen Color", Color) = (1,1,1,1)	
		_Cutoff("Cutoff ", Range(0, 1)) = 0
		
    }
    SubShader
    {

			Tags
			{
				"Queue" = "Transparent"
				"RenderType" = "Transparent"
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
					float2 texcoord : TEXCOORD0;
				};

				struct v2f
				{
					float4 vertex   : SV_POSITION;
					float2 texcoord  : TEXCOORD0;
				};

				sampler2D _TransitionTex;
				sampler2D _MainTex;
				float4 _MainTex_TexelSize;
				float _Cutoff;
				fixed4 _Color;

				v2f vert(appdata_t IN)
				{
					v2f OUT;
					OUT.vertex = UnityObjectToClipPos(IN.vertex);
					OUT.texcoord = IN.texcoord;
					return OUT;
				}

				fixed4 frag (v2f IN) : SV_Target
				{
					fixed4 transit = tex2D(_TransitionTex, IN.texcoord);

					if (transit.r < _Cutoff + 0.01) {
						return _Color * transit.a;
					}

					return 0;
				}
				
				ENDCG
        }
    }
}
