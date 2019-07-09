Shader "Custom/TargetShader"
{
    Properties
    {
        _BaseColour ("Base Colour", Color) = (1,1,1,1)
        _FillColour ("Fill Colour", Color) = (0,0,0,1)
        _Fill ("Fill", Range(0,1)) = 0
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _Texture ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        struct Input
        {
            float2 uv_Texture;
        };

        fixed4 _BaseColour;
        fixed4 _FillColour;
        half _Fill;
        half _Glossiness;
        half _Metallic;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = _BaseColour;
            if ((IN.uv_Texture.y < _Fill && _Fill > 1e-4) || _Fill > 0.9999) {
                c = _FillColour;
            }
            o.Albedo = c.rgb;
            o.Alpha = c.a;

            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
