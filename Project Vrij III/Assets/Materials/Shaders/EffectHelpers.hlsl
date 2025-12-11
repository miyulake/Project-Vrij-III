// Approximations from http://chilliant.blogspot.com.au/2012/08/srgb-approximations-for-hlsl.html?m=1
inline half3 GammaToLinearSpace (half3 sRGB)
{
    return sRGB * (sRGB * (sRGB * 0.305306011h + 0.682171111h) + 0.012522878h);
}

inline half3 LinearToGammaSpace (half3 linRGB)
{
    linRGB = max(linRGB, half3(0.h, 0.h, 0.h));
    return max(1.055h * pow(linRGB, 0.416666667h) - 0.055h, 0.h);
}
//

// Bayer calculations
float4 Posterize(float4 value, float steps, float bayerValue)
{
    value.rgb = LinearToGammaSpace(value.rgb);
    value = floor(value * steps + bayerValue) / steps;
    value.rgb = GammaToLinearSpace(value.rgb);
    return value;
}

float GetBayer2x2(float2 p)
{
    const float bayer2[4] = { 0.0/4.0, 2.0/4.0, 3.0/4.0, 1.0/4.0 };
    int x = (uint)floor(p.x) % 2;
    int y = (uint)floor(p.y) % 2;
    return bayer2[y * 2 + x];
}

float GetBayer4x4(float2 p)
{
    const float bayer4[16] = {
        0.0/16.0, 8.0/16.0, 2.0/16.0, 10.0/16.0,
        12.0/16.0, 4.0/16.0, 14.0/16.0, 6.0/16.0,
        3.0/16.0, 11.0/16.0, 1.0/16.0, 9.0/16.0,
        15.0/16.0, 7.0/16.0, 13.0/16.0, 5.0/16.0
    };
    int x = (uint)floor(p.x) % 4;
    int y = (uint)floor(p.y) % 4;
    return bayer4[y * 4 + x];
}

float GetBayer8x8(float2 p)
{
    const float bayer8[64] = {
        0.0/64.0, 32.0/64.0, 8.0/64.0, 40.0/64.0, 2.0/64.0, 34.0/64.0, 10.0/64.0, 42.0/64.0,
        48.0/64.0,16.0/64.0,56.0/64.0,24.0/64.0,50.0/64.0,18.0/64.0,58.0/64.0,26.0/64.0,
        12.0/64.0,44.0/64.0,4.0/64.0,36.0/64.0,14.0/64.0,46.0/64.0,6.0/64.0,38.0/64.0,
        60.0/64.0,28.0/64.0,52.0/64.0,20.0/64.0,62.0/64.0,30.0/64.0,54.0/64.0,22.0/64.0,
        3.0/64.0,35.0/64.0,11.0/64.0,43.0/64.0,1.0/64.0,33.0/64.0,9.0/64.0,41.0/64.0,
        51.0/64.0,19.0/64.0,59.0/64.0,27.0/64.0,49.0/64.0,17.0/64.0,57.0/64.0,25.0/64.0,
        15.0/64.0,47.0/64.0,7.0/64.0,39.0/64.0,13.0/64.0,45.0/64.0,5.0/64.0,37.0/64.0,
        63.0/64.0,31.0/64.0,55.0/64.0,23.0/64.0,61.0/64.0,29.0/64.0,53.0/64.0,21.0/64.0
    };
    int x = (uint)floor(p.x) % 8;
    int y = (uint)floor(p.y) % 8;
    return bayer8[y * 8 + x];
}

float GetBayer(float2 p, int bayerSize)
{
    return (bayerSize==2) ? GetBayer2x2(p) :
           (bayerSize==4) ? GetBayer4x4(p) :
                            GetBayer8x8(p);
}

void Dither_float(float4 pixelPosition, float4 color, float steps, float renderScale, int bayerSize, out float4 result)
{
    float2 p = pixelPosition.xy * _ScreenParams.xy * renderScale;
    float bayer = GetBayer(p, bayerSize);
    result = Posterize(color, steps, bayer);
}
//

// Scanline calculations
void Scanline_float(float4 pixelPosition, float4 color, float intensity, float frequency, out float4 result)
{
    float2 uv = pixelPosition.xy / _ScreenParams.xy; 
    float scan = sin(uv.y * _ScreenParams.y * frequency * 3.14159); 
    color.rgb *= 1.0 - intensity * (1.0 - scan); 
    result = color;
}
//

// Phosphor calculations
float Hash21(float2 p)
{
    p = frac(p * float2(123.34, 345.45));
    p += dot(p, p + 34.345);
    return frac(p.x * p.y);
}


void PhosphorNoise_float(float4 pixelPosition, float4 color, float intensity, float speed, out float4 result)
{
    float2 uv = pixelPosition.xy / _ScreenParams.xy;
    float n = Hash21(uv * _ScreenParams.xy + _Time.y * speed);
    float3 grain = float3(
        n,
        Hash21(uv * _ScreenParams.xy + 23.1 + _Time.y * speed),
        Hash21(uv * _ScreenParams.xy + 47.2 + _Time.y * speed)
    );
    color.rgb += (grain - 0.5) * intensity;
    result = color;
}
//

void ApplyEffects_float(float4 pixelPosition, float4 color,
    float ditherSteps, float ditherScale, int bayerSize,
    float scanIntensity, float scanFrequency,
    float noiseIntensity, float noiseSpeed,
    out float4 result)
{
    float4 temp; // Store output color

    // Dither
    Dither_float(pixelPosition, color, 
        ditherSteps, ditherScale, bayerSize, temp);

    // Scanline
    Scanline_float(pixelPosition, temp, 
        scanIntensity, scanFrequency, temp);

    // Noise
    PhosphorNoise_float(pixelPosition, temp,
        noiseIntensity, noiseSpeed, result);
}