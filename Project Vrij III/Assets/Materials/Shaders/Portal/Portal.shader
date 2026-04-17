// Original by shader_daily https://www.shadertoy.com/view/DdXyRl#
// Based on Morgan McGuire @morgan3d
// https://www.shadertoy.com/view/4dS3Wd
// Unity conversion by miyulake

Shader "Miyu/Portal"
{
    Properties
    {
        [Header(Colors)]
        [HDR] _MainColor1("Base Color 1", Color) = (0.101961,0.619608,0.666667)
        [HDR] _MainColor2("Base Color 2", Color) = (0.666667,0.666667,0.498039)
        [HDR] _BackgroundColor("Background Color", Color) = (0,0,0.164706)
        [HDR] _HighlightColor("Highlight Color", Color) = (0.666667,1,1)

        [Header(Distortion)]
        _Speed("Animation Speed", Range(0, 10)) = 1
        _RotationSpeed("Rotation Speed", Range(0,5)) = 0.15
        _FbmAmplitude("FBM Amplitude", Range(0,2)) = 0.65
        _FbmLacunarity("FBM Lacunarity", Range(0,4)) = 2.0
        _FbmOctaves("FBM Octaves", Integer) = 5
        _SpiralScale("Spiral Scale", Range(0.1,2.0)) = 0.3
        _SpiralOffset("Spiral Offset", Range(-1,1)) = 0.95
        _SpiralBias("Spiral Bias", Range(0.001, 0.01)) = 0.01

        [Header(Effects)]
        _Brightness("Brightness", Range(0,2)) = 1
        _Contrast("Contrast", Range(0,2)) = 1
        [ShowAsVector2] _PortalCenter("Portal Center", Vector) = (0.5, 0.5, 0)
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "Queue" = "Overlay" }

        Pass
        {
            ZWrite Off
            Cull Back
            ZTest LEqual

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes { float4 positionOS : POSITION; float2 uv : TEXCOORD0; };
            struct Varyings { float4 positionCS : SV_POSITION; float2 uv : TEXCOORD0; };

            float _Speed, _RotationSpeed, _FbmAmplitude, _FbmLacunarity, _SpiralScale,
                _SpiralOffset, _SpiralBias, _Brightness, _Contrast;
            float2 _PortalCenter;
            float3 _MainColor1, _MainColor2, _HighlightColor, _BackgroundColor;
            int _FbmOctaves;

            Varyings Vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                return OUT;
            }

            float random(float2 _st) { return frac(sin(dot(_st, float2(12.9898,78.233))) * 43758.5453123); }

            float noise(float2 _st)
            {
                float2 i = floor(_st);
                float2 f = frac(_st);

                float a = random(i);
                float b = random(i + float2(1.0, 0.0));
                float c = random(i + float2(0.0, 1.0));
                float d = random(i + float2(1.0, 1.0));

                float2 u = f * f * (3.0 - 2.0 * f);

                return lerp(a, b, u.x) + (c - a) * u.y * (1.0 - u.x) + (d - b) * u.x * u.y;
            }

            float fbm(float2 _st)
            {
                float v = 0.0;
                float a = _FbmAmplitude;
                float2 shift = float2(100.0, 100.0);
                float2x2 rot = float2x2(cos(0.5), sin(0.5), -sin(0.5), cos(0.5));

                for (int i = 0; i < _FbmOctaves; ++i)
                {
                    v += a * noise(_st);
                    _st = mul(rot, _st * _FbmLacunarity) + shift;
                    a *= 0.5;
                }
                return v;
            }

            float4 Frag(Varyings IN) : SV_Target
            {
                float t = _Time.y * _Speed;

                float2 fragCoord = IN.uv * _ScreenParams.xy;
                float2 uResolution = _ScreenParams.xy;
                uResolution.y *= 1.5;
                float2 vUv = fragCoord / uResolution.xy;

                float2 uv2 = vUv - _PortalCenter.xy; // allow center movement

                // Portal
                uv2.y += 0.125;
                float an = -t * _RotationSpeed;
                float cosA = cos(an);
                float sinA = sin(an);
                float2x2 rot = float2x2(cosA, -sinA, sinA, cosA);
                uv2 = mul(rot, uv2);

                float r1 = abs(length(uv2));
                float a = abs(atan2(uv2.y, uv2.x));
                uv2 = float2(_SpiralScale / (r1 + _SpiralBias) + _SpiralOffset * t, a);

                float2 q;
                q.x = fbm(uv2 + 0.0 * t);
                q.y = fbm(uv2 + float2(1.0,0.0));

                float2 r;
                r.x = fbm(uv2 + q + float2(1.7,9.2) + 0.15 * t);
                r.y = fbm(uv2 + q + float2(8.3,2.8) + 0.126 * t);

                float f = fbm(uv2 + r * fbm(uv2 + r * 2.0));

                float3 baseCol = lerp(_MainColor1.rgb, _MainColor2.rgb, saturate((f * f) * 4.0));
                float3 colQ = lerp(baseCol, _BackgroundColor.rgb, saturate(length(q)));
                float3 colR = lerp(colQ, _HighlightColor.rgb, saturate(length(r.x)));

                // brightness / contrast
                float3 color = pow(max(colR, 0.0), float3(_Contrast, _Contrast, _Contrast)) * _Brightness;

                return float4(color, 1.0);
            }
            ENDHLSL
        }
    }
}