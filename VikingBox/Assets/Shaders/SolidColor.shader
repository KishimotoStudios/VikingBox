//==============================================================================
// Copyright (c) 2020 Kishimoto Studios. All Rights Reserved.
// contact@kishimotostudios.com
//==============================================================================

Shader "_VikingBox/Solid Color"
{
    Properties
    {
        _Color ("Main Color", Color) = (1.0, 1.0, 1.0, 1.0)
    }

    SubShader
    {
        Pass
        {
            CGPROGRAM

            fixed4 _Color;

            #pragma vertex vertexMain
            #pragma fragment fragmentMain

            struct appdata
            {
                float4 pos : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            v2f vertexMain(appdata input)
            {
                v2f output;
                output.pos = UnityObjectToClipPos(input.pos);

                return output;
            }

            fixed4 fragmentMain(v2f input) : SV_TARGET
            {
                return _Color;
            }

            ENDCG
        }
    }
}