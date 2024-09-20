Shader "Custom/ModifiedStandard"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0.0,1.0)) = 0.5
        _Metallic ("Metallic", Range(0.0,1.0)) = 0.0
        _BumpMap ("Normal Map", 2D) = "bump" {}
        _EmissionColor ("Emission Color", Color) = (0,0,0)
        _EmissionMap ("Emission Map", 2D) = "black" {}
        _OcclusionStrength ("Occlusion Strength", Range(0, 1)) = 1
        _Queue ("Render Queue", Float) = 3000 // Render queue modifiable
    }
    
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        sampler2D _MainTex;
        sampler2D _BumpMap;
        fixed4 _Color;
        half _Glossiness;
        half _Metallic;
        fixed3 _EmissionColor;
        sampler2D _EmissionMap;
        half _OcclusionStrength;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_BumpMap;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo
            fixed4 albedo = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = albedo.rgb;

            // Normal map
            o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));

            // Metallic and smoothness
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;

            // Emission
            fixed3 emission = tex2D(_EmissionMap, IN.uv_MainTex).rgb * _EmissionColor.rgb;
            o.Emission = emission;

            // Occlusion
            o.Occlusion = _OcclusionStrength;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
