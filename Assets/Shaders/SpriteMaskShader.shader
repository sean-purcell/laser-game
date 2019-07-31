Shader "Custom/FillSpriteShader"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _FillColor ("Fill Color", Color) = (0,0,1,1)
        _Fill ("Fill", Range(0,1)) = 0
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
    }

    SubShader
    {
        Tags
        { 
            "Queue"="Transparent" 
            "IgnoreProjector"="True" 
            "RenderType"="TransparentCutout" 
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

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
                float2 texcoord : TEXCOORD0;

            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                float2 texcoord  : TEXCOORD0;
            };
            
            fixed4 _Color;
            fixed4 _FillColor;

            float _Fill;

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = IN.texcoord;
                #ifdef PIXELSNAP_ON
                OUT.vertex = UnityPixelSnap (OUT.vertex);
                #endif

                return OUT;
            }

            sampler2D _MainTex;

            fixed4 SampleSpriteTexture (float2 uv)
            {
                fixed4 color = tex2D (_MainTex, uv);

                return color;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                fixed4 c = _Color;
                if (IN.texcoord.y < _Fill) {
                    c = _FillColor;
                }
                c.a = SampleSpriteTexture(IN.texcoord).a;
                return c;
            }
        ENDCG
        }
    }
}
