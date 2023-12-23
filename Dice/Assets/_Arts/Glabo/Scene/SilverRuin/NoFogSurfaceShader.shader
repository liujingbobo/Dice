Shader "Custom/NoFogSurfaceShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _AutoFadeMetallic ("Metallic Fade Z", Range(0,1)) = 0.0
        _EmissionTex ("Emission Texture", 2D) = "white" {}
        [HDR]
        _Emission ("Emission", Color) = (0,0,0,0)
        
        _ShadowTex ("ShadowTex", 2D) = "white" {}
        _MapBound("MapBound", vector) = (0,0,1,1)
        _Cutoff("Cutoff",Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags 
        { 
            "RenderType"="Cutout"
            "Queue" = "Transparent"
            "LightMode" = "ShadowCaster"
        }
        LOD 200

        CGPROGRAM        
        // Physically based Standard lighting model, and enable shadows on all light types
        //#pragma surface surf Standard fullforwardshadows
        #pragma surface surf Standard fullforwardshadows nofog alphatest:_Cutoff
        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _EmissionTex;
        sampler2D _ShadowTex;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos : TEXCOORD2;
        };

        half _Glossiness;
        half _Metallic;
        half _AutoFadeMetallic;
        half3 _Emission;
        fixed4 _Color;
        float4 _MapBound;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)
        
        fixed4 SteepleShadowCast(sampler2D shadowMap,float3 worldPos,float4 mapBound)
        {
            fixed2 coord = (worldPos.xy - mapBound.xy) / mapBound.zw;
            return tex2D(shadowMap,coord);
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float3 p = IN.worldPos;
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;

            half3 e = tex2D (_EmissionTex, IN.uv_MainTex).rgb * _Emission;
            fixed4 shadowColor = SteepleShadowCast(_ShadowTex,p,_MapBound);
			c.rgb *= shadowColor.rgb;
            
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            //o.Metallic = saturate(1- shadowColor.r + p.z * 0.5 + pow(p.x * p.x + p.y * p.y, 0.5) * 0.25);
            o.Metallic = lerp(_Metallic, saturate(1- shadowColor.r + p.z * 0.5),_AutoFadeMetallic);
            o.Smoothness = _Glossiness;
            o.Emission = e;
            o.Alpha = c.a;
            
            //clip(c.a - 0.001);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
