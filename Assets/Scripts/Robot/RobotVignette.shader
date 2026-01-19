Shader "Hidden/RobotVignette"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
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

            sampler2D _MainTex;
            float4 _RobotPosition;
            float _VisibleRadius;
            float _FadeDistance;
            float4 _DarknessColor;
            float _MaxDarkness;
            float _AspectRatio;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                
                // Calculate distance from current pixel to robot position
                // Adjust for aspect ratio to make circular vignette
                float2 adjustedUV = i.uv;
                adjustedUV.x = (adjustedUV.x - 0.5) * _AspectRatio + 0.5;
                
                float2 adjustedRobotPos = _RobotPosition.xy;
                adjustedRobotPos.x = (adjustedRobotPos.x - 0.5) * _AspectRatio + 0.5;
                
                float dist = distance(adjustedUV, adjustedRobotPos);
                
                // Calculate vignette strength based on distance
                // Normalize distance by screen height to make radius consistent
                float normalizedDist = dist * 10.0; // Scale factor for visibility
                
                // Smooth transition from visible to dark
                float vignetteStrength = smoothstep(_VisibleRadius, _VisibleRadius + _FadeDistance, normalizedDist);
                vignetteStrength = vignetteStrength * _MaxDarkness;
                
                // Blend original color with darkness
                col.rgb = lerp(col.rgb, _DarknessColor.rgb, vignetteStrength);
                
                return col;
            }
            ENDCG
        }
    }
}