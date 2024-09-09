Shader "Custom/OutlineSpriteShader"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (1,0,0,1)
        _OutlineThickness ("Outline Thickness", Range(0.0, 0.03)) = 0.015
    }

    SubShader
    {
        Tags{"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert alpha:blend nofog

        sampler2D _MainTex;
        fixed4 _OutlineColor;
        half _OutlineThickness;

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf(Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
            o.Alpha = c.a;

            // Outline logic
            if (c.a == 0)
            {
                o.Emission = _OutlineColor.rgb * _OutlineColor.a;
            }
        }
        ENDCG
    }

    Fallback "Diffuse"
}
