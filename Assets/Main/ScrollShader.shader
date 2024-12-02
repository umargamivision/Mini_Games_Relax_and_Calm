Shader "Custom/SpriteScrollShader"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _ScrollX ("Scroll Speed X", Float) = 0.1
        _ScrollY ("Scroll Speed Y", Float) = 0.1
        _Color ("Tint", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        Lighting Off
        ZWrite Off
        Fog { Mode Off }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 texcoord : TEXCOORD0;
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            float _ScrollX;
            float _ScrollY;
            float4 _Color;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord; // Pass UVs as-is for SpriteRenderer compatibility
                o.color = v.color * _Color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Adjust UVs with scrolling
                float2 uv = i.texcoord;
                uv.x += _ScrollX * _Time.y;
                uv.y += _ScrollY * _Time.y;

                // Sample texture and apply alpha
                fixed4 texColor = tex2D(_MainTex, uv);
                texColor.a *= i.color.a; // Combine with vertex alpha
                return texColor * i.color;
            }
            ENDCG
        }
    }
}