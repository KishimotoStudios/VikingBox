//==============================================================================
// Copyright (c) 2020 Kishimoto Studios. All Rights Reserved.
// contact@kishimotostudios.com
//==============================================================================

Shader "_VikingBox/Keyboard Tab"
{
    Properties
    {
        _MainTexture ("Main Texture", 2D) = "white" {}
        _Color("Main Color", Color) = (1.0, 1.0, 1.0, 1.0)
    }

    SubShader
    {
        Pass
        {
            CGPROGRAM

            sampler2D _MainTexture;
            fixed4 _Color;

            #pragma vertex vertexMain
            #pragma fragment fragmentMain

            struct appdata
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vertexMain(appdata input)
            {
                v2f output;
                output.pos = UnityObjectToClipPos(input.pos);
                output.uv = input.uv;

                return output;
            }

            fixed4 fragmentMain(v2f input) : SV_TARGET
            {
                return tex2D(_MainTexture, input.uv) * _Color;
            }

            ENDCG
        }
    }
}