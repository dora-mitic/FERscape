Shader "Hidden/RadialStripes"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color1 ("Color 1", Color) = (1, 0.8, 0, 1)
        _Color2 ("Color 2", Color) = (1, 0.95, 0.3, 1)
        _StripeCount ("Stripe Count", Float) = 16
        _Rotation ("Rotation", Float) = 0
        _Alpha ("Alpha", Range(0, 1)) = 0.8
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
        
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

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
            float4 _Color1;
            float4 _Color2;
            float _StripeCount;
            float _Rotation;
            float _Alpha;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Center the UV coordinates
                float2 centered = i.uv - 0.5;
                
                // Calculate angle from center
                float angle = atan2(centered.y, centered.x);
                
                // Convert to degrees and add rotation
                angle = degrees(angle) + _Rotation;
                
                // Normalize to 0-360
                angle = fmod(angle + 360.0, 360.0);
                
                // Create stripes
                float stripe = fmod(angle / 360.0 * _StripeCount, 2.0);
                stripe = step(1.0, stripe);
                
                // Blend between two colors
                fixed4 col = lerp(_Color1, _Color2, stripe);
                
                // Apply alpha
                col.a = _Alpha;
                
                return col;
            }
            ENDCG
        }
    }
}