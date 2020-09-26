//==============================================================================
// Copyright (c) 2020 Kishimoto Studios. All Rights Reserved.
// contact@kishimotostudios.com
//==============================================================================

Shader "_VikingBox/Letter and Envelope"
{
    Properties
    {
        _MainTexture ("Main Texture", 2D) = "white" {}
        _Multiplier("Offset Multiplier", Float) = 0.180
        [IntRange]_CurrentIndex ("Current Index", Range(0, 4)) = 0 // index=0 is also used for the envelope.
        
        // Both properties below will change Y-axis mapping.
        _MultiplierEnvelope("Offset Multiplier Y (Envelope)", Float) = 0.5
        [IntRange]_CurrentIndexEnvelope ("Current Index Y (Envelope)", Range(0, 1)) = 0
    }

    SubShader
    {
		Tags
		{
			"Queue" = "Transparent"
			"RenderType" = "Transparent"
			"IgnoreProjector" = "True"
		}
		
        Pass
        {
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
			
            CGPROGRAM

            sampler2D _MainTexture;
            half _Multiplier;
            half _MultiplierEnvelope;
            int _CurrentIndex;
            int _CurrentIndexEnvelope;

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
                output.uv.x += _CurrentIndex * _Multiplier;
                output.uv.y += _CurrentIndexEnvelope * _MultiplierEnvelope;

                return output;
            }

            fixed4 fragmentMain(v2f input) : SV_TARGET
            {
                return tex2D(_MainTexture, input.uv);
            }

            ENDCG
        }
    }
}