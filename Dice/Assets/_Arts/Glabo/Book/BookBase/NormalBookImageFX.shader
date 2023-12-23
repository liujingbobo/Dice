Shader "SteepleEffect/NormalBookImageFX"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Threshold ("Threshold", Range(0,255)) = 48
        _ThresholdUpper ("ThresholdUpper", Range(0,255)) = 144
        _LightColor ("Light Color", Color) = (1,1,1,1)
        _DarkColor ("Dark Color", Color) = (1,1,1,1)
        _ShadowColor ("ShadowColor", Color) = (1,1,1,1)
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 _MainTex_ST;
            float4 _MainTex_TexelSize;
            sampler2D _MainTex;
            float _Threshold;
            float _ThresholdUpper;
            float4 _LightColor;
            float4 _DarkColor;
            float4 _ShadowColor;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                float rawGray = dot(col.rgb, fixed3(0.299,0.587,0.114));
                float threshold = _Threshold / 255;
                float thresholdUpper = _ThresholdUpper / 255;
                float a = step(threshold, rawGray);
                float l = saturate((rawGray - threshold) / (thresholdUpper - threshold));
                
                float4 dColor = l * _LightColor + (1 - l) * _DarkColor;
                col = a * dColor + (1 - a) * float4(1,1,1,0);
                
                if(a == 0)
                {
                    fixed2 size = _MainTex_TexelSize;
                    fixed2 Up = i.uv + fixed2(0, size.y);
                    fixed4 col_up = tex2D(_MainTex, Up);
                    float upGray = dot(col_up.rgb, fixed3(0.299,0.587,0.114));
                    float a_up = step(threshold, upGray);
                    col = a_up * _ShadowColor + (1 - a_up) * col;
                }
                return col;
            }
            ENDCG
        }
    }
}
